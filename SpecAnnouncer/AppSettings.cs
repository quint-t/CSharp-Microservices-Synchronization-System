namespace Settings
{
    public class AppSettings
    {
        public Application Application { get; set; } = new Application{ };
    }

    public class Application
    {
        public string Name { get; set; } = "";
        public string Version { get; set; } = "";
    }
}