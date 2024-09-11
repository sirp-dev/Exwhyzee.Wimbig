using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exwhyzee.Wimbig.Application.Images;
using Exwhyzee.Wimbig.Application.Raffles;
using Exwhyzee.Wimbig.Application.Raffles.Dto;
using Exwhyzee.Wimbig.Core.Images;
using Exwhyzee.Wimbig.Core.Raffles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exwhyzee.Wimbig.Web.Areas.Admin.Pages.RaffleManagement
{
    [Authorize(Roles = "mSuperAdmin,SuperAdmin,Admin")]
    public class ImageModel : PageModel
    {
        private readonly IImageFileAppService _imgFileAppSevice;

        public ImageModel(IImageFileAppService imgFileAppSevice)
        {
            _imgFileAppSevice = imgFileAppSevice;
        }

        public PagedList<ImageFile> ImageFiles { get; set; }
        List<ImageFile> imageFile = new List<ImageFile>();

        public async Task<IActionResult> OnGetAsync()
        {
            var paggedImg = await _imgFileAppSevice.GetAllImages();
            var paggedSource = paggedImg.Source.Where(x=>x.Status == Enums.EntityStatus.Active).ToList();


            imageFile.AddRange(paggedSource.Select(x => new ImageFile()
            {
               Url = x.Url,
               Status = x.Status,
               IsDefault = x.IsDefault,
               Id = x.Id

            }));

             ImageFiles = new PagedList<ImageFile>(source: imageFile, pageIndex: paggedImg.PageIndex,
                                            pageSize: paggedImg.PageSize, filteredCount: paggedImg.FilteredCount, totalCount:
                                            paggedImg.TotalCount);
            return Page();
        }

    }
}