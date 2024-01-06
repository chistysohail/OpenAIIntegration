using HigLabo.OpenAI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OpenAIIntegration.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OpenAIIntegration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        public readonly IConfiguration configuration;
        string apiKey;
        public string assistanId;

        public HomeController(IConfiguration _configuration)
        {
            configuration = _configuration;
            apiKey = _configuration.GetValue<string>("apiKey");
            assistanId = _configuration.GetValue<string>("assistantId");
        }

        // GET: api/<Home>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<Home>/5
        [HttpGet("{value}")]
        public async Task<string> Get(int value)
        {
            return null;
        }
        // Without Assistant Id

        // POST api/<Home>
        //[HttpPost]
        //public async Task<string> Post([FromBody] OpenAIModel value)
        //{
        //    string apiKey = "apiKey";
        //    string answer = string.Empty;
        //    var client = new OpenAIClient(apiKey);
        //    try
        //    {
        //        if (value != null)
        //        {
        //            var p = new ChatCompletionsParameter();
        //            p.Messages.Add(new ChatMessage(ChatMessageRole.User, value.question));

        //            p.Model = "gpt-3.5-turbo";
        //            var res = await client.ChatCompletionsAsync(p);
        //            foreach (var choice in res.Choices)
        //            {
        //                answer = choice.Message.Content;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        answer = "Got an Exception!! " + ex.Message;
        //    }
        //    return answer;
        //}

        // ---------------------------With Assistant Id

        [HttpPost]
        public async Task<List<string>> Post([FromBody] OpenAIModel value)
        {
            List<string> answer = new List<string>();

            var cl = new OpenAIClient(apiKey);
            try
            {
                var now = DateTimeOffset.Now;
                var threadId = "";
                if (threadId.Length == 0)
                {
                    var res = await cl.ThreadCreateAsync();
                    threadId = res.Id;
                }
                {
                    var p = new MessageCreateParameter();
                    p.Thread_Id = threadId;
                    p.Role = "user";
                    p.Content = value.question;
                    var res = await cl.MessageCreateAsync(p);
                }
                var runId = "";
                {
                    var p = new RunCreateParameter();
                    p.Assistant_Id = assistanId;
                    p.Thread_Id = threadId;
                    var res = await cl.RunCreateAsync(p);
                    runId = res.Id;
                }
                var loopCount = 0;
                Thread.Sleep(3000);
                var interval = 1000;
                while (true)
                {
                    Thread.Sleep(interval);
                    var p = new RunRetrieveParameter();
                    p.Thread_Id = threadId;
                    p.Run_Id = runId;
                    var res = await cl.RunRetrieveAsync(p);
                    if (res.Status != "queued" &&
                        res.Status != "in_progress" &&
                        res.Status != "cancelling")
                    {
                        var p1 = new MessagesParameter();
                        p1.Thread_Id = threadId;
                        p1.QueryParameter.Order = "desc";
                        var res1 = await cl.MessagesAsync(p1);
                        foreach (var item in res1.Data)
                        {
                            foreach (var content in item.Content)
                            {
                                if (content.Text == null) { continue; }
                                answer.Add(content.Text.Value);
                            }
                        }
                        break;
                    }
                    loopCount++;
                    if (loopCount > 120) { break; }
                }
            }
            catch (Exception ex)
            {
                answer.Add("Got an Exception!! " + ex.Message);
            }
            return answer;
        }

        // PUT api/<Home>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<Home>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
