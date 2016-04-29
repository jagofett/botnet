using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using KerioBot.ApiModels;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Utilities;
using Newtonsoft.Json;
using Microsoft.Bot.Builder.Dialogs;
using KerioBot.Models;
using Microsoft.Bot.Builder.FormFlow;

namespace KerioBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        private readonly string KerioAPIUri = "http://mrdtest.int.i-deal.hu/";

        private List<MeetingRoomViewModel> meetingRooms = new List<MeetingRoomViewModel>(); 

        private async Task GetMeetingRooms()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(KerioAPIUri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // New code:
                var response = await client.GetAsync("api/meetingRooms");
                if (response.IsSuccessStatusCode)
                {
                    meetingRooms =
                        await response.Content.ReadAsAsync<IEnumerable<MeetingRoomViewModel>>() as
                            List<MeetingRoomViewModel>;
                }
            }
        }

        internal static IDialog<KerioForm> MakeRoot()
        {
            return Chain.From(() => FormDialog.FromForm(KerioForm.BuildForm))
                .Do(async (context, meeting) =>
                {
                    try
                    {
                        var completed = await meeting;
                        await context.PostAsync("OK!");
                    }
                    catch (FormCanceledException<KerioForm> e)
                    {
                        string reply;
                        if(e.InnerException == null)
                        {
                            reply = $"Mér mész el? :'(";
                        }
                        else
                        {
                            reply = "Ezt elrontottam.. :/";
                        }

                        await context.PostAsync(reply);
                    }
                });
            //return FormDialog.FromForm(KerioForm.BuildForm);
        }
        public async Task<Message> Post([FromBody]Message message)
        {
            if(message.Type == "Message")
            {
                if(message.Text == "cucc")
                {
                    return message.CreateReplyMessage("CUCCCC!!");
                }

                if (message.Text.ToLower() == "hodor")
                {
                    return message.CreateReplyMessage("HODOOOR!!");
                }

                if (message.Text.ToLower() == "morton")
                {
                    return message.CreateReplyMessage("Üdv a MortON napon!");
                }


                //await GetMeetingRooms();

                return await Conversation.SendAsync(message, MakeRoot);
            }
            else
            {
                return HandleSystemMessage(message);
            }
        }

        private Message HandleSystemMessage(Message message)
        {
            if (message.Type == "Ping")
            {
                Message reply = message.CreateReplyMessage();
                reply.Type = "Ping";
                return reply;
            }
            else if (message.Type == "DeleteUserData")
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == "BotAddedToConversation")
            {
                return message.CreateReplyMessage("Hello Botty McBotFace!");
            }
            else if (message.Type == "BotRemovedFromConversation")
            {
            }
            else if (message.Type == "UserAddedToConversation")
            {
            }
            else if (message.Type == "UserRemovedFromConversation")
            {
            }
            else if (message.Type == "EndOfConversation")
            {
            }

            return null;
        }
    }
}