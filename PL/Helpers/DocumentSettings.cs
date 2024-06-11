using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace PL.Helpers
{
    public static class DocumentSettings
    {



        public static string UploadFile(IFormFile file,string FolderName)
        {
            //1)Get Located Folder Path
            string FolderPath=Path.Combine(Directory.GetCurrentDirectory(),@"wwwroot//Files", FolderName);

            //2)Get FileName and make it unique
          string FileName= $"{Guid.NewGuid()}{ file.FileName }";
            /*{Guid.NewGuid()}*/


            //3)Get FilePath

            string FilePath =Path.Combine(FolderPath,FileName);

            //4) Save file as Streams

          using  var FS=new FileStream(FilePath, FileMode.Create);
            file.CopyTo(FS);

            //5)return FileName

            return FileName;





        }

      public static void DeleteFile(string  FileName,string FolderName)
        {
            //1)Get FilePath
            var FilePath = Path.Combine(Directory.GetCurrentDirectory(),@"wwwroot//Files", FolderName, FileName);

            //2)check if FilePath Exists or not

            if (File.Exists(FilePath))
            {
                //If exists Remove it
                File.Delete(FilePath);
            }



        }




    }
}
