
namespace Sample.Model
{
    public class MessageModel
    {
        public string MsgId { get; set; }

        public string Title { get; set; }

        public string Sender { get; set; }

        public string Receiver { get; set; }

        public string RecvTime { get; set; }

        public string Body { get; set; }

        public bool IsRead { get; set; }
    }
}
