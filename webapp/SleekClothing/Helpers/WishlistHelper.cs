using Newtonsoft.Json;
using SleekClothing.Data;
using SleekClothing.Models;
using System.Security.Claims;

namespace SleekClothing.Helpers
{
    public class WishlistHelper
    {
        #region db

        public static void AddToWishlistDb(Product product, ApplicationDbContext context, ClaimsPrincipal userClaim)
        {
            if (userClaim == null && context == null) { return; }
            var user = UsersHelper.GetUser(context, userClaim);

            // 1. get users existing wishlist
            List<Product> updatedWishlist = GetUserWishlistDb(userClaim, context);

            // 2. add new product to wishlist
            updatedWishlist.Add(product);

            // 3. update users wishlist
            var wishlist = context.UserWishlists.FirstOrDefault(c => c.UserId == user.Id);

            if (wishlist != null)
            {
                wishlist.WishlistDataJSON = JsonConvert.SerializeObject(updatedWishlist, Formatting.Indented);
            }
            else
            {
                context.UserWishlists.Add(new UserWishlist()
                {
                    UserId = user.Id,
                    WishlistDataJSON = JsonConvert.SerializeObject(updatedWishlist, Formatting.Indented),
                });
            }


            // 4. save changes
            context.SaveChanges();
        }

        public static void RemoveFromWishlistDb(Product product, ApplicationDbContext context, ClaimsPrincipal userClaim)
        {
            if (userClaim == null && context == null) { return; }
            var user = UsersHelper.GetUser(context, userClaim);

            var userWishlist = context.UserWishlists.Where(x => x.UserId == user.Id).First();

            if (userWishlist == null) return;

            var productsList = JsonConvert.DeserializeObject<List<Product>>(userWishlist.WishlistDataJSON);

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

                string newWishlistJson = JsonConvert.SerializeObject(productsList, Formatting.Indented);

                //userCart.UserId = user.Id;
                userWishlist.WishlistDataJSON = newWishlistJson;
            }
            else
            {
                return;
            }

            context.SaveChanges();
        }

        public static List<Product> GetUserWishlistDb(ClaimsPrincipal userClaim, ApplicationDbContext context)
        {
            var user = UsersHelper.GetUser(context, userClaim);

            List<Product> products = new List<Product>();

            var cartJson = "";
            try
            {
                cartJson = context.UserWishlists.Where(x => x.UserId == user.Id).First().WishlistDataJSON;
            }
            catch
            {
                return new List<Product>();
                throw;
            }

            products = JsonConvert.DeserializeObject<List<Product>>(cartJson);

            return products;
        }

        #endregion
    }
}
