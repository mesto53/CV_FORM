using ass3.Pages.Data;
using ass3.Pages.Model;
using ass3.Pages.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;



namespace ass3.Pages.RazorPages.CV
{
    public class Index : PageModel
    {
        [BindProperty]

        public string SearchContent { get; set; }
        private AppDBContext AppDBContext { get; set; }
        public List<CVMODELDB> CVMODELS { get; set; } = new List<CVMODELDB>();
        public readonly CVServices services;

        public Index(AppDBContext appDBContext,CVServices c)
        {
            services = c;
            AppDBContext = appDBContext;
        }
        public  void OnGet()
        {
            CVMODELS = AppDBContext.CVMODEL.ToList();
        }

        public void OnGetDelete(int id) {
            CVMODELDB c = AppDBContext.CVMODEL.FirstOrDefault(u => u.Id == id);
            if (c!=null)
            {
              AppDBContext.CVMODEL.Remove(c);
              AppDBContext.SaveChanges();
            }
            CVMODELS = AppDBContext.CVMODEL.ToList();


        }

        public async Task OnPost()
        {

            if (SearchContent != null)
            {
                CVMODELS = await services.Search(SearchContent);
            }
            else
            {
                CVMODELS = AppDBContext.CVMODEL.ToList();
            }

        }
    }
}
