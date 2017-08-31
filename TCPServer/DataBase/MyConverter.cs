using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCP_Chat_Library;

namespace TCPServer_DataBase
{
    public static class MyConverter
    {
        public static User UserObjToUser(UserObj obj)
        {
            User user = new User();
            user.Age = obj.Age;
            user.Login = obj.Login;
            user.Name = obj.Name;
            user.Password = obj.Password;
            user.Sex = obj.Sex;
            return user;
        }

        public static UserObj UserToUserObj(User obj)
        {
            UserObj user = new UserObj();
            user.Age = (int)obj.Age;
            user.Login = obj.Login;
            user.Name = obj.Name;
            user.Password = obj.Password;
            user.Sex = obj.Sex;
            return user;
        }
    }
}
