using CocktionMVC.Functions;
using CocktionMVC.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace CocktionMVC.Controllers.ApiControllers
{
    public class UploadFileController : ApiController
    {
        [HttpPost]
        [Authorize]
        public UploadInfo Post()
        {
            UploadInfo info;
            string filePath = "";
            string fileName = "";
            HttpPostedFile postedFile = null;
            var httpRequest = HttpContext.Current.Request;
            var fuckField = HttpContext.Current.Request.Form.GetValues("fuck")[0];
            if (httpRequest.Files.Count > 0)
            {
                foreach (string file in httpRequest.Files)
                {
                    postedFile = httpRequest.Files[file];
                    filePath = HttpContext.Current.Server.MapPath("~/Images/Photos/" + postedFile.FileName);
                    fileName = postedFile.FileName;
                    postedFile.SaveAs(filePath);

                }
                info = new UploadInfo() { Status = fuckField };
            }
            else
            {
                info = new UploadInfo() { Status = "failed" };
            }
            return info;
        }
    }

    public class UploadInfo
    {
        public string Status { get; set; }
        
    }
}
