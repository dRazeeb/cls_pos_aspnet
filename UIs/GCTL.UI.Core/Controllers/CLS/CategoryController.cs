using GCTL.Data.Models;
using GCTL.UI.Core.ViewModels.Category;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GCTL.UI.Core.Controllers.Razeeb
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext context)
        {
            _db = context;
        }
        public IActionResult Index()
        {
            ViewBag.Name = _db.ProductCategory.Select(x => new {
                x.Name,
                x.Id
            }).Where(x => x.Id == 1).FirstOrDefault();
            return View();
        }
        [HttpPost]
        public JsonResult LoadData()
        {
            // Search Value from (Search box)  
            string searchStr = Request.Form["search[value]"].FirstOrDefault();
            // Sort Column Name  
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            // Sort Column Direction (asc, desc)  
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            // Paging Length 10,20  
            var length = Request.Form["length"].FirstOrDefault();
            // Skip number of Rows count  
            var start = Request.Form["start"].FirstOrDefault();
            //Paging Size (10, 20, 50,100)  
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skipRecords = start != null ? Convert.ToInt32(start) : 0;
            int totalRec;


            IEnumerable<CategoryTableViewModel> courses = GetCourses(searchStr, sortColumn, sortColumnDirection, pageSize, skipRecords, out totalRec);
            return Json(new { data = courses, recordsFiltered = totalRec, recordsTotal = totalRec });
        }

        private IEnumerable<CategoryTableViewModel> GetCourses(string searchStr, string sortCol, string sortDir, int pageSize, int skipRecords, out int totalRec)
        {

            // get all course from DB
            // search need specify menually like: .Where( c => c.CourseName.Contains(searchStr) || c.Duration.Contains(searchStr)).AsEnumerable<Course>();
            IEnumerable<CategoryTableViewModel> lstCourse = _db.ProductCategory
                .Include(x => x.CreatedByNavigation)
                .Include(x => x.Parent)
                .Where(
                c => c.Id.ToString().Contains(searchStr) ||
                c.Name.Contains(searchStr) 
                //c.ParentId.Contains(searchStr) ||
                //c.email.Contains(searchStr) ||
                //c.mobile.Contains(searchStr) ||
                //c.Company.name.Contains(searchStr) ||
                //c.address.Contains(searchStr)
                )
                 .Select(c => new CategoryTableViewModel
                 {
                     Id = c.Id,
                     Name = c.Name,
                     ParentName = c.Parent.Name,
                     CreatedByName = c.CreatedByNavigation.Username,
                     CreatedAt = c.CreatedAt,
                 }
                ).OrderBy(x =>x.Name).AsEnumerable<CategoryTableViewModel>();
            // get total records
            totalRec = lstCourse.Count();


            // menually sort column name add like:  case "CID":  lstCourse = sortDir.ToLower() == "asc" ? lstCourse.OrderBy(c => c.CourseID) : lstCourse.OrderByDescending(c => c.CourseID); break;
            switch (sortCol)
            {
                case "id":
                    lstCourse = sortDir.ToLower() == "asc" ? lstCourse.OrderBy(c => c.Id) : lstCourse.OrderByDescending(c => c.Id);
                    break;
                case "name":
                    lstCourse = sortDir.ToLower() == "asc" ? lstCourse.OrderBy(c => c.Name) : lstCourse.OrderByDescending(c => c.Name);
                    break;
                //case "GuestId":
                //    lstCourse = sortDir.ToLower() == "asc" ? lstCourse.OrderBy(c => c.GuestId) : lstCourse.OrderByDescending(c => c.GuestId);
                //    break;
                //case "Email":
                //    lstCourse = sortDir.ToLower() == "asc" ? lstCourse.OrderBy(c => c.Email) : lstCourse.OrderByDescending(c => c.Email);
                //    break;
                //case "Mobile":
                //    lstCourse = sortDir.ToLower() == "asc" ? lstCourse.OrderBy(c => c.Mobile) : lstCourse.OrderByDescending(c => c.Mobile);
                //    break;
                //case "CompanyName":
                //    lstCourse = sortDir.ToLower() == "asc" ? lstCourse.OrderBy(c => c.CompanyName) : lstCourse.OrderByDescending(c => c.CompanyName);
                //    break;
                //case "Address":
                //    lstCourse = sortDir.ToLower() == "asc" ? lstCourse.OrderBy(c => c.Address) : lstCourse.OrderByDescending(c => c.Address);
                //    break;
            }

            lstCourse = lstCourse.Skip(skipRecords).Take(pageSize);


            return lstCourse;
        }
    }
}
