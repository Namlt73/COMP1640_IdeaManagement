using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using COMP1640_IdeaManagement.Data;
using COMP1640_IdeaManagement.Models;
using COMP1640_IdeaManagement.Helpper;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using System.Net;
using System.Net.Mail;
using MailKit.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace COMP1640_IdeaManagement.Controllers
{
    [Authorize]
    public class IdeasController : Controller
    {
        
        private readonly ApplicationDbContext _context;
        private IHostingEnvironment _oIHostingEnvironment;
        private readonly UserManager<IdentityUser> _userManager;

        public IdeasController(ApplicationDbContext context, IHostingEnvironment oIHostingEnvironment, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _oIHostingEnvironment = oIHostingEnvironment;
            _userManager = userManager;
        }

        private async Task<IdentityUser> GetCurrentUser()
        {
            return await _userManager.GetUserAsync(HttpContext.User);
        }

        public IActionResult AllFiles()
        {
            //Fetch all files in the Folder (Directory).
            string[] filePaths = Directory.GetFiles(Path.Combine(this._oIHostingEnvironment.WebRootPath, "Uploads/Ideas/"));

            //Copy File names to Model collection.
            List<FileModel> files = new List<FileModel>();
            foreach (string filePath in filePaths)
            {
                files.Add(new FileModel { FileName = Path.GetFileName(filePath) });
            }

            return View(files);
        }
        [Authorize(Roles = "QA Coordinator")]
        public FileResult DownloadFile(string fileName)
        {
            //Build the File Path.
            string path = Path.Combine(this._oIHostingEnvironment.WebRootPath, "Uploads/Ideas/") + fileName;

            //Read the File data into Byte Array.
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            //Send the File to Download.
            return File(bytes, "application/octet-stream", fileName);
        }

        [Authorize(Roles = "QA Coordinator")]
        public FileResult Download()
        {
            var webRoot = _oIHostingEnvironment.WebRootPath;
            var fileName = "MyZip.zip";
            var tempOutput = webRoot + "/Uploads/Ideas/" + fileName;

            using (ZipOutputStream oZipOutputStream = new ZipOutputStream(System.IO.File.Create(tempOutput)))
            {
                oZipOutputStream.SetLevel(9);
                byte[] buffer = new byte[4096];

                var fileList = new List<string>();

                fileList.Add(webRoot + "/Uploads/Ideas/img1.jpg");

                for (int i = 0; i < fileList.Count; i++)
                {
                    ZipEntry entry = new ZipEntry(Path.GetFileName(fileList[i]));
                    entry.DateTime = DateTime.Now;
                    entry.IsUnicodeText = true;
                    oZipOutputStream.PutNextEntry(entry);

                    using (FileStream oFileStream = System.IO.File.OpenRead(fileList[i]))
                    {
                        int sourceByte;

                        do
                        {

                            sourceByte = oFileStream.Read(buffer, 0, buffer.Length);
                            oZipOutputStream.Write(buffer, 0, sourceByte);
                        } while (sourceByte > 0);
                    }
                }

                oZipOutputStream.Finish();
                oZipOutputStream.Flush();
                oZipOutputStream.Close();
            }

            byte[] finalResult = System.IO.File.ReadAllBytes(tempOutput);

            if (System.IO.File.Exists(tempOutput))
            {
                System.IO.File.Delete(tempOutput);
            }

            if (finalResult == null || !finalResult.Any())
            {
                throw new Exception(String.Format("Nothing Found"));

            }

            return File(finalResult, "application/zip", fileName);

        }
        // GET: Ideas
        [Authorize(Roles = "QA Coordinator")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Ideas.Include(i => i.Category).Include(i => i.Mission).Include(i => i.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Ideas/Details/5
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> Details(int? id)
        {
            
            if (id == null)
            {
                return NotFound();
            }

            var idea = await _context.Ideas
                .Include(i => i.Category)
                .Include(i => i.Mission)
                .Include(i => i.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (idea == null)
            {
                return NotFound();
            }

            return View(idea);
        }

        // GET: Ideas/Create
        [Authorize(Roles = "Staff")]
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            ViewData["MissionId"] = new SelectList(_context.Missions, "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Ideas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> Create([Bind("Id,Title,Content,CreatedAt,Status,UserId,MissionId,CategoryId")] Idea idea)
        {
            if (ModelState.IsValid)
            {
                _context.Add(idea);
                await _context.SaveChangesAsync();

            }
            // TODO tro toi gmail can gui (query trong db)
            string to = "anhroyal110@gmail.com";
            string subject = "A new idea has just been created";
            string body = "Just created a new idea, please go see it and leave a review";


            var email = new MimeMessage();

            email.Sender = new MailboxAddress("thuancisdd@gmail.com", "thuancisdd@gmail.com");
            email.From.Add(new MailboxAddress("thuancisdd@gmail.com", "thuancisdd@gmail.com"));
            email.To.Add(new MailboxAddress(to, to));
            email.Subject = subject;
            var builder = new BodyBuilder();
            builder.HtmlBody = body;
            email.Body = builder.ToMessageBody();
            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            //kết nối máy chủ
            await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            // xác thực
            // TODO CHỖ NÀY LÀ SEND MAIL VỚI MẬT KHẨU VÀ ACCOUT
            await smtp.AuthenticateAsync("thuancisdd@gmail.com", "ChanhThuanDuyen#2579#GmailCis12/2021");
            //gởi
            await smtp.SendAsync(email);

            smtp.Disconnect(true);

            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", idea.CategoryId);
            ViewData["MissionId"] = new SelectList(_context.Missions, "Id", "Name", idea.MissionId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", idea.UserId);
            return RedirectToAction(nameof(Index));
        }

        // GET: Ideas/Edit/5
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var idea = await _context.Ideas.FindAsync(id);
            if (idea == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", idea.CategoryId);
            ViewData["MissionId"] = new SelectList(_context.Missions, "Id", "Name", idea.MissionId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", idea.UserId);
            return View(idea);
        }

        // POST: Ideas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content,CreatedAt,Status,UserId,MissionId,CategoryId")] Idea idea)
        {
            if (id != idea.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(idea);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IdeaExists(idea.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", idea.CategoryId);
            ViewData["MissionId"] = new SelectList(_context.Missions, "Id", "Name", idea.MissionId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", idea.UserId);
            return View(idea);
        }

        // GET: Ideas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var idea = await _context.Ideas
                .Include(i => i.Category)
                .Include(i => i.Mission)
                .Include(i => i.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (idea == null)
            {
                return NotFound();
            }

            return View(idea);
        }

        // POST: Ideas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var idea = await _context.Ideas.FindAsync(id);
            _context.Ideas.Remove(idea);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IdeaExists(int id)
        {
            return _context.Ideas.Any(e => e.Id == id);
        }

        [HttpGet]
        public IActionResult FileUpload(int id)
        {
            var idea = _context.Ideas
                .Include(c => c.Images)
                .SingleOrDefault(c => c.Id == id);
            if (idea == null)
                return NotFound("The idea really doesn't exist! Please try again later");
            ViewData["Idea"] = idea;

            return View(new UploadFile());
        }
        [HttpPost, ActionName("FileUpload")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> FileUploadAsync(int id, [Bind("FileUpload")] UploadFile model)
        {
            var idea = _context.Ideas
                .Include(c => c.Images)
                .SingleOrDefault(c => c.Id == id);
            if (idea == null)
                return NotFound("The idea really doesn't exist! Please try again later");
            ViewData["Idea"] = idea;

            if(model != null)
            {
                var file = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + Path.GetExtension(model.FileUpload.FileName);

                var filePath = Path.Combine(@"wwwroot/Uploads", "Ideas", file); // path to save img

                using(var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.FileUpload.CopyToAsync(fileStream);
                }

                _context.Add(new IdeaImage()
                {
                    IdeaId = idea.Id,
                    FileName = file
                });

                await _context.SaveChangesAsync();
            }
            

            return View(new UploadFile());  
        }
    }
}
