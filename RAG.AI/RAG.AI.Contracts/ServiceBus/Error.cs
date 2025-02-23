using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAG.AI.Contracts.ServiceBus
{
    public class Error
    {
        public Error()
        {
        }

        public Error(Enum error, string message = null)
        {
            ErrorCode = Convert.ToInt32(error);
            Message = message;
        }

        public int ErrorCode { get; set; }
        public string Message { get; set; }

        public bool Is(Enum e)
        {
            return Convert.ToInt32(e) == ErrorCode;
        }
    }
}



