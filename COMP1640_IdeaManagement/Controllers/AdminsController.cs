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
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminsController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            this._roleManager = roleManager;
        }

        [Authorize(Policy = "readpolicy")]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult RoleList()
        {
            var roles = _roleManager.Roles.ToList();
            return View(roles);
        }

        public IActionResult UserList()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }

        public async Task<IActionResult> AsignRole(string id)
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

                var user = await _userManager.FindByIdAsync(id);
                var admin = await _userManager.FindByNameAsync("Administrator");
                if (admin == null)
                {
                   
                    await _userManager.AddToRoleAsync(user, Utils.Utils.Administrator);
                    
                }
                else
                {
                    return Content("This account is already an Admin role");
                }



                var QA = await _userManager.FindByNameAsync("QA Coordinato");
                if (QA == null)
                {

                    await _userManager.AddToRoleAsync(user, Utils.Utils.QACoordinator );

                }
                else
                {
                    return Content("This account is already an QA Coordinato role");
                }

                var staff = await _userManager.FindByNameAsync("Staff");
                if (staff == null)
                {

                    await _userManager.AddToRoleAsync(user, Utils.Utils.Staff);

                }
                else
                {
                    return Content("This account is already an Staff role");
                }



            }

            return RedirectToAction("UserList");
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
