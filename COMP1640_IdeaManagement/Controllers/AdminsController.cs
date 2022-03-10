using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace COMP1640_IdeaManagement.Controllers
{
    public class AdminsController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AdminsController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            this.roleManager = roleManager;
        }

        [Authorize(Policy = "readpolicy")]
        public IActionResult Index()
        {
            var roles = roleManager.Roles.ToList();
            return View(roles);
        }

        [Authorize(Policy = "readpolicy")]
        public IActionResult Create()
        {
            return View(new IdentityRole());
        }

        [HttpPost]
        public async Task<IActionResult> Create(IdentityRole role)
        {
            await roleManager.CreateAsync(role);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Seeder()
        {
            var roleNames = typeof(Utils.Utils).GetFields().ToList();
            foreach (var role in roleNames)
            {
                var roleName = (string)role.GetRawConstantValue();
                var roleInDb = await roleManager.FindByNameAsync(roleName);
                if (roleInDb == null)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
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
