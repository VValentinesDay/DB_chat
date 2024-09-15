using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_chat.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Message> ToMessages { get; set; }// коллекция отправленных сообщений
        public virtual ICollection<Message> FromMessages { get; set; }
    }
}
