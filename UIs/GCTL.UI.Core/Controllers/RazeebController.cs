using GCTL.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace GCTL.UI.Core.Controllers
{
    public class RazeebController : Controller
    {
        private readonly ApplicationDbContext _db;
        public RazeebController(ApplicationDbContext context)
        {
            _db = context;
        }
        public IActionResult Index()
        {
            ViewBag.Name = _db.ProductCategory.Select(x => new { 
            x.Name,
            x.Id
            } ).Where(x => x.Id == 1).FirstOrDefault();
            return View();
        }
    }
}
