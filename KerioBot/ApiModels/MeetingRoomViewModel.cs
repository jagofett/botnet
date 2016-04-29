using System;

namespace KerioBot.ApiModels
{
    [Serializable]
    public class MeetingRoomViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string EMail { get; set; }
        public string PhotoUrl { get; set; }

    }
}