using System;

namespace Fortune_Teller_Service.Common.Services
{
    public class FortuneServiceConfig
    {
        public string Scheme { get; set; } = "http";
        public string Address { get; set; }

        public string RandomFortunePath { get; set; }

        public string AllFortunesPath { get; set; }

        public string RandomFortuneURL()
        {
            return MakeUrl(RandomFortunePath);
        }
        public string AllFortunesURL()
        {
            return MakeUrl(AllFortunesPath);
        }

        private string MakeUrl(string path)
        {
            return Scheme + "://" + Address + "/" + path;
        }

    }
}


