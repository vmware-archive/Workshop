namespace CloudFoundry
{
  public class AppConfigurationOptions
    {
        public AppConfigurationOptions()
        {
            // Set default value.
            Option1 = "value1_from_ctor";
        }
        public string Option1 { get; set; }
        public int Option2 { get; set; } = 5; 

    }
}