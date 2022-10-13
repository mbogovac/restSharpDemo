
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RestSharp;
using RestSharpDemo.Model;
using System;

namespace RestSharpDemo
{
    [TestFixture]
    public class UnitTest1
    {
        [Test]
        public void TestMethod1()
        {
            var client = new RestClient("http://localhost:3000/");

            var request = new RestRequest("posts/{postid}", Method.Get);
            request.AddUrlSegment("postid", 1);

            var response = client.Execute(request);

            JObject obs = JObject.Parse(response.Content);
            Assert.That(obs["author"].ToString(), Is.EqualTo("Karthik KK"), "Author is not correct");
        }

        [Test]
        public void PostWithAnonymousClass()
        {
            var client = new RestClient("http://localhost:3000/");

            var request = new RestRequest("posts/{postid}/profile", Method.Post);

            request.RequestFormat = DataFormat.Json;

            request.AddBody(new { name = "Sam" });
            request.AddUrlSegment("postid", 1);

            var response = client.Execute(request);

            JObject obs = JObject.Parse(response.Content);
            Assert.That(obs["name"].ToString(), Is.EqualTo("Sam"), "Name is not correct");
        }

        [Test]
        public void PostWithTypeClass()
        {
            var client = new RestClient("http://localhost:3000/");

            var request = new RestRequest("posts", Method.Post);

            request.RequestFormat = DataFormat.Json;

            request.AddBody(new Posts() { id = "13", author = "Del Boy", title = "RestSharp Course"});

            var response = client.Execute(request);

            JObject obs = JObject.Parse(response.Content);
            Assert.That(obs["author"].ToString(), Is.EqualTo("Del Boy"), "Name is not correct");
        }
    }
}
