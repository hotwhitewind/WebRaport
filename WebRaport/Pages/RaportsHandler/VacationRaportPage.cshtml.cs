using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GleamTech.DocumentUltimate.AspNet.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace WebRaport.Pages.RaportsHandler
{
    [ValidateAntiForgeryToken]
    public class VacationRaportPageModel : PageModel
    {
        [BindProperty]
        public int UserId { get; set; }
        [BindProperty]
        public int RaportId { get; set; }
        [BindProperty]
        public DocumentViewer documentViewer { get; set; }
        [BindProperty]
        public string Message { get; set; }

        public VacationRaportPageModel()
        {
            documentViewer = new DocumentViewer
            {
                Width = 800,
                Height = 600,
                Resizable = true
            };
        }

        public void OnGet(int userId = 0, int raportId = 0)
        {
            UserId = userId;
            RaportId = raportId;
        }

        public PartialViewResult OnGetResult()
        {
            return new PartialViewResult
            {
                ViewName = "DocumentViewPartialModal",
                ViewData = new ViewDataDictionary<DocumentViewer>(ViewData, documentViewer)
            };
            //return Partial("DocumentViewPartialModal", documentViewer);
        }

        public IActionResult OnPost()
        {
            documentViewer = new DocumentViewer
            {
                Width = 800,
                Height = 600,
                Resizable = true,
                Document = "~/App_Data/Raports_Template/Рапорт на отпуск 2020_1.docx"
            };
            //var stream = new FileStream("c:\\Project\\WebRaport\\WebRaport\\wwwroot\\App_Data\\ExampleFiles\\Default.docx", FileMode.Open, FileAccess.Read);

            //documentViewer.DocumentSource = new DocumentSource(
            //  new DocumentInfo("d", "Default.docx"),
            //  new StreamResult(stream));
            return Page();
        }
    }
}