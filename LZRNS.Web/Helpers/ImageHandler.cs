using LZRNS.DomainModel.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace LZRNS.Web.Helpers
{
    public enum ObjectType
    {
        PLAYER,
        TEAM
    };

    public class ImageHandler
    {



        public static string SaveImage(object model, ObjectType objType)
        {
            string uploadDirectoryPath = HttpContext.Current.Server.MapPath("~");

            if (!Directory.Exists(uploadDirectoryPath))
            {
                Directory.CreateDirectory(uploadDirectoryPath);
            }
            //only two options - player or team
            dynamic targetModel;
            if (objType.Equals(ObjectType.PLAYER))
            {
                targetModel = model as Player;
            }
            else
            {
                targetModel = model as Team;
            }
            if (targetModel.ImageFile != null)
            {


                string image = Path.Combine("\\Uploads\\" + targetModel.Id.ToString() + targetModel.ImageFile.FileName);
                targetModel.ImageFile.SaveAs(uploadDirectoryPath + image);
                return image;
            }
            return null;
        }


        public static void RemoveImage(string image)
        {
            if (!string.IsNullOrWhiteSpace(image) && System.IO.File.Exists(HttpContext.Current.Server.MapPath("~") +  image))
            {
                System.IO.File.Delete(HttpContext.Current.Server.MapPath("~") + image);
            }

        }
    }
}