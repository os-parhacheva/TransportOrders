using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;
using TransportOrders.Presentation.Models;
using TransportOrders.Presentation.Models.DTO;
using TransportOrders.Presentation.Services;

namespace TransportOrders.Presentation.Controllers
{
    public class HomeController : Controller
    {       
        private readonly OrderService _orderService;

        public HomeController(OrderService orderService)
        {          
            _orderService = orderService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var orders = await _orderService.GetOrdersAsync();
                return View(orders);
            }
            catch (HttpRequestException ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
            catch (JsonException ex)
            {                
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
            catch (Exception ex)
            {               
                ViewBag.ErrorMessage = "Произошла непредвиденная ошибка. Пожалуйста, попробуйте позже.";
                return View("Error");
            }

        }
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(id);

                if (order == null )
                {
                    return NotFound();
                }

                return PartialView("ViewOrder", order);
            }
            catch (HttpRequestException ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
            catch (JsonException ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }

        public IActionResult CreateOrder() => View();
        
        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);
            try
            {           
                var(success, errors) = await _orderService.CreateOrderAsync(model);

                if (success)
                    return RedirectToAction("Index");

                if (errors != null)
                {
                    foreach (var error in errors)
                    {
                        foreach (var message in error.Value)
                        {
                            ModelState.AddModelError(error.Key, message);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Ошибка при создании заказа");
                }

                return View(model);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
