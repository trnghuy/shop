using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shop.Models;
using Shop.Repository;

namespace Shop.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Route("Admin/Contact")]
    [Authorize(Roles = "Quản trị viên")]
    public class ContactController : Controller
    {
        private readonly DataContext _dataContext;
        public ContactController(DataContext context)
        {
            _dataContext = context;
        }
        //[Route("Index")]
        public IActionResult Index()
        {
            var contact = _dataContext.Contacts.ToList();
            return View(contact);
        }
        //[Route("Edit")]
        public async Task<IActionResult> Edit()
        {
            ContactModel contact = await _dataContext.Contacts.FirstOrDefaultAsync();
            return View(contact);
        }
        //[Route("Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ContactModel contact)
        {
            var existed_contact = _dataContext.Contacts.FirstOrDefault(); 

            if (ModelState.IsValid)
            {
                existed_contact.Name = contact.Name;
                existed_contact.Map = contact.Map;
                existed_contact.Description = contact.Description;
                existed_contact.Email = contact.Email;
                existed_contact.Phone = contact.Phone;


                _dataContext.Update(existed_contact);
                await _dataContext.SaveChangesAsync();
                TempData["success"] = "Cập nhật thành công";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "Model đang bị lỗi";
                List<string> errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors);
                return BadRequest(errorMessage);
            }

            return View(contact);
        }
    }
}
