using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32.SafeHandles;
using Newtonsoft.Json;
using SleekClothing.Models;
using System;
using System.Collections.Generic;

namespace SleekClothing.Helpers
{
    public class CartHelper
    {

        const string COOKIE_NAME = "SLKCARTDATA";

        private static void CreateCart(HttpContext httpContext)
        {
            // create cookie
            var cookieOptions = new CookieOptions
            { 
                Expires = DateTime.Now.AddDays(240)
            };

            //httpContext.Response.Cookies.Delete(COOKIE_NAME);
            httpContext.Response.Cookies.Append(COOKIE_NAME, "", cookieOptions);
            
        }

        public static void AddToCart(Product newProduct, HttpContext httpContext)
        {
            List<Product> cartItems = new List<Product>();
            var cookieValue = httpContext.Request.Cookies[COOKIE_NAME];
            if (cookieValue == null)
            {
                CreateCart(httpContext);
            }
            else
            {
                cartItems = JsonConvert.DeserializeObject<List<Product>>(cookieValue);
            }


            // append to list
            cartItems.Add(newProduct);

            //serialize to JSON 
            var newCookieValue = JsonConvert.SerializeObject(cartItems, Formatting.Indented);

            // update cookie
            //httpContext.Response.Cookies.Delete(COOKIE_NAME);
            httpContext.Response.Cookies.Append(COOKIE_NAME, newCookieValue);
        }

        public static void RemoveFromCart(Product product, HttpContext httpContext)
        {
            // get current items if there are any
            var cookieValue = httpContext.Request.Cookies[COOKIE_NAME];
            if (cookieValue == "" || cookieValue == string.Empty) return;

            var cartItems = JsonConvert.DeserializeObject<List<Product>>(cookieValue);

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

        public static void DeleteCart(HttpContext httpContext)
        {
            httpContext.Response.Cookies.Delete(COOKIE_NAME);
        }

        // get list of one of each item in cart but filtered with the correct quantity
        public static List<Product> GetGroupedCartItems(HttpRequest httpRequest)
        {
            var cookieValue = httpRequest.Cookies[COOKIE_NAME];
            if (cookieValue == null)
            {
                return new List<Product>(); // return empty list if cookie is null
            }

            var products = JsonConvert.DeserializeObject<List<Product>>(cookieValue);

            // group each unique item into its own list 
            var group = products
            .GroupBy(u => u.Id)
            .Select(grp => grp.ToList())
            .ToList();

            // loop through each grouped list and add it to the returned list 
            // with the correct quantity of that unique item
            List<Product> groupedProducts = new List<Product>();
            foreach (var list in group)
            {
                var product = list[0];
                product.CartQuantity = list.Count();
                groupedProducts.Add(product);
                continue;
            }

            return groupedProducts ?? new List<Product>(); // return empty list if cookie is null
        }
  
        // total items in cart
        public static int GetCartItemsCount(HttpContext httpContext)
        {
            var cookieValue = httpContext.Request.Cookies[COOKIE_NAME];
            if (cookieValue == null)
            {
                return 0; // return 0 if no cart exists from cookie
            }

            var products = JsonConvert.DeserializeObject<List<Product>>(cookieValue);
            
            return products.Count();
        }

        public static decimal GetCartTotal(HttpRequest httpRequest)
        {
            decimal total = 0;

            var items = GetGroupedCartItems(httpRequest);

            foreach (var item in items)
            {
                total += item.PriceAfterDiscount * item.CartQuantity;
            }

            return total;
        }
    }
}
