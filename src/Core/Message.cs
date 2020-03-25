namespace Core
{
    public class Message
    {
        public int Match { get; set; }

        public string Player { get; set; }

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
