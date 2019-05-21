using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using testingWebApp.Models;
using Microsoft.AspNetCore.Identity;

namespace testingWebApp.Pages
{
    public class IndexModel : PageModel
    {
//        public List<Movie> Movie { get; set; }
        readonly TextStoreService _service;

        public IndexModel(TextStoreService service)
        {
            _service = service;
        }

        public void OnGet()
        {

        }

        public ContentResult OnPostSearchAsync(string id)
        {
            var res = _service.Get(EncryptionHelper.GenerateHash(id));
            return Content(res == null ? "" : EncryptionHelper.Decrypt(res.TextContent.AsByteArray,id));
        }
    }
}