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
            if (content == "")
            {
                ViewData["Error"] = "Content can't be empty";
                return;
            }

            if ( _service.Create(new TextStore() {SearchID = EncryptionHelper.GenerateHash(id), TextContent = EncryptionHelper.Encrypt(content, id)}))
            {
                ViewData["Success"] = "Successfully added something";
            }
            else
            {
                ViewData["Error"] = "Key already exists";
            }
        }

        public ActionResult OnPostCheckIdAsync(string id)
        {
            return id != "" ? Content(_service.Get(EncryptionHelper.GenerateHash(id)) == null ? "Free" : "Exists") : Content("Exists");
        }

        public void OnGet()
        {

        }
    }
}