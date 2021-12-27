using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestLab;
using LabClient;
using System;
using System.IO;
using System.Net;


namespace LabServ.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly Serializer serializer;
        private static string tempData;
        private static string tempData2;
        public ValuesController()
        {
            serializer = new Serializer();
        }
        [Route("Ping")]
        [HttpGet]
        public IActionResult Ping()
        {
            return Content("Ok");
            //return HttpStatusCodeResult(200);
        }

        [Route("GetInputData")]
        [HttpGet]
        public IActionResult Getinputdata()
        {
            Random rnd = new Random();
            int k = rnd.Next(1, 10);
            int countArr1 = rnd.Next(2, 6);
            int countArr2 = rnd.Next(2, 6);
            decimal[] sums = new decimal[countArr1];
            int[] muls = new int[countArr2];
            for (int i = 0; i < sums.Length; i++)
            {
                sums[i] = rnd.Next(1, 10);
            }
            for (int i = 0; i < muls.Length; i++)
            {
                muls[i] = rnd.Next(1, 10);
            }
            var input = new Input
            {
                K = k,
                Sums = sums,
                Muls = muls
            };
            var inputSerialized = serializer.SerializeJson(input);
            tempData = inputSerialized;
            return Content(inputSerialized);
        }

        [Route("WriteAnswer")]
        [HttpPost]

        public string WriteAnswer()
        {
            using var streamReader = new StreamReader(Request.Body);
            var answer = streamReader.ReadToEndAsync().Result;

            var serializer = new Serializer();
            var jsonstrDeserialized = serializer.DeserializeJson<Input>(tempData);

            var output = new Output
            {
                SumResult = Calculations.calc_Sum2(jsonstrDeserialized.Sums, jsonstrDeserialized.K),
                MulResult = Calculations.calc_Mul2(jsonstrDeserialized.Muls),
                SortedInputs = Calculations.calc_Inputs2(jsonstrDeserialized.Sums, jsonstrDeserialized.Muls)
            };
            var outputData = serializer.SerializeJson(output);
            if (answer == outputData)
                return "Correct";
            else 
                return "Incorrect";

        }

        [Route("PostInputData")]
        [HttpPost]

        public void PostInputData()
        {
            using var streamReader = new StreamReader(Request.Body);
            tempData2 = streamReader.ReadToEndAsync().Result;
        }

        [Route("GetAnswer")]
        [HttpGet]

        public IActionResult GetAnswer()
        {
            var serializer = new Serializer();
            var jsonstrDeserialized = serializer.DeserializeJson<Input>(tempData2);

            var output = new Output
            {
                SumResult = Calculations.calc_Sum2(jsonstrDeserialized.Sums, jsonstrDeserialized.K),
                MulResult = Calculations.calc_Mul2(jsonstrDeserialized.Muls),
                SortedInputs = Calculations.calc_Inputs2(jsonstrDeserialized.Sums, jsonstrDeserialized.Muls)
            };
            var outputData = serializer.SerializeJson(output);

            return Content(outputData);
        }

        [Route("Stop")]
        [HttpGet]

        public void Stop()
        {
            System.Environment.Exit(0);
        }
    }
}
