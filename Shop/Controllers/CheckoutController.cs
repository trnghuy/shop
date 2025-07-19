using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Shop.Areas.Admin.Repository;
using Shop.Models;
using Shop.Models.ViewModels;
using Shop.Repository;
using Shop.Services.Vnpay;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Shop.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IEmailSender _emailSender;
        private readonly IVnPayService _vnPayService;
        public CheckoutController(DataContext context, IEmailSender emailSender, IVnPayService vnPayService)
        {
            _vnPayService = vnPayService;
            _dataContext = context;
            _emailSender = emailSender;
        }
        public async Task<IActionResult> Checkout(string PaymentMethod, string PaymentId)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                var orderCode = Guid.NewGuid().ToString();
                var orderItem = new OrderModel();
                orderItem.OrderCode = orderCode;
                var shippingPriceCookie = Request.Cookies["ShippingPrice"];
                decimal shippingPrice = 0;
                if (shippingPriceCookie != null)
                {
                    var shippingPriceJson = shippingPriceCookie;
                    shippingPrice = JsonConvert.DeserializeObject<decimal>(shippingPriceJson);
                }
                else
                {
                    shippingPrice = 0;
                }
                orderItem.ShippingCode = shippingPrice;
                orderItem.UserName = userEmail;
                if (PaymentMethod != "VnPay")
                {
                    orderItem.PaymentMethod = "COD";
                }
                else if (PaymentMethod == "VnPay")
                {
                    orderItem.PaymentMethod = "VnPay" + " " + PaymentId;
                }

                orderItem.Status = 1;
                orderItem.CreatedDate = DateTime.Now;
                _dataContext.Add(orderItem);
                _dataContext.SaveChanges();
                
                List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel> { };
                foreach (var cart in cartItems) 
                {
                    var orderDetails = new OrderDetails();
                    orderDetails.UserName = userEmail;
                    orderDetails.OrderCode = orderCode;
                    orderDetails.ProductId = cart.ProductId;
                    orderDetails.Price = cart.Price;
                    orderDetails.Quantity = cart.Quantity;
                    var product = await _dataContext.Products.Where(p => p.Id == cart.ProductId).FirstAsync();
                    product.Quantity -= cart.Quantity;
                    product.Sold += cart.Quantity;
                    _dataContext.Update(product);
                      
                    _dataContext.Add(orderDetails);
                    _dataContext.SaveChanges();
                }
                HttpContext.Session.Remove("Cart");
                // Gửi email
                var receiver = userEmail;
                var subject = "Đặt hàng thành công";
                var message = "Đặt hàng thành công, cảm ơn bạn đã sử dụng dịch vụ.";

                await _emailSender.SendEmailAsync(receiver, subject, message);

                TempData["success"] = "Thanh toán thành công, vui lòng chờ duyệt đơn hàng";
                return RedirectToAction("Index", "Cart");
            }
                return View();
        }
        [HttpGet]
        public async Task<IActionResult> PaymentCallbackVnpay()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);
            if (response.VnPayResponseCode == "00")
            {
                var newVnpayInsert = new VnpayModel()
                {
                    OrderId = response.OrderId,
                    PaymentMethod = response.PaymentMethod,
                    OrderDescription = response.OrderDescription,
                    TransactionId = response.TransactionId,
                    PaymentId = response.PaymentId,
                    DateCreated = DateTime.Now
                };
                _dataContext.Add(newVnpayInsert);
                await _dataContext.SaveChangesAsync();
                var PaymentMethod = response.PaymentMethod;
                var PaymentId = response.PaymentId;

                await Checkout(PaymentMethod, PaymentId);
            }
            else
            {
                TempData["error"] = "Thanh toán VnPay không thành công";
                return RedirectToAction("Index", "Cart");
            }

            return View(response);
        }

    }
}
//{
//    "orderDescription": "dtm030624 Thanh toán qua VnPay 300000",
//  "transactionId": "14898178",
//  "orderId": "638798243921838506",
//  "paymentMethod": "VnPay",
//  "paymentId": "14898178",
//  "success": true,
//  "token": "0eb8787c0cd23a19fd03d9eaaceadabcfa4c2a2ee2e2d9114422ad33ad09c68f577a788d5b948c8b850d0c3cc7a84a81bd3305b59e31d846036dc50e873dd404",
//  "vnPayResponseCode": "00"
//}