using Azure;
using Newtonsoft.Json;
using SleekEcommerce.Models;
using System;
namespace SleekEcommerce.Helpers
{
    public class CartHelper
    {

        const string COOKIE_NAME = "SleekCartData";

        public void CreateCart(HttpContext httpContext)
        {
            // create cookie
            var cookieOptions = new CookieOptions
            { 
                Expires = DateTime.Now.AddDays(240)
            };
            httpContext.Response.Cookies.Append(COOKIE_NAME, "", cookieOptions);
            
        }

        public void AddToCart(Product product, HttpRequest httpRequest, HttpContext httpContext)
        {
            var cookieValue = httpRequest.Cookies[COOKIE_NAME];
            if (cookieValue == null)
            {
                CreateCart(httpContext);
            }

            // get current items if there are any
            var cartItems = GetCartItems(httpRequest);

            // append to list
            cartItems.Add(product);

            //serialize to JSON 
            var newCookieValue = JsonConvert.SerializeObject(cartItems, Formatting.Indented);

            // update cookie
            httpContext.Response.Cookies.Append(COOKIE_NAME, newCookieValue);
        }

        public void RemoveFromCart(Product product, HttpRequest httpRequest, HttpContext httpContext)
        {
            var cookieValue = httpRequest.Cookies[COOKIE_NAME];

            // get current items if there are any
            var cartItems = GetCartItems(httpRequest);

            // append to list
            foreach (var item in cartItems)
            {
                if (item.Id == product.Id)
                {
                    cartItems.Remove(item);
                    break;
                }
            }

            //serialize to JSON 
            var newCookieValue = JsonConvert.SerializeObject(cartItems, Formatting.Indented);

            // update cookie
            httpContext.Response.Cookies.Append(COOKIE_NAME, newCookieValue);
        }

        public List<Product> GetCartItems(HttpRequest httpRequest)
        {
            var cookieValue = httpRequest.Cookies[COOKIE_NAME];
            if (cookieValue == null)
            {
                return new List<Product>(); // return empty list if cookie is null
            }

            var cartItems = JsonConvert.DeserializeObject<List<Product>>(cookieValue);

            return cartItems ?? new List<Product>(); // return empty list if cookie is null
        }
    }
}
