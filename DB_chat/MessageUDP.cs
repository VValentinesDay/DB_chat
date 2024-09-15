using DB_chat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DB_chat
{
    public enum Command
    {
        Register,
        Message,
        Confirmation
    }
    public class MessageUDP
    {
        public Command Command { get; set; }
        public int? Id { get; set; }
        public string FromName { get; set; }
        public string ToName { get; set; }
        public string Text { get; set; }

        // сериализация в JSon

        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }

        // десериализация из джейсон

        public static MessageUDP FromJson(string json)
        {
            return JsonSerializer.Deserialize<MessageUDP>(json);
        }
    }
}
