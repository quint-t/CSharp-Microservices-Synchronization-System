using Grpc.Core;
using SpecAnnouncer.Data;
using Microsoft.EntityFrameworkCore;

namespace SpecAnnouncer.Services
{
	public class EventHandlerService : EventHandler.EventHandlerBase
	{
		private readonly ILogger<EventHandlerService> _logger;
		private readonly ApplicationDbContext _context;

		public EventHandlerService(ILogger<EventHandlerService> logger, ApplicationDbContext context)
		{
			_logger = logger;
			_context = context;
		}

		public override Task<NewEventReply> NewEvent(NewEventRequest request, ServerCallContext context)
		{
			try
			{
                var eventObject = _context.Events.FirstOrDefault(e => e.UniqueName == request.EventUniqueName);
                if (eventObject == null)
				{
                    // event not found => create it!
                    eventObject = new Event { UniqueName = request.EventUniqueName };
					_context.Events.Add(eventObject);
					_context.SaveChanges();
				}

                EventHistory eventHistory = new EventHistory
				{
					Event = eventObject,
					DateTime = request.EventDate.ToDateTime(),
					Data = request.EventData
				};
				_context.EventsHistory.Add(eventHistory);
                _context.SaveChanges();

				var signatures = _context.SubscribersSignatures.Include(ss => ss.Subscriber).Where(
					ss => ss.Event == eventObject
                ).OrderBy(ss => ss.Priority);

                foreach (var signature in signatures)
				{
					_context.ResponsesHistory.Add(new ResponseHistory
					{
						Subscriber = signature.Subscriber,
						EventHistory = eventHistory,
						ResponseReceived = false
					});
				}

                _context.SaveChanges();

                return Task.FromResult(new NewEventReply
				{
					Status = 200,
					StatusMessage = "OK",
				});
			}
			catch
			{
				return Task.FromResult(new NewEventReply
				{
					Status = 500,
					StatusMessage = "Internal Server Error",
				});
			}
		}
	}
}