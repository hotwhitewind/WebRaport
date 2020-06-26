using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GleamTech.DocumentUltimate;
using GleamTech.DocumentUltimate.AspNet.UI;
using Microsoft.AspNetCore.Mvc;

namespace WebRaport.Controllers
{
    public class DocumentViewController : Controller
    {
        public IActionResult Index()
        {
            var documentViewer = new DocumentViewer
            {
                Width = 800,
                Height = 600,
                Resizable = true,
                Document = "~/App_Data/ExampleFiles/Default.docx"
            };
            //var stream = new FileStream("c:\\Project\\WebRaport\\WebRaport\\wwwroot\\App_Data\\ExampleFiles\\Default.docx", FileMode.Open, FileAccess.Read);

            //documentViewer.DocumentSource = new DocumentSource(
            //  new DocumentInfo("d", "Default.docx"),
            //  new StreamResult(stream));
            return View(documentViewer);
        }
    }
}