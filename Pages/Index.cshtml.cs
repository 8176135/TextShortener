using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Driver;
using MongoDB.Bson;
using testingWebApp.Models;

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
            var res = _service.Get(id);
            return Content(res == null ? "" : res.TextContent);
        }
    }
}