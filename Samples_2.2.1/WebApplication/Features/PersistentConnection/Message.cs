using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebApplication.Features.SamplePersistentConnection
{
    public class Message
    {
        public string Type { get; set; }
        public string ConnectionId { get; set; }
        public string Content { get; set; }
        public string GroupName { get; set; }
    }
}
