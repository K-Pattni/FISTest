using FIS.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using RestSharp.Serialization.Json;
using System.Collections.Generic;

namespace FIS
{
    [TestClass]
    public class APITests
    {
        string baseUrl = "https://jsonplaceholder.typicode.com";
    

        [TestMethod]
        public void GetList()
        {
            //Create Client
            var client = new RestClient(baseUrl);

            //Create Request
            var request = new RestRequest("/posts", Method.GET);

           //Add Request header
            request.AddHeader("accept", "application/jason");

            //Execute Request
            var response = client.Execute(request);



            //Deserialize JSON response
            var ds = new JsonDeserializer();
            List<Posts_ResponseModel> output = ds.Deserialize<List<Posts_ResponseModel>>(response);

            //Assertions
            //Verify correct Status Code
            Assert.AreEqual((int)response.StatusCode, 200);

            //Verify the values of the First Object in the content
            Assert.AreEqual(output[0].Id, 1);
            Assert.AreEqual(output[0].UserId, 1);

            //Verify the values of the Last Object in the content
            Assert.AreEqual(output[99].Id, 100);
            Assert.AreEqual(output[99].UserId, 10);

            //Verify Object Count 
            Assert.AreEqual(output.Count, 100);

        }


        [TestMethod]
        public void GetSingleObject()
        {
            //Create Client
            var client = new RestClient(baseUrl);
            
            //Create Request for Single post
            var request = new RestRequest("/posts/{postid}", Method.GET);
            request.AddUrlSegment("postid", 3);

            //Execute Request
            var response = client.Execute(request);

            //Deserialize JSON response
            var ds = new JsonDeserializer();
            List<Posts_ResponseModel> output = ds.Deserialize<List<Posts_ResponseModel>>(response);

            //Assertions
            //Verify Status Code returned
            Assert.AreEqual((int)response.StatusCode, 200);

            //Verify Count of objects
            Assert.AreEqual(output.Count, 1);

            //Verify Content
            Assert.AreEqual(output[0].Id, 3);
            Assert.AreEqual(output[0].UserId, 1);

        }

        [TestMethod]
        public void CreatePost()
        {
            //Create Client
            var client = new RestClient(baseUrl);

            //Create Request with POST Method
            var request = new RestRequest("/posts", Method.POST);
            request.RequestFormat = DataFormat.Json;
            //request.AddHeader("Content-type", "application/json");

            //Serialize JSON Payload Body
            request.AddJsonBody(new Posts_PayloadModel() { Title = "Test Title", Body = "Test Body", UserId = 1 }  );

            //Execute Method
            var response = client.Execute(request);
            
            //Deserialize Response
            var ds = new JsonDeserializer();
            List<Posts_ResponseModel> output = ds.Deserialize<List<Posts_ResponseModel>>(response);
           
            //Assertions
            //Verify Status Code returned
            Assert.AreEqual((int)response.StatusCode, 201);

            //Verify new object returned
            Assert.AreEqual(output[0].Id, 101);
            Assert.AreEqual(output[0].Title, "Test Title");
            Assert.AreEqual(output[0].Body, "Test Body");
            Assert.AreEqual(output[0].UserId, 1);


        }
        [TestMethod]
        public void UpdatePost()
        {
            //Create Client
            var client = new RestClient(baseUrl);

            //Decorate URL for Put Method
            var request = new RestRequest("/posts/{postid}", Method.PUT);
            request.AddUrlSegment("postid", 1);
            request.RequestFormat = DataFormat.Json;
            //request.AddHeader("Content-type", "application/json");

            //Serialize JSON Payload Body
            request.AddJsonBody(new Posts_ResponseModel() {Id=1, Title = "Updated Title", Body = "Updated Body", UserId = 1 });
            var response = client.Execute(request);

            //Deserialise Response
            var ds = new JsonDeserializer();
            List<Posts_ResponseModel> output = ds.Deserialize<List<Posts_ResponseModel>>(response);

            //Assertions
            //Verify Status Code
            Assert.AreEqual((int)response.StatusCode, 200);

            //Verify Updated Object/Post
            Assert.AreEqual(output[0].Id, 1);
            Assert.AreEqual(output[0].Title, "Updated Title");
            Assert.AreEqual(output[0].Body, "Updated Body");
            Assert.AreEqual(output[0].UserId, 1);


        }
        [TestMethod]
        public void DeletePost()
        {
            //Create Client
            var client = new RestClient(baseUrl);

            //Decorate URL for Delete Method
            var request = new RestRequest("/posts/{postid}", Method.DELETE);
            request.AddUrlSegment("postid", 1);

            request.RequestFormat = DataFormat.Json;

            var response = client.Execute(request);
           
            //Assertions
            Assert.AreEqual((int)response.StatusCode, 200);
            Assert.AreEqual(response.ResponseStatus.ToString(), "Completed");
            Assert.AreEqual(response.Content.ToString(),"{}");
                                    


        }

        [TestMethod]
        public void NegativeTestOnGet()
        {
            //Create Client
            var client = new RestClient(baseUrl);

            //Create Request for Single post
            var request = new RestRequest("/posts/{postid}", Method.GET);
            request.AddUrlSegment("postid", "idontexist");

            //Execute Request
            var response = client.Execute(request);

            //Assertions
            //Verify Status Code returned
            Assert.AreEqual((int)response.StatusCode, 404);
            Assert.AreEqual(response.StatusCode.ToString(), "NotFound");

            //Verify Content
            Assert.AreEqual(response.Content.ToString(), "{}");

        }
    }
}
