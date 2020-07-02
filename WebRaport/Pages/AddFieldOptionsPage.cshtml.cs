using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebRaport.Models;

namespace WebRaport.Pages
{
    public class AddFieldOptionsPageModel : PageModel
    {
        [BindProperty]
        public FieldModel fieldModel { get; set; }

        public AddFieldOptionsPageModel()
        {
            fieldModel = new FieldModel();
        }

        public void OnGet()
        {

        }

        public IActionResult OnPost()
        {
            return RedirectToPage("FieldTablePage", "AddField");
        }
    }
}