using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
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