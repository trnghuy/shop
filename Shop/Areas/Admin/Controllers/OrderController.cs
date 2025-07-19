using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Repository;
using System.Threading.Tasks;

namespace Shop.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Route("Admin/Order")]
    [Authorize(Roles = "Quản trị viên, Nhân viên")]

    public class OrderController : Controller
    {
        private readonly DataContext _dataContext;
        public OrderController(DataContext context)
        {
            _dataContext = context;
        }
        //[Route("Index")]
        public async Task<IActionResult> Index()
        {
            return View(await _dataContext.Orders.OrderByDescending(p => p.Id).ToListAsync());
        }
        [HttpGet]
        //[Route("ViewOrder")]
        public async Task<IActionResult> ViewOrder(string orderCode)
        {
            var detailOrder = await _dataContext.OrderDetails.Include(od => od.Product).Where(od => od.OrderCode == orderCode).ToListAsync();
            var ShippingCost = _dataContext.Orders.Where(o => o.OrderCode == orderCode).First();
            ViewBag.ShippingCost = ShippingCost.ShippingCode;
            return View(detailOrder);
        }
        [HttpGet]
        //[Route("Delete")]
        public async Task<IActionResult> Delete(string ordercode)
        {
            var order = await _dataContext.Orders.FirstOrDefaultAsync(o => o.OrderCode == ordercode);

            if (order == null)
            {
                return NotFound();
            }
            try
            {

                //delete order
                _dataContext.Orders.Remove(order);


                await _dataContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                return StatusCode(500, "An error occurred while deleting the order.");
            }
        }





    }
}
