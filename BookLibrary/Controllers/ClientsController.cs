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
    public class ClientsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IClientService _clientService;

        public ClientsController(ApplicationDbContext context, IClientService clientService)
        {
            _context = context;
            _clientService = clientService;
        }

        // GET: Clients
        public async Task<IActionResult> Index()
        {
            return View(await _context.Clients.ToListAsync());
        }

        // GET: Clients/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            var client = await _clientService.GetById(id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // GET: Clients/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,Name,DateOfBirth,CNP")] Client client)
        {
            if (ModelState.IsValid)
            {
                // this should be in a service
                ApplicationUser owner = _context.Users.FirstOrDefault(u => u.Id == User.GetId());
                client.Owner = owner;
                await _clientService.Insert(client);
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        // GET: Clients/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            var client = await _clientService.GetById(id);
            if (client == null)
            {
                return NotFound();
            }

            if (!User.IsUser(client.Owner) && !User.IsAdmin())
            {
                return Forbid();
            }

            return View(client);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,DateOfBirth,CNP")] Client client)
        {
            if (id != client.Id)
            {
                return NotFound();
            }

            var existingClient = await _clientService.GetById(id);
            if (!User.IsUser(existingClient.Owner) && !User.IsAdmin())
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(client);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientExists(client.Id))
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
            return View(client);
        }

        // GET: Clients/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            var client = await _clientService.GetById(id);
            if (client == null)
            {
                return NotFound();
            }

            if (!User.IsUser(client.Owner) && !User.IsAdmin())
            {
                return Forbid();
            }

            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var client = await _clientService.GetById(id);
            if (!User.IsUser(client.Owner) && !User.IsAdmin())
            {
                return Forbid();
            }
            await _clientService.Remove(id);
            return RedirectToAction(nameof(Index));
        }

        private bool ClientExists(Guid id)
        {
            return _context.Clients.Any(e => e.Id == id);
        }
    }
}
