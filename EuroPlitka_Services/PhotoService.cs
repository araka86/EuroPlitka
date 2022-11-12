using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using static System.Environment;

namespace EuroPlitka_Services
{
    public static class PhotoService 
    {




        public  static async Task<byte[]> FileToByte(IFormFileCollection file) 
        {
           
            string PublicPictureFolder = GetFolderPath(SpecialFolder.CommonPictures);
            string FullPathToDirectory = Path.Combine(PublicPictureFolder + WebConstanta.ImageFolder);
            CheckFolder(FullPathToDirectory);
            string fileName = Guid.NewGuid().ToString();       //greate random guid
            string extension = Path.GetExtension(file[0].FileName); //get extension file which uploaded
            string FullPath = Path.Combine(FullPathToDirectory, fileName + extension);

            using (var filestream = new FileStream(FullPath, FileMode.Create))
            {
                file[0].CopyTo(filestream);
            }
            byte[] contents = await File.ReadAllBytesAsync(FullPath);  //crush to byte
            CheckFolder(FullPathToDirectory);
            return contents;

        }

        public static void CheckFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            else
            {
                var existFile = Directory.GetFiles(path);
                if (existFile.Length > 0)
                {
                    foreach (var item in existFile)
                    {
                        File.Delete(item);
                    }
                }
            }
        }






    }
}
