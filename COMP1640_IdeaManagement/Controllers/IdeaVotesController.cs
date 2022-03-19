using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using COMP1640_IdeaManagement.Data;
using COMP1640_IdeaManagement.Models;

namespace COMP1640_IdeaManagement.Controllers
{
    public class IdeaVotesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IdeaVotesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: IdeaVotes
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Votes.Include(i => i.Idea).Include(i => i.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: IdeaVotes/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ideaVote = await _context.Votes
                .Include(i => i.Idea)
                .Include(i => i.User)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (ideaVote == null)
            {
                return NotFound();
            }

            return View(ideaVote);
        }

        // GET: IdeaVotes/Create
        public IActionResult Create()
        {
            ViewData["IdeaId"] = new SelectList(_context.Ideas, "Id", "Content");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: IdeaVotes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,IdeaId,IsLike,IsDislike")] IdeaVote ideaVote)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ideaVote);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdeaId"] = new SelectList(_context.Ideas, "Id", "Content", ideaVote.IdeaId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", ideaVote.UserId);
            return View(ideaVote);
        }

        // GET: IdeaVotes/Edit/5
        public async Task<IActionResult> Edit(string userId, int ideaId)
        {
            if (userId == null && ideaId == null)
            {
                return NotFound();
            }
            
            /*var ideaVote = await _context.Votes.SingleOrDefaultAsync(c=>c.UserId == userId);
            if (ideaVote == null)
            {
                return NotFound();
            }*/
            var result = _context.Votes.SingleOrDefault(c => c.IdeaId == ideaId);
            if (result == null)
                return NotFound();

            ViewData["IdeaId"] = new SelectList(_context.Ideas, "Id", "Content", result.IdeaId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", result.UserId);
            return View(result);
        }

        // POST: IdeaVotes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string userId, int ideaId, [Bind("UserId,IdeaId,IsLike,IsDislike")] IdeaVote ideaVote)
        {
            if (userId != ideaVote.UserId && ideaId != ideaVote.IdeaId)
            {
                return NotFound();
            }
            
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ideaVote);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IdeaVoteExists(ideaVote.UserId))
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
            ViewData["IdeaId"] = new SelectList(_context.Ideas, "Id", "Content", ideaVote.IdeaId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", ideaVote.UserId);
            return View(ideaVote);
        }

        // GET: IdeaVotes/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ideaVote = await _context.Votes
                .Include(i => i.Idea)
                .Include(i => i.User)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (ideaVote == null)
            {
                return NotFound();
            }

            return View(ideaVote);
        }

        // POST: IdeaVotes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var ideaVote = await _context.Votes.FindAsync(id);
            _context.Votes.Remove(ideaVote);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IdeaVoteExists(string id)
        {
            return _context.Votes.Any(e => e.UserId == id);
        }
    }
}
