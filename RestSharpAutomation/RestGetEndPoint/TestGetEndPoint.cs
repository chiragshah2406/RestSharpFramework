﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServiceAutomation.Model.JsonModel;
using WebServiceAutomation.Model.XmlModel;

namespace RestSharpAutomation.RestGetEndPoint
{
    [TestClass]
    public class TestGetEndPoint
    {
        private string getUrl = "http://localhost:8080/laptop-bag/webapi/api/all";

        [TestMethod]
        public void TestGetUsingRestSharp()
        {
            IRestClient restClient = new RestClient();
            IRestRequest restRequest = new RestRequest(getUrl);
            IRestResponse restResponse =  restClient.Get(restRequest);
            /*Console.WriteLine(restResponse.IsSuccessful);
            Console.WriteLine(restResponse.StatusCode);
            Console.WriteLine(restResponse.ErrorMessage);
            Console.WriteLine(restResponse.ErrorException);*/

            if (restResponse.IsSuccessful)
            {
                Console.WriteLine("Status Code " + restResponse.StatusCode);
                Console.WriteLine("Response Content " + restResponse.Content);
            }

        }

        [TestMethod]
        public void TestGetInXmlFormat()
        {
            IRestClient restClient = new RestClient();
            IRestRequest restRequest = new RestRequest(getUrl);
            restRequest.AddHeader("Accept", "application/xml");
            IRestResponse restResponse = restClient.Get(restRequest);

            if (restResponse.IsSuccessful)
            {
                Console.WriteLine("Status Code " + restResponse.StatusCode);
                Console.WriteLine("Response Content " + restResponse.Content);
            }

        }

        [TestMethod]
        public void TestGetInJsonFormat()
        {
            IRestClient restClient = new RestClient();
            IRestRequest restRequest = new RestRequest(getUrl);
            restRequest.AddHeader("Accept", "application/json");
            IRestResponse restResponse = restClient.Get(restRequest);

            if (restResponse.IsSuccessful)
            {
                Console.WriteLine("Status Code " + restResponse.StatusCode);
                Console.WriteLine("Response Content " + restResponse.Content);
            }

        }

       [TestMethod]
       public void TestGetWithJson_Deserialize()
       {
            IRestClient restClient = new RestClient();
            IRestRequest restRequest = new RestRequest(getUrl);
            restRequest.AddHeader("Accept", "application/json");
            IRestResponse<List<JsonRootObject>> restResponse =  restClient.Get<List<JsonRootObject>>(restRequest);

            if (restResponse.IsSuccessful)
            {
                Console.WriteLine("Status Code " + restResponse.StatusCode);
                Assert.AreEqual(200, (int)restResponse.StatusCode);
                Console.WriteLine("Size of List " + restResponse.Data.Count);
                List<JsonRootObject> data = restResponse.Data;

                JsonRootObject jsonRootObject =  data.Find((x) =>
                {
                    return x.Id == 1;
                });

                Assert.AreEqual("Alienware M17", jsonRootObject.LaptopName);
                Assert.IsTrue(jsonRootObject.Features.Feature.Contains("8th Generation Intel® Core™ i5-8300H"), "Element is not Present");

            }
            else
            {
                Console.WriteLine("Error Msg " + restResponse.ErrorMessage);
                Console.WriteLine("Stack Trace " + restResponse.ErrorException);
            }

        }

        [TestMethod]
        public void TestGetWithXml_Deserialize()
        {
            IRestClient restClient = new RestClient();
            IRestRequest restRequest = new RestRequest(getUrl);
            restRequest.AddHeader("Accept", "application/xml");

           var dotNetXmlDeserializer = new RestSharp.Deserializers.DotNetXmlDeserializer();

            //IRestResponse<LaptopDetailss> restResponse = restClient.Get<LaptopDetailss>(restRequest);
            IRestResponse restResponse = restClient.Get(restRequest);

            if (restResponse.IsSuccessful)
            {
                Console.WriteLine("Status Code " + restResponse.StatusCode);
                Assert.AreEqual(200, (int)restResponse.StatusCode);
                
                LaptopDetailss data = dotNetXmlDeserializer.Deserialize<LaptopDetailss>(restResponse);
                Console.WriteLine("Size of List " + data.Laptop.Count);

                Laptop laptop = data.Laptop.Find((x) =>
                {
                    return x.Id.Equals("1", StringComparison.OrdinalIgnoreCase);
                });

                Assert.AreEqual("Alienware M17", laptop.LaptopName);
                Assert.IsTrue(laptop.Features.Feature.Contains("8th Generation Intel® Core™ i5-8300H"), "Element is not Present");
            }
            else
            {
                Console.WriteLine("Error Msg " + restResponse.ErrorMessage);
                Console.WriteLine("Stack Trace " + restResponse.ErrorException);
            }
        }

        [TestMethod]
        public void TestGetWithExecute()
        {
            IRestClient restClient = new RestClient();
            IRestRequest restRequest = new RestRequest()
            {
                Method = Method.GET,
                Resource = getUrl
            };

            restRequest.AddHeader("Accept", "application/json");

            IRestResponse<List<Laptop>> restResponse =  restClient.Execute<List<Laptop>>(restRequest);

            Assert.AreEqual(200, (int)restResponse.StatusCode);
            Assert.IsNotNull(restResponse.Data, "Response is null");
        }
        
    }
}