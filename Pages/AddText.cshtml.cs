using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MongoDB.Bson;
using testingWebApp.Models;

namespace testingWebApp.Pages
{
    public class AddTextModel : PageModel
    {
        readonly TextStoreService _service;
        public List<TextStore> TextStore { get; set; }

        public AddTextModel(TextStoreService service)
        {
            _service = service;
        }

        public void OnPostAddNewAsync(string id, string content)
        {
            _service.Create(new TextStore() {SearchID = id, TextContent = content});
            ViewData["Success"] = "Successfully added something";
        }

        public ActionResult OnPostCheckIdAsync(string id)
        {
            return id != "" ? Content(_service.Get(id) == null ? "Free" : "Exists") : Content("Exists");
        }

        public void OnGet()
        {

        }
    }
}