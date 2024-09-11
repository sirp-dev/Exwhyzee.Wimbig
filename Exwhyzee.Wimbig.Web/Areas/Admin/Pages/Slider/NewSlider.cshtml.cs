using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Exwhyzee.Enums;
using Exwhyzee.Wimbig.Application.Barner;
using Exwhyzee.Wimbig.Core.BarnerImage;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exwhyzee.Wimbig.Web.Areas.Admin.Pages.Slider
{
    public class NewSliderModel : PageModel
    {
        private readonly IBarnerAppService _barnerAppService;
        private readonly IHostingEnvironment _hostingEnv;


        public NewSliderModel(IBarnerAppService barnerAppService, IHostingEnvironment env)
        {
            _barnerAppService = barnerAppService;
            _hostingEnv = env;
        }
        public BarnerFile barnerFile { get; set; }



        public async Task OnGet(string returnUrl = null)
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {

                if (ModelState.IsValid)
                {


                    #region Raffle Image(s)
                    int imgCount = 0;
                    if (HttpContext.Request.Form.Files != null && HttpContext.Request.Form.Files.Count > 0)
                    {
                        var newFileName = string.Empty;
                        var filePath = string.Empty;
                        string pathdb = string.Empty;
                        var files = HttpContext.Request.Form.Files;
                        foreach (var file in files)
                        {
                            if (file.Length > 0)
                            {
                                filePath = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                                imgCount++;
                                var now = DateTime.Now;
                                var uniqueFileName = $"{now.Year}{now.Month}{now.Day}_{now.Hour}{now.Minute}{now.Second}{now.Millisecond}".Trim();

                                var fileExtension = Path.GetExtension(filePath);

                                newFileName = uniqueFileName + fileExtension;

                                // if you wish to save file path to db use this filepath variable + newFileName
                                var fileDbPathName = $"/Barner/".Trim();

                                filePath = $"{_hostingEnv.WebRootPath}{fileDbPathName}".Trim();

                                if (!(Directory.Exists(filePath)))
                                    Directory.CreateDirectory(filePath);

                                var fileName = "";
                                fileName = filePath + $"{newFileName}".Trim();


                                // copy the file to the desired location from the tempMemoryLocation of IFile and flush temp memory
                                using (FileStream fs = System.IO.File.Create(fileName))
                                {
                                    file.CopyTo(fs);
                                    fs.Flush();
                                }

                                #region Save Image Propertie to Db
                                var img = new BarnerFile()
                                {
                                    Url = $"{fileDbPathName}/{newFileName}",
                                    Extension = fileExtension,
                                    DateCreated = DateTime.UtcNow.AddHours(1),
                                    Status = EntityStatus.Active,
                                    IsDefault = imgCount == 1 ? true : false,
                                };
                                var saveImageToDb = await _barnerAppService.Insert(img);

                                #endregion

                            }
                        }
                    }
                    #endregion

                    return RedirectToPage("Index");
                }

                return Page();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Helper Agents
        private static string CurrentYear()
        {
            var currentYear = DateTime.Now;
            return currentYear.Year.ToString();
        }


        #endregion
    }
}