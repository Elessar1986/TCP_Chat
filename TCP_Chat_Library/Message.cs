using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCP_Chat_Library
{
    class Message
    {
            public long MessageID { get; set; }

            public long FromID { get; set; }

            public long ToId { get; set; }

            public DateTime? Time { get; set; }

            public string message { get; set; }
   
    }
}
