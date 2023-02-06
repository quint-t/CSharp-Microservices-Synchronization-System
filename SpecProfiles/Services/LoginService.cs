using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using SpecProfiles.Data;

namespace SpecProfiles.Services
{
    public class LoginService : IHostedService
    {
        private readonly string[] EventTypes = { "LoginUser" };

        private readonly IConfiguration _configuration;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<LoginService> _logger;
        private readonly EventProvider _eventProvider;
        private Task? _serviceTask;

        public LoginService(IConfiguration configuration, IServiceScopeFactory scopeFactory,
            ILogger<LoginService> logger, EventProvider eventProvider)
        {
            _configuration = configuration;
            _scopeFactory = scopeFactory;
            _logger = logger;
            _eventProvider = eventProvider;
            _serviceTask = null;
            _eventProvider.Notify += OnMessageReceived;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _serviceTask = Task.Run(() => {
                cancellationToken.WaitHandle.WaitOne();
            });
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public void OnMessageReceived(Event @event)
        {
            if (EventTypes.Contains(@event.EventUniqueName))
            {
				var myJObject = JObject.Parse(@event!.EventData!);
				var userId = myJObject.SelectToken("Id")?.Value<string>();
				if (userId != null)
				{
					_logger.LogInformation($"User {userId} login!\n");
				}
				else
				{
					_logger.LogError("User id not specified in event data!");
				}
				_logger.LogInformation($"LoginService: {@event.Status} {@event.StatusMessage}\n" +
                                   $"(Subscriber: {@event.SubscriberUniqueName}, Event: {@event.EventUniqueName}, " +
                                   $"Date: {@event.EventDate})\nData: {@event.EventData}\n");
                using (var scope = _scopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    _logger.LogInformation($"Persons: {dbContext.Person.Count()}\n");
                    _logger.LogInformation($"Organizations: {dbContext.Organization.Count()}\n");
                }
            }
            else
            {
                _logger.LogInformation($"Event {@event.EventUniqueName} skipped!\n");
            }
        }
    }
}
