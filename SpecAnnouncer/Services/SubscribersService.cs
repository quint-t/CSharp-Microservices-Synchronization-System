using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using SpecAnnouncer.Data;
using Microsoft.EntityFrameworkCore;

namespace SpecAnnouncer.Services
{
    public class SubscribersService : Subscribers.SubscribersBase
    {
        private readonly ILogger<SubscribersService> _logger;
        private readonly ApplicationDbContext _context;
        private readonly int _ms_delay = 500;

        public SubscribersService(ILogger<SubscribersService> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public override Task<SubscribeReply> Subscribe(SubscribeRequest request, ServerCallContext context)
        {
            var eventObject = _context.Events.FirstOrDefault(e => e.UniqueName == request.EventUniqueName);
            if (eventObject == null)
            {
                // event not found => create it!
                eventObject = new Event { UniqueName = request.EventUniqueName };
                _context.Events.Add(eventObject);
            }

            var subscriberObject = _context.Subscribers.FirstOrDefault(s => s.UniqueName == request.SubscriberUniqueName);
            if (subscriberObject == null)
            {
                // subscriber not found => create it!
                subscriberObject = new Subscriber { UniqueName = request.SubscriberUniqueName, Url = request.SubscriberUrl };
                _context.Subscribers.Add(subscriberObject);
            }
            else if (subscriberObject.Url != request.SubscriberUrl)
            {
                // urls not equals => change it!
                subscriberObject.Url = request.SubscriberUrl;
            }

            _context.SaveChanges();

            var p = _context.SubscribersSignatures.FirstOrDefault(s => s.Subscriber == subscriberObject && s.Event == eventObject);

            if (request.Enable)
            {
                if (p == null)
                {
                    p = new SubscriberSignature
                    {
                        Subscriber = subscriberObject,
                        Event = eventObject,
                        Priority = request.Priority
                    };
                    _context.Add(p);
                }
            }
            else
            {
                if (p != null)
                {
                    _context.Remove(p);
                }
            }

            _context.SaveChanges();

            return Task.FromResult(new SubscribeReply
            {
                Status = 200,
                StatusMessage = "OK",
            });
        }

        public override async Task SubscribeStream(IAsyncStreamReader<SubscribeStreamRequest> requestStream,
                IServerStreamWriter<SubscribeStreamReply> responseStream,
                ServerCallContext context)
        {
            List<string> subscribersUniqueNames = new List<string>();

            var readTask = Task.Run(async () =>
            {
                await foreach (SubscribeStreamRequest message in requestStream.ReadAllAsync())
                {
                    if (message.Enable && !subscribersUniqueNames.Contains(message.SubscriberUniqueName))
                    {
                        subscribersUniqueNames.Add(message.SubscriberUniqueName);
                    }
                    else if (!message.Enable && subscribersUniqueNames.Contains(message.SubscriberUniqueName))
                    {
                        subscribersUniqueNames.Remove(message.SubscriberUniqueName);
                    }
                }
            });

            while (!context.CancellationToken.IsCancellationRequested)
            {
                await Task.Delay(_ms_delay);
                foreach (var uniqueName in subscribersUniqueNames)
                {
                    var xs = _context.ResponsesHistory.Include(rh => rh.Subscriber)
                        .Include(rh => rh.EventHistory).Include(rh => rh.EventHistory.Event)
                        .Where(rh => rh.Subscriber != null && rh.Subscriber.UniqueName == uniqueName && rh.ResponseReceived == false);
                    
                    bool changed = false;
                    foreach (var x in xs)
                    {
                        await responseStream.WriteAsync(new SubscribeStreamReply
                        {
                            Status = 200,
                            StatusMessage = "OK",
                            SubscriberUniqueName = x.Subscriber?.UniqueName,
                            EventUniqueName = x.EventHistory?.Event?.UniqueName,
                            EventDate = x.EventHistory?.DateTime.ToTimestamp(),
                            EventData = x.EventHistory?.Data
                        });
                        x.ResponseReceived = true;
                        changed = true;
                    }
                    if (changed)
                    {
                        _context.SaveChanges();
                    }
                }
            }
            await readTask;
        }

        public override Task<GetSubscribeReply> GetSubscribe(GetSubscribeRequest request, ServerCallContext context)
        {
            var subscriberObject = _context.Subscribers.FirstOrDefault(s => s.UniqueName == request.SubscriberUniqueName);
            if (subscriberObject != null)
            {
                return Task.FromResult(new GetSubscribeReply
                {
                    Status = 200,
                    StatusMessage = "OK",
                    SubscriberUrl = subscriberObject.Url
                });
            }

            return Task.FromResult(new GetSubscribeReply
            {
                Status = 404,
                StatusMessage = "Not found",
                SubscriberUrl = ""
            });
        }
    }
}