using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAG.AI.Contracts.ServiceBus
{
    public class BusResponse
    {
        public virtual Error Error { get; set; }

        public virtual bool HasError()
        {
            return Error != null;
        }
    }
}



