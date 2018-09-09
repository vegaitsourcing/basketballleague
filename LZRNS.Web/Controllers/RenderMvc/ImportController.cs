using LZRNS.Models.DocumentTypes.Pages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using ExL = LZRNS.ExcelLoader;


namespace LZRNS.Web.Controllers.RenderMvc
{
    public class ImportController : RenderMvcController
    {
        public ActionResult Index(ImportModel model)
        {
            return CurrentTemplate(model);
        }


        [HttpPost]
        public ActionResult Index(HttpPostedFileBase[] files)
        {
            var content = new ImportModel();
            if (files == null || files.Length == 0)
            {
                var currentPageId = UmbracoContext.PageId;
                

                return CurrentTemplate(content);
            }

            ExL.ExcelLoader loader = new ExL.ExcelLoader();

            foreach (var file in files)
            {
                if(file != null)
                {
                    var memStr = GetFileAsMemoryStream(file);
                    
                    //file.FileName
                    loader.ProcessFile(memStr, file.FileName);
                   

                }
            }

            
            return CurrentTemplate(content);
        }

        private MemoryStream GetFileAsMemoryStream(HttpPostedFileBase uploadedFile)
        {
            var buf = new byte[uploadedFile.InputStream.Length];
            uploadedFile.InputStream.Read(buf, 0, (int)uploadedFile.InputStream.Length);
            MemoryStream memStr = new MemoryStream(buf);
            return memStr;
        }
    }
}