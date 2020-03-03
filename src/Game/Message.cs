using System;
using System.Collections.Generic;
using System.Text;

namespace Game
{
    public class Message
    {
        public string Text { get; set; }

        public Status Status { get; set; }
    }

    public enum Status
    {
        None,
        Accept,
        Reject,
        GiveUp,
    }
}
