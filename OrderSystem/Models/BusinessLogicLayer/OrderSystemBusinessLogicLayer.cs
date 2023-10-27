using Microsoft.Identity.Client;
using OrderSystem.Data;
using OrderSystem.Models.ViewModel;
using System.Data;

namespace OrderSystem.Models.BusinessLogicLayer
{
    public class OrderSystemBusinessLogicLayer
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<InCartProduct> _inCartProductRepository;
        private readonly IRepository<InOrderProduct> _inOrderProductRepository;
        private readonly IRepository<Cart> _cartRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<Country> _countryRepository;
        public OrderSystemBusinessLogicLayer(IRepository<Product> productRepository, IRepository<InCartProduct> inCartProductactorRepository, IRepository<Cart> cartRepository, IRepository<Order> orderRepository, IRepository<Country> countryRepository, IRepository<InOrderProduct> inOrderProductRepository)
        {
            _productRepository = productRepository;
            _inCartProductRepository = inCartProductactorRepository;
            _cartRepository = cartRepository;
            _orderRepository = orderRepository;
            _countryRepository = countryRepository;
            _inOrderProductRepository = inOrderProductRepository;
        }
        public Product GetProductById(Guid id)
        {
            Product product = _productRepository.Get(id);
            if (product == null)
            {
                throw new InvalidOperationException();
            }
            return product;
        }
        public Cart GetCart()
        {
            Cart cart = _cartRepository.GetAll().FirstOrDefault();
            return cart;
        }
        public ICollection<Product> ListProducts()
        {
            return _productRepository.GetAll().OrderBy(p => p.Name).ToHashSet();
        }
        public ICollection<Product> SearchProducts(string name)
        {
            return _productRepository.GetAll().Where(p => p.Name.Contains(name) || p.Description.Contains(name)).OrderBy(p => p.Name).ToHashSet();
        }
        public Cart CreateCart()
        {
            Cart cart = new Cart
            {
                Products = new HashSet<InCartProduct>()
            };
            return cart;
        }
        public Cart AddProductToCart(Guid id)
        {
            Product addProduct = GetProductById(id);
            Cart cart = _cartRepository.GetAll().FirstOrDefault();
            InCartProduct existingInCartProduct = new InCartProduct();
            if (cart == null)
            {
                cart = CreateCart();
            }
            if (cart.Products.Any())
            {
                existingInCartProduct = cart.Products.FirstOrDefault(icp => icp.Product.Id == id);
            }           
            if (existingInCartProduct != null && existingInCartProduct.Product != null)
            {
                existingInCartProduct.Quantity++;
                _inCartProductRepository.Update(existingInCartProduct);
            }
            else
            {
                InCartProduct inCartProduct = new InCartProduct
                {
                    Product = addProduct,
                    Quantity = 1
                };
                cart.Products.Add(inCartProduct);
                _inCartProductRepository.Create(inCartProduct);
            }
            addProduct.AvailableQuantity--;
            _productRepository.Update(addProduct);
            _cartRepository.Update(cart);
            return cart;
        }
        public Cart ReduceProduct(Guid id)
        {
            Product reduceProduct = GetProductById(id);
            Cart cart = _cartRepository.GetAll().First();
            InCartProduct existingInCartProduct = cart.Products.First(p => p.Product.Id == id);
            if (existingInCartProduct.Quantity > 1)
            {
                existingInCartProduct.Quantity--;
            } else
            {
                cart.Products.Remove(existingInCartProduct);
                _inCartProductRepository.Delete(existingInCartProduct);
            }
            reduceProduct.AvailableQuantity++;
            _productRepository.Update(reduceProduct);
            _cartRepository.Update(cart);
            return cart;
        }
        public Cart RemoveProduct(Guid id)
        {
            Product reduceProduct = GetProductById(id);
            Cart cart = _cartRepository.GetAll().First();
            InCartProduct existingInCartProduct = cart.Products.First(p => p.Product.Id == id);
            reduceProduct.AvailableQuantity += existingInCartProduct.Quantity;
            _productRepository.Update(reduceProduct);
            cart.Products.Remove(existingInCartProduct);
            _inCartProductRepository.Delete(existingInCartProduct);
            _cartRepository.Update(cart);
            return cart;
        }
        public Cart EmptyCart()
        {
            Cart cart = _cartRepository.GetAll().First();
            HashSet<InCartProduct> inCartProducts = _inCartProductRepository.GetAll().ToHashSet();
            foreach (InCartProduct inCartProduct in inCartProducts)
            {
                Product emptyProduct = GetProductById(inCartProduct.Product.Id);
                emptyProduct.AvailableQuantity += inCartProduct.Quantity;
                _productRepository.Update(emptyProduct);
                _inCartProductRepository.Delete(inCartProduct);
            }
            cart.Products.Clear();
            _cartRepository.Update(cart);
            return cart;
        }
        public ICollection<Country> GetCountries()
        {
            HashSet<Country> countries = _countryRepository.GetAll().ToHashSet();
            return countries;
        }
        public Country GetSelectedCountry(string name)
        {
            Country country = GetCountries().First(c => c.Id.ToString() == name);
            return country;
        }
        public ConfirmOrderViewModel ConfirmOrder(string name)
        {
            Cart cart = GetCart();
            Country country = GetSelectedCountry(name);
            decimal convertedPrice = 0;
            foreach (InCartProduct product in cart.Products)
            {
                convertedPrice += product.Product.Price * product.Quantity;
            }
            convertedPrice *= country.ConversionRate;
            decimal totalPrice = convertedPrice * (decimal)1.07;
            ConfirmOrderViewModel viewModel = new ConfirmOrderViewModel
            {
                Cart = cart,
                Country = country,
                CountryId = country.Id,
                ConvertedPrice = convertedPrice,
                TotalPrice = totalPrice,
            };
            return viewModel;
        }
        public Order MakeOrder(ConfirmOrderViewModel viewModel)
        {
            HashSet<InOrderProduct> products = new HashSet<InOrderProduct>();
            foreach(InCartProduct product in viewModel.Cart.Products)
            {
                InOrderProduct inOrderProduct = new InOrderProduct
                {
                    Product = product.Product,
                    Quantity = product.Quantity
                };
                products.Add(inOrderProduct);
            }
            Order order = new Order
            {
                Products = products,
                DestinationCountry = viewModel.Country.Name,
                Address = viewModel.Address,
                MailingCode = viewModel.MailingCode,
                TotalPrice = viewModel.TotalPrice,
            };
            _orderRepository.Create(order);
            Cart cart = _cartRepository.GetAll().First();
            if (_inCartProductRepository.GetAll() != null)
            {
                HashSet<InCartProduct> inCartProducts = _inCartProductRepository.GetAll().ToHashSet();
                foreach (InCartProduct inCartProduct in inCartProducts)
                {
                    _inCartProductRepository.Delete(inCartProduct);
                }
            }           
            _cartRepository.Delete(cart);
            return order;
        }
        public ICollection<Order> ListOrders()
        {
            return _orderRepository.GetAll().ToHashSet();
        }
    }
}
