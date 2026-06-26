using Newtonsoft.Json;
using ShopEase.Models;

namespace ShopEase.Data
{
    public static class CartHelper
    {
        private const string CartKey = "ShopEaseCart";

        public static List<CartItem> GetCart(ISession session)
        {
            var cartJson = session.GetString(CartKey);
            return cartJson == null ? new List<CartItem>() : JsonConvert.DeserializeObject<List<CartItem>>(cartJson)!;
        }

        public static void SaveCart(ISession session, List<CartItem> cart)
        {
            session.SetString(CartKey, JsonConvert.SerializeObject(cart));
        }

        public static void AddToCart(ISession session, CartItem item)
        {
            var cart = GetCart(session);
            var existing = cart.FirstOrDefault(c => c.ProductId == item.ProductId);
            if (existing != null)
                existing.Quantity += item.Quantity;
            else
                cart.Add(item);
            SaveCart(session, cart);
        }

        public static void RemoveFromCart(ISession session, int productId)
        {
            var cart = GetCart(session);
            cart.RemoveAll(c => c.ProductId == productId);
            SaveCart(session, cart);
        }

        public static void UpdateQuantity(ISession session, int productId, int quantity)
        {
            var cart = GetCart(session);
            var item = cart.FirstOrDefault(c => c.ProductId == productId);
            if (item != null)
            {
                if (quantity <= 0)
                    cart.Remove(item);
                else
                    item.Quantity = quantity;
            }
            SaveCart(session, cart);
        }

        public static void ClearCart(ISession session)
        {
            session.Remove(CartKey);
        }

        public static int GetCartCount(ISession session)
        {
            return GetCart(session).Sum(c => c.Quantity);
        }
    }
}
