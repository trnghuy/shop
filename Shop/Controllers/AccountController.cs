using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Shop.Areas.Admin.Repository;
using Shop.Models;
using Shop.Models.ViewModels;
using Shop.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Shop.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<AppUserModel> _userManager;
        private SignInManager<AppUserModel> _signManager;
        private readonly IEmailSender _emailSender;
        private readonly DataContext _dataContext;

        public AccountController(UserManager<AppUserModel> userManager, SignInManager<AppUserModel> signManager, IEmailSender emailSender, DataContext dataContext)
        {
            _userManager = userManager;
            _signManager = signManager;
            _emailSender = emailSender;
            _dataContext = dataContext;
        }

        public IActionResult Login(string returnUrl)
        {

            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }




        [HttpPost]
        public async Task<IActionResult> UpdateInfoAccount()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userById = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (userById == null)
            {
                return NotFound();
            }

            _dataContext.Update(userId);
            await _dataContext.SaveChangesAsync();
            TempData["success"] = "Cập nhật thông tin tài khoản thành công";

            return RedirectToAction("UpdateAccount", "Account");
        }





        public async Task<IActionResult>History()
        {
            if ((bool)!User.Identity?.IsAuthenticated)
            {
               
                return RedirectToAction("Login", "Account"); 
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            var Orders = await _dataContext.Orders
                .Where(od => od.UserName == userEmail).OrderByDescending(od => od.Id).ToListAsync();
            ViewBag.UserEmail = userEmail;
            return View(Orders);
        }

        public async Task<IActionResult> CancelOrder(string ordercode)
        {
            if ((bool)!User.Identity?.IsAuthenticated)
            {
                
                return RedirectToAction("Login", "Account");
            }
            try
            {
                var order = await _dataContext.Orders.Where(o => o.OrderCode == ordercode).FirstAsync();
                order.Status = 3;
                _dataContext.Update(order);
                await _dataContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                return BadRequest("Đã xảy ra lỗi khi hủy đơn hàng.");
            }


            return RedirectToAction("History", "Account");
        }


        public async Task<IActionResult> NewPass(AppUserModel user, string token)
        {
            var checkuser = await _userManager.Users
                .Where(u => u.Email == user.Email)
                .Where(u => u.Token == user.Token).FirstOrDefaultAsync();
            if(checkuser != null)
            {
                ViewBag.Email = checkuser.Email;
                ViewBag.Token = token;
            }
            else
            {
                TempData["error"] = "Cập nhật mật khẩu thành công";
                return RedirectToAction("ForgetPass", "Account");
            }
            return View();
        }

        // Cập nhật mật khẩu
        [HttpPost]
        public async Task<IActionResult> UpdateNewPassword(AppUserModel user, string token)
        {
            var checkuser = await _userManager.Users
                .Where(u => u.Email == user.Email)
                .Where(u => u.Token == user.Token).FirstOrDefaultAsync();
            if (checkuser != null)
            {
                string newtoken = Guid.NewGuid().ToString();
                var passwordHasher = new PasswordHasher<AppUserModel>();
                var passwordHash = passwordHasher.HashPassword(checkuser, user.PasswordHash);

                checkuser.PasswordHash = passwordHash;
                checkuser.Token = newtoken;

                await _userManager.UpdateAsync(checkuser);
                TempData["success"] = "Cập nhật mật khẩu thành công";
                return RedirectToAction("Login", "Account");
            }
            else
            {
                TempData["error"] = "Email hoặc token không tồn tại";
                return RedirectToAction("ForgetPass", "Account");
            }
            return View();
        }



        public async Task<IActionResult> ForgetPass(string returnUrl)
        {
            return View();
        }
        public async Task<IActionResult> SendMailForgotPass(AppUserModel user)
        {
            var checkMail = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (checkMail == null)
            {
                TempData["error"] = "Email không tồn tại.";
                return RedirectToAction("ForgetPass", "Account");
            }
            else
            {
                string token = Guid.NewGuid().ToString();
                checkMail.Token = token;
                _dataContext.Update(checkMail);
                await _dataContext.SaveChangesAsync();

                var recriver = checkMail.Email;
                var subject = "Đổi mật khẩu cho tài khoản " + checkMail.Email;
                var message = "Bấm vào đây để đổi mật khẩu " + "<a href='" + $"{Request.Scheme}://{Request.Host}/Account/NewPass?email=" + checkMail.Email + "&token=" + token + "'>";
                await _emailSender.SendEmailAsync(recriver, subject, message);
            }

            return RedirectToAction("ForgetPass", "Account");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _signManager.PasswordSignInAsync(loginVM.Username, loginVM.Password, false, false);
                if (result.Succeeded)
                {
                    TempData["success"] = "Đăng nhập thành công!";
                    return RedirectToAction("Index", "Home"); // Chuyển về trang chính sau đăng nhập
                }

                ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không đúng");
            }

            return View(loginVM);
        }


        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> Register(UserModel user)
        {
            if (ModelState.IsValid) 
            { 
                AppUserModel newUser = new AppUserModel { UserName = user.Username, Email = user.Email};
                IdentityResult result = await _userManager.CreateAsync(newUser, user.Password);
                if (result.Succeeded)
                {
                    TempData["success"] = "Tạo tài khoản thành công";
                    return Redirect("/account/login");
                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(user);
        }

        public async Task<IActionResult> Logout(string returnUrl = "/")
        {
            await _signManager.SignOutAsync();
            return Redirect(returnUrl);
        }



        public async Task LoginByGoogle()
        {
           
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme,
                new AuthenticationProperties
                {
                    RedirectUri = Url.Action("GoogleResponse")
                });
        }


        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

            if (!result.Succeeded)
            {
                return RedirectToAction("Login");
            }

            var claims = result.Principal.Identities.FirstOrDefault()?.Claims;
            var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            string emailName = email?.Split('@')[0];

            var existingUser = await _userManager.FindByEmailAsync(email);

            if (existingUser == null)
            {
                var passwordHasher = new PasswordHasher<AppUserModel>();
                var hashedPassword = passwordHasher.HashPassword(null, "123456789");

                var newUser = new AppUserModel
                {
                    UserName = emailName,
                    Email = email,
                    PasswordHash = hashedPassword
                };

                var createUserResult = await _userManager.CreateAsync(newUser);

                if (!createUserResult.Succeeded)
                {
                    TempData["error"] = "Đăng ký tài khoản thất bại. Vui lòng thử lại sau.";
                    return RedirectToAction("Login", "Account");
                }

                await _signManager.SignInAsync(newUser, isPersistent: false);
                TempData["success"] = "Đăng nhập Google thành công!";
                return RedirectToAction("Index", "Home"); // ✅ Chuyển về trang chính sau khi đăng ký qua Google
            }
            else
            {
                await _signManager.SignInAsync(existingUser, isPersistent: false);
                TempData["success"] = "Đăng nhập Google thành công!";
                return RedirectToAction("Index", "Home"); // ✅ Chuyển về trang chính sau khi đã tồn tại
            }
        }


    }
}
