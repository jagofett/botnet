using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using KerioBot.ApiModels;
using Newtonsoft.Json;

namespace KerioBot.Models
{
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Builder.FormFlow;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;

    [Serializable]
    public class KerioForm
    {
        internal static string[] TimeFormats = new string[] { "HHmm", "HH:mm" };
        internal static string[] DateFormats = new string[] { "yyyy.MM.dd", "yyyy.MM.dd.", "yyyy/MM/dd", "yyyyMMdd", "yyyy-MM-dd" };
        private DateTime meetingTime { get; set; }
        private List<string> emails { get; set; }

        [Prompt("Mikorra foglaljak?")]
        [Describe("Dátum")]
        [Template(TemplateUsage.NotUnderstood, "Nem értem mi az a \"{0}\".", "")]
        [Template(TemplateUsage.DateTime, "dátum template")]
        [Template(TemplateUsage.DateTimeHelp, "date help")]
        [Terms("dátum")]
        public string Date
        {
            get
            {
                return $"{this} ({meetingTime.ToString("yyyy.MM.dd")})";
            }
            set
            {

            }
        }

        [Prompt("Mikorra szeretnél foglalni?")]
        [Terms("óra", "idő")]
        public string Time;

        [Prompt("Kik vesznek részt?")]
        public string Participants;

        [Prompt("Hova foglaljak?")]
        public string Location;


        private static  MeetingRoomViewModel meeting;
        //private static List<MeetingRoomViewModel> avMeetings = new List<MeetingRoomViewModel>();

        private static readonly string KerioAPIUri = "http://mrdapi.azurewebsites.net/";

        private static IEnumerable<MeetingRoomViewModel> meetingRooms = new List<MeetingRoomViewModel>();
        private static void GetMeetingRooms()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(KerioAPIUri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // New code:
                var response = client.GetAsync("api/meetingRooms").Result;
                if (response.IsSuccessStatusCode)
                {
                    var str = response.Content.ReadAsStringAsync().Result;
                    meetingRooms = JsonConvert.DeserializeObject<IEnumerable<MeetingRoomViewModel>>(str);
                     
                }
            }
        }

        private static  string ReserveMeeting(DateTime meetingTime)
        {
            var model = new MeetingViewModel
            {
                Name = "Teszt meeting",
                StartDate = meetingTime.ToString(),
                EndDate =  meetingTime.AddMinutes(30).ToString(),
                Organizer = new AttendeeViewModel
                {
                    Name = "Magasvári Ákos",
                    Email = "a.magasvari@mortoff.hu"
                },
                Summary = "Teszt esemeny"
            };
            var res = string.Empty;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(KerioAPIUri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // New code:
                var postStr = JsonConvert.SerializeObject(model);
                var response = client.PostAsJsonAsync("api/meetings?roomAddress="+meeting.Id, model).Result;
                if (response.IsSuccessStatusCode)
                {
                    res = response.Content.ReadAsStringAsync().Result;
                    //res = JsonConvert.DeserializeObject<IEnumerable<MeetingRoomViewModel>>(str);

                }
            }
            return res;
        }

        public static IForm<KerioForm> BuildForm()
        {
            OnCompletionAsyncDelegate<KerioForm> processForm = async (context, state) =>
            {
                var xy = context;
                var resp = ReserveMeeting(state.meetingTime);
                await context.PostAsync("Processing your request response: " + resp);
            };

            FormBuilder<KerioForm> form = new FormBuilder<KerioForm>();

            return form.Message("Szia!")
                .Field(nameof(KerioForm.Date), validate:
                async (_form, val) =>
                {
                    ValidateResult result = new ValidateResult();
                    var value = val as string;

                    _form.meetingTime = DateTime.Today;

                    if (value.Contains("ma"))
                    {
                        result.IsValid = true;
                        return result;
                    }
                    else if (value.Contains("holnap"))
                    {
                        _form.meetingTime = DateTime.Today.AddDays(1);
                        result.IsValid = true;
                        return result;
                    }
                    else if (value.Contains("hétfő"))
                    {
                        int days = ((int)DayOfWeek.Monday - (int)DateTime.Today.DayOfWeek +7) % 7;
                        _form.meetingTime = _form.meetingTime.AddDays(days);
                        result.IsValid = true;
                        return result;
                    }
                    else if (value.Contains("kedd"))
                    {
                        int days = ((int)DayOfWeek.Tuesday - (int)DateTime.Today.DayOfWeek +7) % 7;
                        _form.meetingTime = _form.meetingTime.AddDays(days);
                        result.IsValid = true;
                        return result;
                    }
                    else if (value.Contains("szerda"))
                    {
                        int days = ((int)DayOfWeek.Wednesday - (int)DateTime.Today.DayOfWeek +7) % 7;
                        _form.meetingTime = _form.meetingTime.AddDays(days);
                        result.IsValid = true;
                        return result;
                    }
                    else if (value.Contains("csütörtök"))
                    {
                        int days = ((int)DayOfWeek.Thursday - (int)DateTime.Today.DayOfWeek +7) % 7;
                        _form.meetingTime = _form.meetingTime.AddDays(days);
                        result.IsValid = true;
                        return result;
                    }
                    else if (value.Contains("péntek"))
                    {
                        int days = ((int)DayOfWeek.Friday - (int)DateTime.Today.DayOfWeek +7) % 7;
                        _form.meetingTime = _form.meetingTime.AddDays(days);
                        result.IsValid = true;
                        return result;
                    }
                    else if (value.Contains("szombat"))
                    {
                        int days = ((int)DayOfWeek.Saturday - (int)DateTime.Today.DayOfWeek +7) % 7;
                        _form.meetingTime = _form.meetingTime.AddDays(days);
                        result.IsValid = true;
                        return result;
                    }
                    else if (value.Contains("vasárnap"))
                    {
                        int days = ((int)DayOfWeek.Sunday - (int)DateTime.Today.DayOfWeek +7) % 7;
                        _form.meetingTime = _form.meetingTime.AddDays(days);
                        result.IsValid = true;
                        return result;
                    }
                    else
                    {
                        DateTime date;
                        if (DateTime.TryParseExact(value, DateFormats, new System.Globalization.CultureInfo("hu-HU"), System.Globalization.DateTimeStyles.None, out date))
                        {
                            _form.meetingTime = date.Date;
                            result.IsValid = true;
                            return result;
                        }
                    }

                    result.IsValid = false;
                    result.Feedback = "Ezt az értéket nem ismerem :(";

                    return result;
                })
                .Field(nameof(KerioForm.Time), validate:
                async (_form, val) =>
                {
                    ValidateResult result = new ValidateResult();
                    var value = val as string;
                    if(value.Contains("dél"))
                    {
                        _form.meetingTime = _form.meetingTime.Date.AddHours(12);
                        result.IsValid = true;
                        return result;
                    }
                    else
                    {
                        DateTime time;
                        if(DateTime.TryParseExact(value, TimeFormats, new System.Globalization.CultureInfo("hu-HU"), System.Globalization.DateTimeStyles.None, out time))
                        {
                            _form.meetingTime = _form.meetingTime.Date.AddHours(time.Hour).AddMinutes(time.Minute);
                            result.IsValid = true;
                            return result;
                        }
                    }

                    result.Feedback = "Nem értem :(";
                    result.IsValid = false;

                    return result;
                })
                .Field(nameof(KerioForm.Participants))
                .Field(nameof(KerioForm.Location), validate: async(state, value) =>
                {
                    var res = new ValidateResult();
                    var meeting = meetingRooms.FirstOrDefault(x => x.Name.ToLower().Contains(((string) value).ToLower()));
                    if (meeting == null)
                    {
                        res.Feedback = "Nincs ilyen tárgyaló! :=(";
                        res.IsValid = false;
                        return res;
                    }

                    KerioForm.meeting = meeting;

                    res.Feedback = meeting.Name + " kiválasztva.";
                    res.IsValid = true;
                    return res;
                },prompt: GenerateAvLoc())
                .AddRemainingFields()
                .OnCompletionAsync(processForm)
                .Build();
        }

        private static PromptAttribute GenerateAvLoc()
        {
            GetMeetingRooms();

            var meetingR = string.Join(", ",meetingRooms.Select(x => x.Name));
            var ret = new PromptAttribute("Elérhető tárgyalók: " + meetingR);

            return ret;
        }
    }
}