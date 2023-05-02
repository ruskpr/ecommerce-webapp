using Newtonsoft.Json;
using SleekClothing.Data;
using SleekClothing.Models;
using System.Security.Claims;

namespace SleekClothing.Helpers
{
    public class WishlistHelper
    {

        public static void AddToWishlist(Product product, ApplicationDbContext context, ClaimsPrincipal userClaim)
        {
            if (userClaim == null && context == null) return;

            var user = UsersHelper.GetUser(context, userClaim);

            // 1. get users existing wishlist
            List<Product> oldWishlist = GetUserWishlist(userClaim, context) ?? new List<Product>();

            if (oldWishlist.Any(x => x.Id == product.Id)) return;

            // 2. add new product to wishlist
            oldWishlist.Add(product);

            // 3. update users wishlist
            var newWishlist = context.UserWishlists.FirstOrDefault(c => c.UserId == user.Id);

            if (newWishlist != null)
            {
                newWishlist.WishlistDataJSON = JsonConvert.SerializeObject(oldWishlist, Formatting.Indented);
            }
            else
            {
                context.UserWishlists.Add(new UserWishlist()
                {
                    UserId = user.Id,
                    WishlistDataJSON = JsonConvert.SerializeObject(oldWishlist, Formatting.Indented),
                });
            }


            // 4. save changes
            context.SaveChanges();
        }

        public static void RemoveFromWishlist(Product product, ApplicationDbContext context, ClaimsPrincipal userClaim)
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

        public static List<Product> GetUserWishlist(ClaimsPrincipal userClaim, ApplicationDbContext context)
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

    }
}
