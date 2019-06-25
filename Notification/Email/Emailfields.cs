using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fido_Main.Notification.Email
{
    public class Emailfields
    {
        public string To { get; set; }
        public string CC { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<string> GaugeAttatch  { get; set; }
        public string EmailAttach { get; set; }
    }
}


