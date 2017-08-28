using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCP_Chat_Library
{
    [Serializable]
    public static class Commands
    {
        

        public enum MyCommands
        {
            Message = 1,
            UserData = 2,
            Error = 3,
            Exit = 10
        }
    }

    [Serializable]
    public class Command
    {
        public Commands.MyCommands command { get; set; }
        public int UsersNum { get; set; }
    }
}
