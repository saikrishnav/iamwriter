using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using WriteBot1.Thesaurus;
using WriteBot1.Processors;

namespace WriteBot1
{
    [BotAuthentication]
    //[RoutePrefix("v1/messages")]
    public class MessagesController : ApiController
    {
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));

            if (activity == null || activity.GetActivityType() != ActivityTypes.Message)
            {
                //add code to handle errors, or non-messaging activities
            }

            // var processor = new SentimentProcessor();
            ITextProcessor processor = new KeyPhraseProcessor();
            var result = await processor.ProcessText(activity.Text);

            // Create reply message
            var replyMessage = activity.CreateReply();
            replyMessage.Recipient = activity.From;
            replyMessage.Type = ActivityTypes.Message;


            var thesaurusClient = new ThesaurusClient();

            string text = activity.Text;
            var syncObject = new object();
            var tasks = new List<Task>();
            foreach (var word in result.Split(' '))
            {
                if (!string.IsNullOrEmpty(word))
                {
                    var synonym = await thesaurusClient.GetFirstSynonym(word);

                    if (!string.IsNullOrEmpty(synonym))
                    {
                        text = text.Replace(word, synonym.Split(' ')[0]);
                    }
                }
            }

            Task.WaitAll(tasks.ToArray());

            replyMessage.Text = text;

            await connector.Conversations.ReplyToActivityAsync(replyMessage);
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}