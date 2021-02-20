using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookLibrary.Data;
using BookLibrary.Models;
using BookLibrary.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BookLibrary.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IBookService _bookService;

        public BooksController(ApplicationDbContext context, IBookService bookService)
        {
            _context = context;
            _bookService = bookService;
        }

        // GET: Books
        public async Task<IActionResult> Index(string authorSearch)
        {
            if (!string.IsNullOrEmpty(authorSearch))
            {
                return View(await _bookService.ListForAuthor(authorSearch));
            }
            return View(await _bookService.List());
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            var book = await _bookService.GetById(id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,Title,Author,PublicationYear,Price")] Book book)
        {
            if (ModelState.IsValid)
            {
                // this should be in a service
                ApplicationUser owner = _context.Users.FirstOrDefault(u => u.Id == User.GetId());
                book.Owner = owner;
                await _bookService.Insert(book);
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            var book = await _bookService.GetById(id);

            if (book == null)
            {
                return NotFound();
            }

            if (!User.IsUser(book.Owner) && !User.IsAdmin())
            {
                return Forbid();
            }

            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Title,Author,PublicationYear,Price")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            var existingBook = await _bookService.GetById(id);
            if (!User.IsUser(existingBook.Owner) && !User.IsAdmin())
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _bookService.Update(book);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
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
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            var book = await _bookService.GetById(id);

            if (book == null)
            {
                return NotFound();
            }
            if (!User.IsUser(book.Owner) && !User.IsAdmin())
            {
                return Forbid();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var book = await _bookService.GetById(id);
            if (book == null)
            {
                return NotFound();
            }
            if (!User.IsUser(book.Owner) && !User.IsAdmin())
            {
                return Forbid();
            }
            await _bookService.Remove(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> MinMaxPricesPerYearReport()
        {
            return View(await _bookService.GetMinMaxPricesPerYearReport());

        }

        private bool BookExists(Guid id)
        {
            return _bookService.GetById(id) != null;
        }
    }
}
