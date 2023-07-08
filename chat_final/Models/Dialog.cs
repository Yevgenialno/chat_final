using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace chat_final.Models
{
	public class Dialog
	{
		[Key]
		public int Id { get; set; }

		[ForeignKey("FirstUser")]
		public string FirstUserTag { get; set; }

		[ForeignKey("SecondUser")]
		public string SecondUserTag { get; set; }
		public IdentityUser FirstUser { get; set; }
		public IdentityUser SecondUser { get; set; }

		public Dialog() { }

		public Dialog(IdentityUser firstUser, IdentityUser secondUser)
		{
			FirstUser = firstUser;
			SecondUser = secondUser;
		}
	}
}
