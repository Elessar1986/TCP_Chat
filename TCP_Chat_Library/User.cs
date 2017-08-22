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
        string Name { get; set; }
        int Age { get; set; }
        string Sex { get; set; }

        string Login { get; set; }
        string Password { get; set; }

    }
}
