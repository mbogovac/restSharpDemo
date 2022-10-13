
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RestSharp;
using RestSharpDemo.Model;
using System;
using System.Threading.Tasks;

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

            request.AddBody(new Posts() { id = "17", author = "Tom", title = "RestSharp Course"});

            var response = client.Execute<Posts>(request);

            //JObject obs = JObject.Parse(response.Content);

            Assert.That(response.Data.author, Is.EqualTo("Tom"), "Name is not correct");
        }

        [Test]
        public async void PostWithAsync()
        {
            var client = new RestClient("http://localhost:3000/");

            var request = new RestRequest("posts", Method.Post);

            request.RequestFormat = DataFormat.Json;

            request.AddBody(new Posts() { id = "17", author = "Tom", title = "RestSharp Course" });

            var response = client.Execute<Posts>(request);

            

            //JObject obs = JObject.Parse(response.Content);

            Assert.That(response.Data.author, Is.EqualTo("Tom"), "Name is not correct");
        }

        private async Task<RestResponse<T>> ExecuteAsyncRequest<T>(RestClient client, RestRequest request) where T:class, new()
        {
            //var response = await client.ExecuteAsync<T>(request);

            var taskCompletionSource = new TaskCompletionSource<RestResponse<T>>();
            
            client.ExecuteAsync<T>(request, restResponse =>
            {
                if(restResponse.ErrorException != null)
                {
                    const string message = "Error retrieving response";
                    throw new ApplicationException(message, restResponse.ErrorException);
                }

                taskCompletionSource.SetResult(restResponse);
            });
            return await taskCompletionSource.Task;
        }

    }
}
