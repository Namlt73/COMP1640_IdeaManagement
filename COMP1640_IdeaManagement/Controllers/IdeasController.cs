﻿using System;
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

namespace COMP1640_IdeaManagement.Controllers
{
    public class IdeasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IdeasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Ideas
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Ideas.Include(i => i.Category).Include(i => i.Mission).Include(i => i.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Ideas/Details/5
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
        public async Task<IActionResult> Create([Bind("Id,Title,Content,CreatedAt,Status,UserId,MissionId,CategoryId")] Idea idea)
        {
            if (ModelState.IsValid)
            {
                _context.Add(idea);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", idea.CategoryId);
            ViewData["MissionId"] = new SelectList(_context.Missions, "Id", "Name", idea.MissionId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", idea.UserId);
            return View(idea);
        }

        // GET: Ideas/Edit/5
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

                var filePath = Path.Combine("Uploads", "Ideas", file); // path to save img

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