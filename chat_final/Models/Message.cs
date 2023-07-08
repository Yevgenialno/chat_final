using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace chat_final.Models;

public class Message
{
    [Key]
    public int Id { get; set; }
    public DateTime SendTime{get; set;}
    [Required]
    public string? Content{get; set;}

    [ForeignKey("Sender")]
    public string SenderTag { get; set;}
    public IdentityUser Sender { get; set;}

	[ForeignKey("Receiver")]
	public string ReceiverTag { get; set; }
	public IdentityUser Receiver { get; set;}

    public Message(IdentityUser u1, string receiverId, string content)
    {
        Sender = u1;
        ReceiverTag = receiverId;
        Content = content;
        SendTime = DateTime.Now;
    }
    public Message()
    {

    }
}