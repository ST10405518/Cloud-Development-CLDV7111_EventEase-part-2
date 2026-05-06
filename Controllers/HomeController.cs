using EventEase.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventEase.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.VenueCount = await _context.Venues.CountAsync();
            ViewBag.EventCount = await _context.Events.CountAsync();
            ViewBag.BookingCount = await _context.Bookings.CountAsync();

            var upcomingBookings = await _context.Bookings
                .Include(b => b.Event)
                .Include(b => b.Venue)
                .Where(b => b.StartDate >= DateTime.Now)
                .OrderBy(b => b.StartDate)
                .Take(5)
                .ToListAsync();

            return View(upcomingBookings);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
