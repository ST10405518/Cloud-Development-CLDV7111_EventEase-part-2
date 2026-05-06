using EventEase.Data;
using EventEase.Models;
using EventEase.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventEase.Controllers
{
    public class VenuesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly BlobStorageService _blobStorageService;

        public VenuesController(ApplicationDbContext context, BlobStorageService blobStorageService)
        {
            _context = context;
            _blobStorageService = blobStorageService;
        }

        // GET: Venues
        public async Task<IActionResult> Index(string searchString)
        {
            var venues = from v in _context.Venues select v;

            if (!string.IsNullOrEmpty(searchString))
            {
                venues = venues.Where(v => v.VenueName.Contains(searchString) || v.Location.Contains(searchString));
            }

            ViewData["CurrentFilter"] = searchString;
            return View(await venues.ToListAsync());
        }

        // GET: Venues/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venue = await _context.Venues
                .Include(v => v.Bookings)
                .ThenInclude(b => b.Event)
                .FirstOrDefaultAsync(m => m.VenueId == id);

            if (venue == null)
            {
                return NotFound();
            }

            return View(venue);
        }

        // GET: Venues/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Venues/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VenueName,Location,Capacity")] Venue venue, IFormFile? imageFile)
        {
            if (ModelState.IsValid)
            {
                // Upload image to Azure Blob Storage if provided
                if (imageFile != null && imageFile.Length > 0)
                {
                    venue.ImageUrl = await _blobStorageService.UploadImageAsync(imageFile);
                }

                venue.CreatedDate = DateTime.Now;
                venue.ModifiedDate = DateTime.Now;
                _context.Add(venue);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(venue);
        }

        // GET: Venues/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venue = await _context.Venues.FindAsync(id);
            if (venue == null)
            {
                return NotFound();
            }
            return View(venue);
        }

        // POST: Venues/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VenueId,VenueName,Location,Capacity,CreatedDate")] Venue venue, IFormFile? imageFile)
        {
            if (id != venue.VenueId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingVenue = await _context.Venues.FindAsync(id);
                    if (existingVenue == null)
                    {
                        return NotFound();
                    }

                    // Upload new image to Azure Blob Storage if provided
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        // Delete old image if exists
                        if (!string.IsNullOrEmpty(existingVenue.ImageUrl))
                        {
                            await _blobStorageService.DeleteImageAsync(existingVenue.ImageUrl);
                        }
                        venue.ImageUrl = await _blobStorageService.UploadImageAsync(imageFile);
                    }
                    else
                    {
                        venue.ImageUrl = existingVenue.ImageUrl;
                    }

                    venue.ModifiedDate = DateTime.Now;
                    _context.Entry(existingVenue).CurrentValues.SetValues(venue);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VenueExists(venue.VenueId))
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
            return View(venue);
        }

        // GET: Venues/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venue = await _context.Venues
                .FirstOrDefaultAsync(m => m.VenueId == id);

            if (venue == null)
            {
                return NotFound();
            }

            // Check if venue has bookings
            var hasBookings = await _context.Bookings.AnyAsync(b => b.VenueId == id);
            ViewBag.HasBookings = hasBookings;

            return View(venue);
        }

        // POST: Venues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Check if venue has bookings - prevent deletion
            var hasBookings = await _context.Bookings.AnyAsync(b => b.VenueId == id);
            if (hasBookings)
            {
                ModelState.AddModelError(string.Empty, "Cannot delete venue with existing bookings. Please remove all bookings first.");
                var venue = await _context.Venues.FindAsync(id);
                ViewBag.HasBookings = true;
                return View(venue);
            }

            var venueToDelete = await _context.Venues.FindAsync(id);
            if (venueToDelete != null)
            {
                // Delete image from Azure Blob Storage if exists
                if (!string.IsNullOrEmpty(venueToDelete.ImageUrl))
                {
                    await _blobStorageService.DeleteImageAsync(venueToDelete.ImageUrl);
                }

                _context.Venues.Remove(venueToDelete);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool VenueExists(int id)
        {
            return _context.Venues.Any(e => e.VenueId == id);
        }
    }
}
