using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAG.AI.Contracts.ServiceBus
{
    public class BusEvent
    {
        public Guid EventId { get; set; } = Guid.NewGuid();
        public ServiceName Owner { get; set; }
        public DateTime EventTime { get; set; } = DateTime.UtcNow;
    }
}



