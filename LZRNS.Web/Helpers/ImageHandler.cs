using LZRNS.DomainModel.Models;
using LZRNS.DomainModels.Repository.Interfaces;
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
				string image = Path.Combine("\\Uploads\\" + targetModel.ImageFile.FileName);
                targetModel.ImageFile.SaveAs(uploadDirectoryPath + image);
                return image;
            }
            return null;
        }


        public static void RemoveImage(object model, ObjectType objType)
        {
			dynamic targetModel;

			if(objType.Equals(ObjectType.PLAYER))
				targetModel = model as Player;
			else
				targetModel = model as Team;

			if (!string.IsNullOrWhiteSpace(targetModel.Image) && System.IO.File.Exists(HttpContext.Current.Server.MapPath("~") + targetModel.Image))
            {
				//System.IO.File.Delete(HttpContext.Current.Server.MapPath("~") + image);
				targetModel.Image = null;
				targetModel.ImageFile = null;
			}

        }
    }
}