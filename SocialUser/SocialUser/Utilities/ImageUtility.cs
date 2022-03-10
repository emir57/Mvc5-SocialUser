using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace SocialUser.Utilities
{
    public static class ImageUtility
    {
        public static void UploadImage(HttpPostedFileBase file,out string databasePath,string path)
        {
            string getEx = Path.GetExtension(file.FileName);
            if (getEx != ".jpg" || getEx != ".png")
            {
                string filename = Guid.NewGuid() + getEx;
                file.SaveAs(Path.Combine(path, filename));
                databasePath = "../../Content/postPicture/" + filename;
            }
            else { databasePath = ""; }
        }
        public static void DeleteImage(string fullPath)
        {
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
        }
    }
}