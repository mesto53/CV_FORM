using ass3.Pages.Data;
using ass3.Pages.Model;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ass3.Pages.RazorPages.CV
{
    public class DetailsModel : PageModel
    {

         public CVMODELDB myCv { get; set; }
         readonly AppDBContext myAppDBContext;

        public DetailsModel( AppDBContext appDBContext)
        {
            myAppDBContext = appDBContext;
        }
        public void OnGet(int id )
        {
            myCv = myAppDBContext.CVMODEL.FirstOrDefault(U => U.Id == id);

        }
    }
}
