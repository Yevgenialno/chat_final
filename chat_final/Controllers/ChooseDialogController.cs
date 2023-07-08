using chat_final.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace chat_final.Controllers
{
    public class ChooseDialogController : Controller
    {
        private readonly ChatDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        public ChooseDialogController(ChatDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            List<IdentityUser> startedDialogs = _db.StartedDialogs.Where(d => d.FirstUser == user).Select(d => d.SecondUser).ToList();
            return View(startedDialogs);
        }
    }
}
