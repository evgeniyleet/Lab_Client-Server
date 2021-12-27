using System;
using System.Net.Http;
using System.Threading.Tasks;
using TestLab;
namespace LabClient
{
    public class Program
    {
        private static HttpClient httpClient = new HttpClient();
        private static Uri uri = new Uri("http://localhost:2425/values/");
        //private static string tempData;
        public static void Ping(string request)
        {
            var response = httpClient.GetAsync($"{uri}{request}").Result;
            var content = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(content);
        }
        public static void GetinputData(string request)
        {
            var response = httpClient.GetAsync($"{uri}{request}").Result;
            var content = response.Content.ReadAsStringAsync().Result;
            //tempData = content;
            Console.WriteLine(content);
        }
        public static void WriteAnswer(string request)
        {
            Console.WriteLine("enter initial input data for calculation...");
            var serializer = new Serializer();
            var tempData = Console.ReadLine();
            Console.WriteLine("calculating output data...");
            var jsonstrDeserialized = serializer.DeserializeJson<Input>(tempData);

            var output = new Output
            {
                SumResult = Calculations.calc_Sum2(jsonstrDeserialized.Sums, jsonstrDeserialized.K),
                MulResult = Calculations.calc_Mul2(jsonstrDeserialized.Muls),
                SortedInputs = Calculations.calc_Inputs2(jsonstrDeserialized.Sums, jsonstrDeserialized.Muls)
            };

            var outputData = serializer.SerializeJson(output);
            Console.WriteLine("result: " + outputData);
            var outputDataContent = new StringContent(outputData);
            var response = httpClient.PostAsync($"{uri}{request}", outputDataContent).Result;
            var content = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(content);
            //Console.WriteLine(outputSerialized);
            
        }
        public static void PostInputData(string request)
        {
            Console.WriteLine("enter initial input data...");
            var tempData = Console.ReadLine();
            var inputDataContent = new StringContent(tempData);
            var response = httpClient.PostAsync($"{uri}{request}", inputDataContent).Result;
            var content = response.Content.ReadAsStringAsync().Result;
        }
        public static void GetAnswer(string request)
        {
            var response = httpClient.GetAsync($"{uri}{request}").Result;
            var content = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(content);
        }
        public static void Stop(string request)
        {
            Console.WriteLine("server shutdown.");
            var response = httpClient.GetAsync($"{uri}{request}").Result;
            var content = response.Content.ReadAsStringAsync().Result;
        }

        static void Main()
        {
            Console.WriteLine("available requests: 1)ping 2)getinputdata 3)writeanswer 4)postinputdata 5)getanswer 6)stop");
            while (true)
            {
                var request = Console.ReadLine();
                if (request.ToLower() == "ping")
                {
                    Ping(request);
                }

                if (request.ToLower() == "getinputdata")
                {
                    GetinputData(request);
                }

                if (request.ToLower() == "writeanswer")
                {
                    WriteAnswer(request);
                }
                if (request.ToLower() == "postinputdata")
                {
                    PostInputData(request);
                }
                if (request.ToLower() == "getanswer")
                {
                    GetAnswer(request);
                }
                if (request.ToLower() == "stop")
                {
                    Stop(request);
                    //break;
                }
            }
        }
    }

}



//internal class Program
//{
//    static async Task Main(string[] args)
//    {
//        Console.WriteLine("press any key to continue...");
//        Console.ReadLine();

//        using (HttpClient client = new HttpClient())
//        {
//            var response = await client.GetAsync("http://localhost:2425/values");
//            response.EnsureSuccessStatusCode();
//            if (response.IsSuccessStatusCode)
//            {
//                string message = await response.Content.ReadAsStringAsync();
//                Console.WriteLine(message);
//            }
//            else
//            {
//                Console.WriteLine($"response error code: {response.StatusCode}");
//            }
//        }


//    }
//}