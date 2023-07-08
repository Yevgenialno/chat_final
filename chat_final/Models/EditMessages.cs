namespace chat_final.Models
{
    public class EditMessages
    {
        public int Id { get; set; }
        public List<Message> Messages { get; set; }
        public EditMessages(int id, List<Message> messages)
        {
            Id = id;
            Messages = messages;
        }
    }
}
