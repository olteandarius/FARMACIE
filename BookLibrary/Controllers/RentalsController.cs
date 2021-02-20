using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookLibrary.Data;
using BookLibrary.Models;
using Microsoft.AspNetCore.Authorization;

namespace BookLibrary.Controllers
{
    public class RentalsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RentalsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Rentals
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Rentals.Include(r => r.Book).Include(r => r.Client).OrderBy(r => r.Client.Name);
            return View(await applicationDbContext.ToListAsync());
        }

        private void PopulateRentalDropdowns(Rental rental)
        {
            rental.Books = new List<SelectListItem>();
            rental.ClientCNPs = new List<SelectListItem>();

            foreach (var b in _context.Books)
            {
                rental.Books.Add(new SelectListItem { Text = $"{b.Title} by {b.Author}", Value = b.Id.ToString() });
            }
            foreach (var c in _context.Clients)
            {
                rental.ClientCNPs.Add(new SelectListItem { Text = $"{c.Name} ({c.CNP})", Value = c.Id.ToString() });
            }

        }

        // GET: Rentals/Create
        [Authorize]
        public IActionResult Create()
        {
            Rental rental = new Rental();
            PopulateRentalDropdowns(rental);
            return View(rental);
        }

        // POST: Rentals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("BookId,ClientId,RentalDate,RentalDuration,ReturnDate")] Rental rental)
        {
            if (ModelState.IsValid)
            {
                rental.Id = Guid.NewGuid();
                _context.Add(rental);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateRentalDropdowns(rental);
            return View(rental);
        }

        [Authorize]
        public IActionResult Return(Guid? id)
        {
            if (id != null && RentalExists(id.Value))
            {
                return View();
            }

            return NotFound();
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Return(RentalReturnViewModel rentalReturnViewModel)
        {
            if (ModelState.IsValid)
            {
                var rental = await _context.Rentals.FirstOrDefaultAsync(r => r.Id == rentalReturnViewModel.Id);
                if (rental == null)
                {
                    return NotFound();
                }

                rental.RentalDuration = (rentalReturnViewModel.ActualReturnDate - rental.RentalDate).Days;
                if (rental.RentalDuration < 0)
                {
                    ModelState.AddModelError("ActualReturnDate", "The actual return date cannot be before the rental date!");
                    return View(rentalReturnViewModel);
                }
                _context.Rentals.Attach(rental);
                var entry = _context.Entry(rental);
                entry.Property(p => p.RentalDuration).IsModified = true;
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(rentalReturnViewModel);
        }


        private bool RentalExists(Guid id)
        {
            return _context.Rentals.Any(e => e.Id == id);
        }
    }
}
