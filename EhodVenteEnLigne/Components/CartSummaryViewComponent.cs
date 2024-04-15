using Microsoft.AspNetCore.Mvc;
using EhodBoutiqueEnLigne.Models;

namespace EhodBoutiqueEnLigne.Components
{
    public class CartSummaryViewComponent : ViewComponent
    {
        private readonly Cart _cart;

        public CartSummaryViewComponent(ICart cart)
        {
            _cart = cart as Cart;
        }

        public IViewComponentResult Invoke()
        {
            return View(_cart);
        }
    }
}
