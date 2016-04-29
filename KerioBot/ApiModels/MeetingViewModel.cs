
using System.Collections.Generic;

namespace KerioBot.ApiModels
{
    /// <summary>
    /// The view model of meeting.
    /// </summary>
    public class MeetingViewModel
    {
        /// <summary>
        /// The Id of meeting.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// The Name of meeting.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The summary of meeting.
        /// </summary>
        public string Summary { get; set; }
        /// <summary>
        /// Is the meeting reserved (busy)
        /// </summary>
        public bool IsReserved { get; set; }
        /// <summary>
        /// The organizer of meeting.
        /// </summary>
        public AttendeeViewModel Organizer { get; set; }
        /// <summary>
        /// The attendees of meeting.
        /// </summary>
        public IList<AttendeeViewModel> Attendees { get; set; }
        /// <summary>
        /// The start of meeting in ISO 8601 format: 2015-03-08T08:00:00
        /// </summary>
        public string StartDate { get; set; }
        /// <summary>
        /// The start of meeting in ISO 8601 format: 2015-03-08T09:00:00
        /// </summary>
        public string EndDate { get; set; }
        /// <summary>
        /// The duration of meeting
        /// </summary>
        public string Duration { get; set; }
    }
}