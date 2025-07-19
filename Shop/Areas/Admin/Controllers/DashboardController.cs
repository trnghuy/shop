using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Models;
using Shop.Repository;

namespace Shop.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Route("Admin/Dashboard")]
    [Authorize(Roles = "Quản trị viên, Nhân viên")]
    public class DashboardController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly DataContext _dataContext;


        public DashboardController(DataContext context, IWebHostEnvironment webHostEnvironment)
        {
            _dataContext = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var count_product = _dataContext.Products.Count();
            var count_order = _dataContext.Orders.Count();
            var count_category = _dataContext.Categories.Count();
            var count_user = _dataContext.Users.Count();
            ViewBag.CountProduct = count_product;
            ViewBag.CountOrder = count_order;
            ViewBag.CountCategory = count_category;
            ViewBag.CountUser = count_user;
            return View();
        }




        [HttpPost]

        public IActionResult GetChartData()
        {

            var data = _dataContext.StatisticaModels.Select(s => new
            {
                date = s.DateCreated.ToString("yyyy-MM-dd"),
                sold = s.Sold,
                quantity = s.Quantity,
                revenue = s.Revenue,
                profit = s.Profit
            }).ToList();

            return Json(data);
        }





        [HttpPost]

        public IActionResult GetChartDataBySelect(DateTime startDate, DateTime endDate)
        {
            var data = _dataContext.StatisticaModels
                .Where(s => s.DateCreated >= startDate && s.DateCreated <= endDate)
                .Select(s => new
                {
                    date = s.DateCreated.ToString("yyyy-MM-dd"),
                    sold = s.Sold,
                    quantity = s.Quantity,
                    revenue = s.Revenue,
                    profit = s.Profit
                })
                .ToList();


            Console.WriteLine($"Querying from {startDate} to {endDate}, found {data.Count} records");

            return Json(data);
        }





        [HttpPost]

        public IActionResult FilterData(DateTime? fromDate, DateTime? toDate)
        {
            var query = _dataContext.StatisticaModels.AsQueryable();

            if (fromDate.HasValue)
            {
                query = query.Where(s => s.DateCreated >= fromDate);
            }

            if (toDate.HasValue)
            {
                query = query.Where(s => s.DateCreated <= toDate);
            }

            var data = query
                .Select(s => new
                {
                    date = s.DateCreated.ToString("yyyy-MM-dd"),
                    sold = s.Sold,
                    quantity = s.Quantity,
                    revenue = s.Revenue,
                    profit = s.Profit
                })
                .ToList();

            return Json(data);
        }





    }
}
