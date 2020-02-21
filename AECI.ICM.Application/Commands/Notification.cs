using System;
using System.Collections.Generic;
using System.Text;

namespace AECI.ICM.Application.Commands
{
    public static class Notification
    {
        public static class V1
        {
            public class CreateEmail
            {
                public string Message { get; set; }
                public string From { get; set; }
                public string To { get; set; }
                public string Subject { get; set; }
                public string Branch { get; set; }
                public string Attachments { get; set; }
            }
        }
    }
}
