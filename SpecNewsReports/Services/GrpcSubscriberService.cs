using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Options;
using Settings;


namespace SpecNewsReports.Services
{
	public class GrpcSubscriberService : BackgroundService
	{
		private readonly IConfiguration _configuration;
		private readonly IHostApplicationLifetime _lifeTime;
		private readonly ILogger<GrpcSubscriberService> _logger;
		private readonly EventProvider _eventProvider;
		private readonly IOptions<AppSettings> _appSettings;

		public GrpcSubscriberService(IConfiguration configuration,
			IHostApplicationLifetime lifeTime, ILogger<GrpcSubscriberService> logger,
			EventProvider eventProvider, IOptions<AppSettings> appSettings)
		{
			_configuration = configuration;
			_lifeTime = lifeTime;
			_logger = logger;
			_eventProvider = eventProvider;
			_appSettings = appSettings;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			string[] events = _configuration.GetSection("Announcer:Events").GetChildren().ToArray().Select(c => c.Value).ToArray();
			string url = _configuration.GetSection("Announcer:URL").Get<string>();
			string subscriberName = _configuration.GetSection("Announcer:SubscriberName").Get<string>();
			var applicationUrl = Environment.GetEnvironmentVariable("ASPNETCORE_URLS")?.Split(";").First();

			using var channel = GrpcChannel.ForAddress(url);

			CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
			CancellationToken token = cancelTokenSource.Token;

			cancelTokenSource.CancelAfter(100);
			await channel.ConnectAsync(token);

			if (channel.State == ConnectivityState.Ready)
			{
				var client = new Subscribers.SubscribersClient(channel);
				foreach (var xevent in events)
				{
					var reply = client.Subscribe(new SubscribeRequest
					{
						Enable = true,
						SubscriberUniqueName = subscriberName,
						SubscriberUrl = applicationUrl,
						EventUniqueName = xevent,
						Priority = 1
					});

					if (reply.Status != 200)
					{
						_logger.LogInformation($"Error: {reply.Status} {reply.StatusMessage} (Event: {xevent})");
					}
					else
					{
						_logger.LogInformation($"Success: {reply.Status} {reply.StatusMessage} (Event: {xevent})");
					}
				}

				while (!stoppingToken.IsCancellationRequested)
				{
					var reply = client.GetSubscribe(new GetSubscribeRequest { SubscriberUniqueName = "SpecProfiles" });
					_appSettings.Value.SpecProfilesUrl = reply.SubscriberUrl;
					_logger.LogInformation($"SpecProfilesUrl: {_appSettings.Value.SpecProfilesUrl}\n");
					if (reply.Status == 200)
					{
						break;
					}
				}

				var call = client.SubscribeStream();

				var readTask = Task.Run(async () =>
				{
					await foreach (var reply in call.ResponseStream.ReadAllAsync(stoppingToken))
					{
						_logger.LogInformation($"NewEvent: {reply.Status} {reply.StatusMessage}\n" +
												   $"(Subscriber: {reply.SubscriberUniqueName}, Event: {reply.EventUniqueName}, " +
												   $"Date: {reply.EventDate})\nData: {reply.EventData}\n");
						_eventProvider.NotifySubscribers(new Event
						{
							Status = reply.Status,
							StatusMessage = reply.StatusMessage,
							SubscriberUniqueName = reply.SubscriberUniqueName,
							EventUniqueName = reply.EventUniqueName,
							EventDate = reply.EventDate.ToDateTime(),
							EventData = reply.EventData
						});
					}
				});

				await call.RequestStream.WriteAsync(new SubscribeStreamRequest
				{
					Enable = true,
					SubscriberUniqueName = subscriberName
				});
				await call.RequestStream.CompleteAsync();

				try
				{
					await readTask;
				}
				catch
				{

				}
				// application stopped
				_logger.LogInformation("Application stopped");
			}
			else
			{
				// SpecAnnouncer don't work
				_lifeTime.StopApplication();
			}
		}
	}
}
