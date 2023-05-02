using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.Win32.SafeHandles;
using Newtonsoft.Json;
using SleekClothing.Data;
using SleekClothing.Models;
using SleekClothing.Pages.products;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace SleekClothing.Helpers
{
    public class CartHelper
    {

        const string COOKIE_NAME = "SLKCARTDATA";

        #region cookie logic

        private static void CreateCartCookie(HttpContext httpContext)
        {
            // create cookie
            var cookieOptions = new CookieOptions
            { 
                Expires = DateTime.Now.AddDays(240)
            };

            //httpContext.Response.Cookies.Delete(COOKIE_NAME);
            httpContext.Response.Cookies.Append(COOKIE_NAME, "", cookieOptions);
            
        }

        public static List<Product> GetUserCartCookie(HttpContext httpContext)
        {
            // get cookie that is named COOKIE_NAME
            var cookieValue = httpContext.Request.Cookies[COOKIE_NAME];
            if (cookieValue == null)
            {
                return new List<Product>();
            }

            return JsonConvert.DeserializeObject<List<Product>>(cookieValue) ?? new List<Product>();
        }

        public static void AddToCartCookie(Product newProduct, HttpContext httpContext)
        {
            List<Product> cartItems = new List<Product>();
            var cookieValue = httpContext.Request.Cookies[COOKIE_NAME];
            if (cookieValue == null)
            {
                CreateCartCookie(httpContext);
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

        public static void RemoveFromCartCookie(Product product, HttpContext httpContext)
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

        public static void DeleteCartCookie(HttpContext httpContext)
        {
            httpContext.Response.Cookies.Delete(COOKIE_NAME);
        }

        // get list of one of each item in cart but filtered with the correct quantity
        public static List<Product> GetGroupedCartItemsCookie(HttpRequest httpRequest)
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
        public static int GetCartItemsCountCookie(HttpContext httpContext)
        {
            var cookieValue = httpContext.Request.Cookies[COOKIE_NAME];
            if (cookieValue == null)
            {
                return 0; // return 0 if no cart exists from cookie
            }

            var products = JsonConvert.DeserializeObject<List<Product>>(cookieValue);
            
            return products.Count();
        }

        public static decimal GetCartTotalCookie(HttpRequest httpRequest)
        {
            decimal total = 0;

            var items = GetGroupedCartItemsCookie(httpRequest);

            foreach (var item in items)
            {
                total += item.PriceAfterDiscount * item.CartQuantity;
            }

            return total;
        }

        #endregion

        #region db

        public static void AddToCartDb(Product product, ApplicationDbContext context, ClaimsPrincipal userClaim)
        {
            if (userClaim == null || context == null) return; 
            var user = UsersHelper.GetUser(context, userClaim);
            // 1. get users existing cart
            List<Product> updatedCart = GetUserCartDb(user.Id, context);

            // 2. add new product to cart

            updatedCart.Add(product);

            // 3. update users cart
            var cart = context.UserCarts.FirstOrDefault(c => c.UserId == user.Id);

            if (cart != null)
            {
                cart.UserId = user.Id;
                cart.CartDataJSON = JsonConvert.SerializeObject(updatedCart, Formatting.Indented);
            }
            else
            {
                context.UserCarts.Add(new UserCart()
                {
                    UserId = user.Id,
                    CartDataJSON = JsonConvert.SerializeObject(updatedCart, Formatting.Indented),
                });
            }


            // 4. save changes
            context.SaveChanges();
        }

        internal static void ClearCartDb(ClaimsPrincipal userClaim, ApplicationDbContext context)
        {
            if (userClaim == null && context == null) { return; }
            var user = UsersHelper.GetUser(context, userClaim);

            var userCart = context.UserCarts.Where(x => x.UserId == user.Id).First();

            if (userCart == null) return;

            var productsList = JsonConvert.DeserializeObject<List<Product>>(userCart.CartDataJSON);

            if (productsList != null)
            {
                productsList.Clear();

                string newCartJson = JsonConvert.SerializeObject(productsList, Formatting.Indented);

                //userCart.UserId = user.Id;
                userCart.CartDataJSON = newCartJson;
            }
            else
            {
                return;
            }

            context.SaveChanges();
        }
        public static void RemoveFromCartDb(Product product, ApplicationDbContext context, ClaimsPrincipal userClaim)
        {
            if (userClaim == null && context == null) { return; }
            var user = UsersHelper.GetUser(context, userClaim);

            var userCart = context.UserCarts.Where(x => x.UserId == user.Id).First();

            if (userCart == null) return;

            var productsList = JsonConvert.DeserializeObject<List<Product>>(userCart.CartDataJSON);

            if (productsList != null)
            {
                foreach (var item in productsList)
                {
                    if (item.Id == product.Id)
                    {
                        productsList.Remove(item);
                        break;
                    }
                }

                string newCartJson = JsonConvert.SerializeObject(productsList, Formatting.Indented);

                //userCart.UserId = user.Id;
                userCart.CartDataJSON = newCartJson;
            }
            else
            {
                return;
            }

            context.SaveChanges();
        }

        public static List<Product> GetUserCartDb(string userid, ApplicationDbContext context)
        {
            List<Product> products = new List<Product>();

            var cartJson = "";
            try
            {
                cartJson = context.UserCarts.Where(x => x.UserId == userid).First().CartDataJSON;
            }
            catch 
            {
                return new List<Product>();
            }

            products = JsonConvert.DeserializeObject<List<Product>>(cartJson);

            return products;
        }

        // get list of one of each item in cart but filtered with the correct quantity
        public static List<Product> GetGroupedCartItemsDb(string userid, ApplicationDbContext context)
        {
            try
            {
                var userCart = context.UserCarts.Where(x => x.UserId == userid).First();

                if (userCart == null)
                {
                    context.UserCarts.Add(new UserCart()
                    {
                        UserId = userid,
                        CartDataJSON = "[]",
                    });

                    context.SaveChanges();
                    return new List<Product>();
                }


                string cartJson = userCart.CartDataJSON;

                if (string.IsNullOrEmpty(cartJson)) return new List<Product>(); // return empty list if no cart exists

                var products = JsonConvert.DeserializeObject<List<Product>>(cartJson);

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
                }

                return groupedProducts ?? new List<Product>(); // return empty list if cookie is null
            }
            catch { return new List<Product>(); }
            
        }

        // total items in cart
        public static int GetCartItemsCountDb(string userid, ApplicationDbContext context)
        {
            try
            {
                var cart = context.UserCarts.Where(x => x.UserId == userid).First();

                if (cart == null)
                {
                    return 0;
                }

                var products = JsonConvert.DeserializeObject<List<Product>>(cart.CartDataJSON);

                return products.Count();
            }
            catch {return 0;}
        }

        public static decimal GetCartTotalDb(string userid, ApplicationDbContext context)
        {
            decimal total = 0;

            var items = GetGroupedCartItemsDb(userid, context);

            foreach (var item in items)
            {
                total += item.PriceAfterDiscount * item.CartQuantity;
            }

            return total;
        }

        #endregion

        #region convert cookie items to db items

        public static void ConvertToDB(HttpContext httpContext, ClaimsPrincipal user, ApplicationDbContext context)
        {
            var cookieItems = GetUserCartCookie(httpContext);

            if (cookieItems.Count == 0) return;

            foreach (var product in cookieItems)
            {
                AddToCartDb(product, context, user);
            }

            DeleteCartCookie(httpContext);
        }

        
        #endregion
    }
}
