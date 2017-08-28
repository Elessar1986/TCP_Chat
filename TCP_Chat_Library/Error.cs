using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCP_Chat_Library
{
    [Serializable]
    public static class ErrorCodeEnum
    {
        
        public enum ErrorCode
        {
            WrongLoginOrPass = 1,
            ServerShotdown =2
        }

        

    }

    [Serializable]
    public class ErrorObj
    {
        public ErrorCodeEnum.ErrorCode errorCode { get; set; }
    }
}
