using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using COMP1640_IdeaManagement.Data;
using COMP1640_IdeaManagement.Models;
using Microsoft.AspNetCore.Authorization;

namespace COMP1640_IdeaManagement.Controllers
{
    [Authorize]
    public class DislikesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DislikesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Dislikes
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Dislikes.Include(d => d.Idea).Include(d => d.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Dislikes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dislike = await _context.Dislikes
                .Include(d => d.Idea)
                .Include(d => d.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dislike == null)
            {
                return NotFound();
            }

            return View(dislike);
        }

        // GET: Dislikes/Create
        public IActionResult Create()
        {
            ViewData["IdeaId"] = new SelectList(_context.Ideas, "Id", "Content");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Dislikes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,IdeaId,IsDisLike")] Dislike dislike)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dislike);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdeaId"] = new SelectList(_context.Ideas, "Id", "Content", dislike.IdeaId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", dislike.UserId);
            return View(dislike);
        }

        // GET: Dislikes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dislike = await _context.Dislikes.FindAsync(id);
            if (dislike == null)
            {
                return NotFound();
            }
            ViewData["IdeaId"] = new SelectList(_context.Ideas, "Id", "Content", dislike.IdeaId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", dislike.UserId);
            return View(dislike);
        }

        // POST: Dislikes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,IdeaId,IsDisLike")] Dislike dislike)
        {
            if (id != dislike.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dislike);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DislikeExists(dislike.Id))
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
            ViewData["IdeaId"] = new SelectList(_context.Ideas, "Id", "Content", dislike.IdeaId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", dislike.UserId);
            return View(dislike);
        }

        // GET: Dislikes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dislike = await _context.Dislikes
                .Include(d => d.Idea)
                .Include(d => d.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dislike == null)
            {
                return NotFound();
            }

            return View(dislike);
        }

        // POST: Dislikes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dislike = await _context.Dislikes.FindAsync(id);
            _context.Dislikes.Remove(dislike);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DislikeExists(int id)
        {
            return _context.Dislikes.Any(e => e.Id == id);
        }
    }
}
