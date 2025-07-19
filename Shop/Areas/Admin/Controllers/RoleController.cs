using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Models;
using Shop.Repository;
using System.Threading.Tasks;

namespace Shop.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Route("Admin/Role")]
    [Authorize(Roles = "Quản trị viên")]
    public class RoleController : Controller
    {
        
        private readonly DataContext _dataContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        public RoleController(DataContext context, RoleManager<IdentityRole> roleManager)
        {
            _dataContext = context;
            _roleManager = roleManager;
        }
        //[Route("Index")]
        public async Task<IActionResult> Index()
        {
            return View(await _dataContext.Roles.OrderByDescending(r => r.Id).ToListAsync());
        }

        [HttpGet]
        //[Route("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        //[Route("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IdentityRole model)
        {
            if (!_roleManager.RoleExistsAsync(model.Name).GetAwaiter().GetResult()) 
            {
                _roleManager.CreateAsync(new IdentityRole(model.Name)).GetAwaiter().GetResult();
            }
            return Redirect("Index");
        }

        [HttpGet]
        //[Route("Edit")]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            var role = await _roleManager.FindByIdAsync(id);
            return View(role);
        }

        [HttpPost]
        //[Route("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, IdentityRole model)
        {
            if (string.IsNullOrEmpty(id)) { 
                return NotFound();
            }
            if (ModelState.IsValid) { 
                var role = await _roleManager.FindByIdAsync(id);
                if (role == null)
                {
                    return NotFound();
                } 
                role.Name = model.Name;
                try
                {
                    await _roleManager.UpdateAsync(role);
                    TempData["success"] = "Sửa chức vụ thành công";
                    return RedirectToAction("Index");   
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi khi sửa chức vụ");
                }
            }
            return View(model ?? new IdentityRole { Id = id});
        }

        [HttpGet]
        //[Route("Delete")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            try
            {
                await _roleManager.DeleteAsync(role);
                TempData["success"] = "Xóa chức vụ thành công";
            }
            catch (Exception ex) {
                ModelState.AddModelError("", "Lỗi khi xóa chức vụ");
            }
            return Redirect("Index");
        }
    }
}
