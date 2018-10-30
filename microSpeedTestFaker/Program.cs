using System;
using System.Threading.Tasks;

namespace microSpeedTestFaker
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        static async Task MainAsync()
        {
            SpeedTest sp = new SpeedTest();

            //if > 2.999 gbps --> no resultid

            var code = await sp.DoFakeTestAsync("9999", "2999999", "9999", "55", "3038");
        }
    }
}
