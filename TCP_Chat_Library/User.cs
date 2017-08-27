using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCP_Chat_Library
{
    [Serializable]
    public class User
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Sex { get; set; }

        public string Login { get; set; }
        public string Password { get; set; }

    }
}
