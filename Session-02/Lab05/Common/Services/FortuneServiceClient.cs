
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Fortune_Teller_Service.Common.Services
{
    public class FortuneServiceClient : IFortuneService
    {

        public FortuneServiceClient()
        {
        }

        public async Task<List<Fortune>> AllFortunesAsync()
        {
            return await Task.Run(() => new List<Fortune>() { new Fortune() { Id = 1, Text = "I need to be wired up!" } });
        }

        public async Task<Fortune> RandomFortuneAsync()
        {
            var all = await AllFortunesAsync();
            return all[0];
        }

    }
}
