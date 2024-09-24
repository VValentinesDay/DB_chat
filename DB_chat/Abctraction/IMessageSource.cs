using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DB_chat.Abctraction
{
    public interface IMessageSource
    {
        public void Send(MessageUDP message, IPEndPoint ep);
        public MessageUDP RecieveMessage(ref IPEndPoint ep);
    }
}
