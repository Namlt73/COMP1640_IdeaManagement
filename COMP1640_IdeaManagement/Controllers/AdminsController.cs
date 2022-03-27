using COMP1640_IdeaManagement.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace COMP1640_IdeaManagement.Controllers
{
    public class AdminsController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public AdminsController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult chart()
        {
            return View(_context.Ideas.ToList());
        }

        public async Task<IActionResult> Seeder()
        {
            var roleNames = typeof(Utils.Utils).GetFields().ToList();
            foreach (var role in roleNames)
            {
                var roleName = (string)role.GetRawConstantValue();
                var roleInDb = await _roleManager.FindByNameAsync(roleName);
                if (roleInDb == null)
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }


            var adminUser = await _userManager.FindByNameAsync("admin");
            if (adminUser == null)
            {
                adminUser = new IdentityUser()
                {
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com",
                    EmailConfirmed = true,
                };

                await _userManager.CreateAsync(adminUser, "admin123");
                await _userManager.AddToRoleAsync(adminUser, Utils.Utils.Administrator);
            }

            return RedirectToAction("Index");
        }
    }
}
