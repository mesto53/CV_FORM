using ass3.Pages.Data;
using ass3.Pages.Model;
using ass3.Pages.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;




namespace ass3.Pages.RazorPages.CV
{
    public class Create : PageModel
    {
        

        [BindProperty]
        public CVMODELDB CV { get; set; }
        [BindProperty]
        public CVMODEL CVMODEL { get; set; } 
        private AppDBContext Db { get; set; }

        private readonly IMemoryCache _cache;
        CVServices Services { get; set; }


        public Create (IMemoryCache cache,AppDBContext db,CVServices c)
        {
            _cache = cache;
            CV =new CVMODELDB();
            CVMODEL = new CVMODEL();
            Services = c;
            this.Db = db;
        }


        public async Task OnGetAsync()
        {
            if (!_cache.TryGetValue("CountryList", out List<SelectListItem> cachedList))
               {
                cachedList = await Services.PopulateCountries(CVMODEL);
               _cache.Set("CountryList", cachedList, TimeSpan.FromMinutes(60)); // Cache for 60 minutes
               }  
               CVMODEL.CountryList = cachedList;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (_cache.TryGetValue("CountryList", out List<SelectListItem> cachedList))
                CVMODEL.CountryList = cachedList;
            List<string> errors = Services.ValidateCVMODELS(CVMODEL, CV);

            foreach (var error in errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }

            if (ModelState.IsValid )
            {
                await Services.addPhoto(CVMODEL,CV);
                await Services.addSkills(CVMODEL, CV);
                await Db.CVMODEL.AddAsync(CV);
                await Db.SaveChangesAsync();
                return RedirectToPage("Index");
            }
            return Page();
            
        }
    }
}
