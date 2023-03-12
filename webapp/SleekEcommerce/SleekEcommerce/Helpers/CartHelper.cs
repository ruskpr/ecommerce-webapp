using Azure;
using Microsoft.Win32.SafeHandles;
using Newtonsoft.Json;
using SleekEcommerce.Models;
using System;
namespace SleekEcommerce.Helpers
{
    public class CartHelper
    {

        const string COOKIE_NAME = "SleekCartData";

        private static void CreateCart(HttpContext httpContext)
        {
            // create cookie
            var cookieOptions = new CookieOptions
            { 
                Expires = DateTime.Now.AddDays(240)
            };
            httpContext.Response.Cookies.Append(COOKIE_NAME, "", cookieOptions);
            
        }

        public static void AddToCart(Product product, HttpContext httpContext)
        {
            var cookieValue = httpContext.Request.Cookies[COOKIE_NAME];
            if (cookieValue == null)
            {
                CreateCart(httpContext);
            }

            // get current items if there are any
            var cartItems = GetCartItems(httpContext.Request);

            // append to list
            cartItems.Add(product);

            //serialize to JSON 
            var newCookieValue = JsonConvert.SerializeObject(cartItems, Formatting.Indented);

            // update cookie
            httpContext.Response.Cookies.Append(COOKIE_NAME, newCookieValue);
        }

        public static void RemoveFromCart(Product product, HttpContext httpContext)
        {
            // get current items if there are any
            var cartItems = GetCartItems(httpContext.Request);

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

        public static List<Product> GetCartItems(HttpRequest httpRequest)
        {
            var cookieValue = httpRequest.Cookies[COOKIE_NAME];
            if (cookieValue == null)
            {
                return new List<Product>(); // return empty list if cookie is null
            }

            var productsFromCookie = JsonConvert.DeserializeObject<List<Product>>(cookieValue);

            List<Product> products = new List<Product>();
            for (int i = 0; i < productsFromCookie.Count; i++)
            {
                if (products.Where(x => x.Id == productsFromCookie[i].Id).Count() > 0)
                { 
                    products.Last().CartQuantity += 1;
                    continue;
                }

                products.Add(productsFromCookie[i]);
                products.Last().CartQuantity += 1;  
            }


            return products ?? new List<Product>(); // return empty list if cookie is null
        }

        

        public static decimal GetCartTotal(HttpRequest httpRequest)
        {
            decimal total = 0;

            var items = GetCartItems(httpRequest);

            foreach (var item in items)
            {
                total += item.PriceAfterDiscount;
            }

            return total;
        }
    }
}
