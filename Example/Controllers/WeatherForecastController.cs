using Example.Service;
using Microsoft.AspNetCore.Mvc;

namespace Example.Controllers
{
    [ApiController]
    [Route("api/WeatherForecast")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            Console.WriteLine("fdsfds");

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [Route("URL")]
        [HttpGet]
        public async Task<int> GetURL()
        {
            var client = new HttpClient();
            Task<string> getStringTask = client.GetStringAsync("https://docs.microsoft.com/dotnet");
            DoIndepentWork();
            string contents = await getStringTask;
            return contents.Length;
        }

        void DoIndepentWork()
        {
            Console.WriteLine("Working...");
        }

        [Route("URLDeadlock")]
        [HttpGet]
        public async Task<string> DownloadStringV1(String url)
        {
            
            var rs = SomeMethod(url).Result.ToString();
            DoIndepentWork();
            //var download = request.Content.ReadAsStringAsync().Result;
            return "fasd";
        }

        [Route("AvoidReturnVoid")]
        [HttpGet]
        public async Task GetAvoidReturnVoidAsync()
        {
            await Task.Delay(TimeSpan.FromSeconds(10));

            //AsyncVoidMethod();

            try
            {
                AsyncVoidMethod();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private async void AsyncVoidMethod()
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
            throw new InvalidOperationException();
        }


        //[Route("Simple")]
        //[HttpGet]
        //public async Task<int> SimpleAsync()
        //{
        //    Console.WriteLine("Main Method Started......");
        //    SomeMethod();
        //    Console.WriteLine("Main Method End");
        //    return 1;
        //}

        private async Task<string> SomeMethod(string url)
        {
            var client = new HttpClient();
            client.GetAsync(url).Wait();
            
            Console.WriteLine("Some Method Started......");
            //Thread.Sleep(TimeSpan.FromSeconds(10));
            await Task.Delay(TimeSpan.FromSeconds(10));
            Console.WriteLine("\n");
            Console.WriteLine("Some Method End");
            return "hello";
        }


        [Route("Cancel")]
        [HttpGet]
        public async Task<string> CancelationAsync()
        {
            Console.WriteLine("Press enter to continue");
            Console.ReadLine();
            await Cancelation.CancelANonCancellableTaskAsync();
            
            return $"Avoid async void";
        }


        private static async Task<string> Foo()
        {
            await Task.Delay(1);
            return "";
        }

        private async static Task<string> Bar()
        {
            return await Foo();
        }

        private async static Task<string> Ros()
        {
            return await Bar();
        }

        // GET api/test
        [Route("GetDeadlock")]
        [HttpGet]
        public IEnumerable<string> GetDeadlock()
        {
            Task.WaitAll(Enumerable.Range(0, 10).Select(x => Ros()).ToArray());

            return new string[] { "value1", "value2" }; // This will never execute
        }
    }
}