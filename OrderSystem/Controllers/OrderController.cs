using Microsoft.AspNetCore.Mvc;
using OrderSystem.Data;
using OrderSystem.Models;
using OrderSystem.Models.BusinessLogicLayer;
using OrderSystem.Models.ViewModel;

namespace OrderSystem.Controllers
{
    public class OrderController : Controller
    {
        private readonly OrderSystemBusinessLogicLayer _orderSystemBusinessLogicLayer;
        public OrderController(IRepository<Product> productRepository, IRepository<InCartProduct> inCartProductactorRepository, IRepository<Cart> cartRepository, IRepository<Order> orderRepository, IRepository<Country> countryRepository, IRepository<InOrderProduct> inOrderProductRepository)
        {
            _orderSystemBusinessLogicLayer = new OrderSystemBusinessLogicLayer(productRepository, inCartProductactorRepository, cartRepository, orderRepository, countryRepository, inOrderProductRepository);
        }
        public IActionResult Index(string? name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                HashSet<Product> allProducts = _orderSystemBusinessLogicLayer.SearchProducts(name).ToHashSet();
                return View(allProducts);
            }
            else
            {
                HashSet<Product> allProducts = _orderSystemBusinessLogicLayer.ListProducts().ToHashSet();
                return View(allProducts);
            }
        }
        public IActionResult Cart()
        {
            Cart newCart = _orderSystemBusinessLogicLayer.GetCart();
            ViewBag.Countries = _orderSystemBusinessLogicLayer.GetCountries().ToHashSet();
            if (newCart == null)
            {
                newCart = _orderSystemBusinessLogicLayer.CreateCart();
                return View(newCart);
            }
            return View(newCart);
        }
        public IActionResult AddtoCart(Guid id)
        {
            _orderSystemBusinessLogicLayer.AddProductToCart(id);
            return RedirectToAction("Cart");
        }
        public IActionResult ReduceQuantity(Guid id)
        {
            _orderSystemBusinessLogicLayer.ReduceProduct(id);
            return RedirectToAction("Cart");
        }
        public IActionResult RemoveFromCart(Guid id)
        {
            _orderSystemBusinessLogicLayer.RemoveProduct(id);
            return RedirectToAction("Cart");
        }
        public IActionResult EmptyCart(Guid id)
        {
            _orderSystemBusinessLogicLayer.EmptyCart();
            return RedirectToAction("Cart");
        }
        public IActionResult ConfirmCountry(string selectedCountry)
        {
            if (string.IsNullOrWhiteSpace(selectedCountry))
            {
                ViewBag.Countries = _orderSystemBusinessLogicLayer.GetCountries();
                return RedirectToAction("Cart");
            }
            return RedirectToAction("OrderConfirmation", new { name = selectedCountry });
        }
        public IActionResult OrderConfirmation(string name)
        {
            ConfirmOrderViewModel viewModel = _orderSystemBusinessLogicLayer.ConfirmOrder(name);
            return View(viewModel);
        }
        [HttpPost]
        public IActionResult OrderConfirmation(ConfirmOrderViewModel viewModel)
        {
            viewModel.Cart = _orderSystemBusinessLogicLayer.GetCart();
            viewModel.Country = _orderSystemBusinessLogicLayer.GetSelectedCountry(viewModel.CountryId.ToString());
            _orderSystemBusinessLogicLayer.MakeOrder(viewModel);
            return RedirectToAction("OrderList");
        }
        public IActionResult OrderList()
        {
            HashSet<Order> allOrders = _orderSystemBusinessLogicLayer.ListOrders().ToHashSet();
            return View(allOrders);
        }
    }
}
