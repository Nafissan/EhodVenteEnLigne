using Microsoft.AspNetCore.Mvc;
using EhodBoutiqueEnLigne.Models.Services;

namespace EhodBoutiqueEnLigne.Components
{
    public class LanguageSelectorViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(ILanguageService languageService)
        {
            return View(languageService);
        }
    }
}
