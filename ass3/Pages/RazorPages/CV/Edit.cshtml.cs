    using ass3.Pages.Data;
    using ass3.Pages.Model;
    using ass3.Pages.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.Extensions.Caching.Memory;


    namespace ass3.Pages.RazorPages.CV
    {
        public class EditModel : PageModel
        {

            [BindProperty]
            public CVMODELDB CV { get; set; }
            [BindProperty]
            public CVMODEL CVMODELS { get; set; }
            private AppDBContext Db { get; set; }

            private readonly IMemoryCache _cache;
            CVServices Services { get; set; }


        public EditModel(IMemoryCache cache, AppDBContext db,CVServices c)
        {
            _cache = cache;
            CV = new CVMODELDB();
            CVMODELS = new CVMODEL();
            Services = c;
            this.Db = db;
        }


        public async Task OnGetAsync(int id)
        {
           
            if (!_cache.TryGetValue("CountryList", out List<SelectListItem> cachedList))
                {
                    cachedList = await Services.PopulateCountries(CVMODELS);
                    _cache.Set("CountryList", cachedList, TimeSpan.FromMinutes(60)); // Cache for 60 minutes
                }
             CVMODELS.CountryList = cachedList;
             CV = Db.CVMODEL.FirstOrDefault(u => u.Id == id);
             CVMODELS.ConfirmPass = CV.Pass;  
        }





        public async Task<IActionResult> OnPostAsync()
        {


            if (_cache.TryGetValue("CountryList", out List<SelectListItem> cachedList))
                CVMODELS.CountryList = cachedList;
            List<string> errors = Services.ValidateCVMODELS(CVMODELS,CV);

            foreach (var error in errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }
            if (ModelState.IsValid)
            {
                await Services.addPhoto(CVMODELS, CV);
                await Services.addSkills(CVMODELS, CV);
                Db.CVMODEL.Update(CV);
                await Db.SaveChangesAsync();
                return RedirectToPage("Index");
             }
             return Page();

        }
    }
    }
