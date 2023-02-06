namespace SpecNewsReports.Services
{
    public class Event
    {
        public uint Status { get; set; }

        public string? StatusMessage { get; set; }

        public string? SubscriberUniqueName { get; set; }

        public string? EventUniqueName { get; set; }

        public DateTime EventDate { get; set; }

        public string? EventData { get; set; }
    }
}
