using System.Collections.Generic;

namespace KerioBot.ApiModels
{
    public class EmployeeViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<string> EmailAddresses { get; set; }

        public string PhotoUrl { get; set; }
    }
}