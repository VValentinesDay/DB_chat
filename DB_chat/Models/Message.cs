﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_chat.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool Received { get; set; }

        public int? ToUserId { get; set; }
        public int? FromUserId { get; set; }

        // эти поля используются для связи "один ко многим"
        public virtual User? ToUser { get; set; }
        public virtual User? FromUser { get; set; }
    }
}
