using ass3.Pages.Data;
using ass3.Pages.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace ass3.Pages.Services
{
    public class CVServices
    {
        readonly HttpClient _client = new HttpClient();
        private readonly IHttpContextAccessor _httpContextAccessor ;
        readonly ILogger _Logger;
        private readonly AppDBContext _dbContext ;



        public CVServices(IHttpContextAccessor httpContextAccessor, ILoggerFactory factory,AppDBContext appDBContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _Logger = factory.CreateLogger<CVServices>();
            _dbContext = appDBContext;
        }


        public async Task<List<SelectListItem>> PopulateCountries(CVMODEL CVMODEL)
        {
            var response = await _client.GetAsync("https://restcountries.com/v3.1/all");
            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                var countries = JArray.Parse(jsonData);

                foreach (var country in countries)
                {
                    CVMODEL.CountryList.Add(new SelectListItem { Text = country["name"]["common"].ToString(), Value = country["name"]["common"].ToString() });
                }
            }
            else
            {
                // Handle the API request failure
                await Console.Out.WriteLineAsync("This is an error message for API request failure.");
            }

            return CVMODEL.CountryList;
        }



        public async Task addPhoto(CVMODEL CVMODELS,CVMODELDB CV)
        {
            var file = CVMODELS.Photo;
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            // Define the file path to save the uploaded file
            string filePath = "wwwroot/Uploads/"+uniqueFileName;
            // Save the file to the specified path
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            if (CVMODELS.Photo != null)
            {
                CV.Photo = $"~/Uploads/{uniqueFileName}";
            }
        }


        public async Task addSkills(CVMODEL CVMODELS, CVMODELDB CV)
        {
            var request = _httpContextAccessor.HttpContext.Request;
            foreach (var skills in CVMODELS.SkillsDictionary.Keys)
            {
                bool isChecked = request.Form.ContainsKey($"SkillsDictionary[{skills}]") && request.Form[$"SkillsDictionary[{skills}]"] == "on";
                CVMODELS.SkillsDictionary[skills] = isChecked;
            }
            var skill = CVMODELS.SkillsDictionary.Where(x => x.Value).Select(x => x.Key).ToList();
            CV.Skills = string.Join(", ", skill);
        }



        public List<string> ValidateCVMODELS(CVMODEL CVMODELS,CVMODELDB CV)
        {
            List<string> errors = new List<string>();

            if (CV.Pass != CVMODELS.ConfirmPass)
            {
                errors.Add("The password and confirm password should be equal");
            }

            if (CVMODELS.Verfication.Nb1 + CVMODELS.Verfication.Nb2 != CVMODELS.Verfication.Nb3)
            {
                errors.Add("nb1 + nb2 should equal nb3");
            }

            if (CVMODELS.Photo == null || CVMODELS.Photo.Length < 0 || CVMODELS.Photo.Length > 4096000)
            {
                errors.Add("Please enter a photo or enter a photo less than 4MB");
            }

            try
            {
                if (CVMODELS.Photo != null && CVMODELS.Photo.ContentType != null && !CVMODELS.Photo.ContentType.StartsWith("image/"))
                {
                    errors.Add("Please enter an image, not anything else");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return errors;
        }



        public async Task<List<CVMODELDB>> Search(string key)
        {
            return await _dbContext.CVMODEL
            .Where(r => r.FirstName.StartsWith(key) || r.LastName.StartsWith(key) || r.Email.StartsWith(key) || r.Sex.StartsWith(key))
            .Select(x => new CVMODELDB 
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                BDay = x.BDay,
                Nationality = x.Nationality,
                Sex = x.Sex,
                Skills = x.Skills,
                Email = x.Email,
                Photo = x.Photo
            })
            .ToListAsync();
        }
    }






}

