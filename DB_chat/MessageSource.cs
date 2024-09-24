using DB_chat.Abctraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DB_chat
{
    public class MessageSource : IMessageSource
    {
        private readonly UdpClient _udpClient;

        public MessageSource(int port)
        {
            _udpClient = new UdpClient(port);
        }

        public MessageUDP RecieveMessage(ref IPEndPoint ep)
        {
            byte[] data = _udpClient.Receive(ref ep);
            string str = Encoding.UTF8.GetString(data);
            return MessageUDP.FromJson(str);
        }

        public void Send(MessageUDP message, IPEndPoint ep)
        {
            string str = message.ToJson();
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            _udpClient.Send(bytes, ep);
        }
 
    }
}
