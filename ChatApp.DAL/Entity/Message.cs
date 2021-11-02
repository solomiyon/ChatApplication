﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ChatApp.DAL.Entity
{
    public class Message
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public User User { get; set; }
        public string UserId { get; set; }
        public Chat Chat { get; set; }
        public int ChatId { get; set; }
    }
}
