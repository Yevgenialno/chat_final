using chat_final.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NuGet.Protocol.Plugins;
using System.Runtime.CompilerServices;

namespace chat_final.Controllers
{
    public class DialogController : Controller
    {
        private readonly ChatDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        public DialogController(ChatDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index(string receiver, string receiverId)
        {
            IdentityUser user = await _userManager.GetUserAsync(HttpContext.User);
            if (receiver is not null && receiverId is not null)
            {
                TempData["ReceiverEmail"] = receiver;
                TempData["ReceiverId"] = receiverId;
            }
            else
            {
                receiverId = TempData.Peek("ReceiverId").ToString();
                receiver = TempData.Peek("ReceiverEmail").ToString();
            }
            List<Models.Message> messages = _db.Messages.Where(m => (m.Sender == user && m.Receiver.Id == receiverId) || (m.Receiver == user && m.Sender.Id == receiverId)).ToList();
            return View("Index", messages);
        }

        public async Task<IActionResult> SendMessage(string newMessage)
        {
            IdentityUser user = await _userManager.GetUserAsync(HttpContext.User);
            string receiverId = TempData.Peek("ReceiverId").ToString();
            Models.Message m = new Models.Message(user, receiverId, newMessage ?? "");
            _db.Messages.Add(m);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteMessage(int id)
        {
            Models.Message messageToDelete = _db.Messages.Single(m => m.Id == id);
            IdentityUser user = await _userManager.GetUserAsync(HttpContext.User);
            if (messageToDelete.SenderTag == user.Id)
            {
                _db.Messages.Remove(messageToDelete);
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> CreateDialog(string newAddresseeTag)
        {
            IdentityUser newAddressee = _db.Users.SingleOrDefault(u => u.Email == newAddresseeTag);
            if(newAddressee is not null)
            {
                TempData["ReceiverEmail"] = newAddresseeTag;
                IdentityUser user = await _userManager.GetUserAsync(HttpContext.User);
                List<Models.Message> messages = _db.Messages.Where(m => (m.Sender == user && m.Receiver.Id ==   newAddresseeTag) || (m.Receiver == user && m.Sender.Id == newAddresseeTag)).ToList();
                if(!_db.StartedDialogs.Any(d => d.FirstUser == user && d.SecondUser == newAddressee))
                {
                    _db.StartedDialogs.Add(new Dialog(user, newAddressee));
                    _db.StartedDialogs.Add(new Dialog(newAddressee, user));
                    _db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            else
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                List<IdentityUser> startedDialogs = _db.StartedDialogs.Where(d => d.FirstUser == user).Select(d => d.SecondUser).ToList();
                return RedirectToAction("Index", "ChooseDialog");
                //return View("../ChooseDialog/Index", startedDialogs);
            }
        }

        public async Task<IActionResult> EditMessage(int id)
        {
            Models.Message messageToEdit = _db.Messages.Single(m => m.Id == id);
            IdentityUser user = await _userManager.GetUserAsync(HttpContext.User);
            string receiverId = TempData.Peek("ReceiverId").ToString();
            string receiver = TempData.Peek("ReceiverEmail").ToString();
            List<Models.Message> messages = _db.Messages.Where(m => (m.Sender == user && m.Receiver.Id == receiverId) || (m.Receiver == user && m.Sender.Id == receiverId)).ToList();
            if (messageToEdit.SenderTag == user.Id)
            {
                return View("EditMessage", new EditMessages(id, messages));
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<IActionResult> EditedMessage(string newContent, int id)
        {
            Models.Message messageToEdit = _db.Messages.Single(m => m.Id == id);
            IdentityUser user = await _userManager.GetUserAsync(HttpContext.User);
            if (messageToEdit.SenderTag == user.Id)
            {
                messageToEdit.Content = newContent;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
