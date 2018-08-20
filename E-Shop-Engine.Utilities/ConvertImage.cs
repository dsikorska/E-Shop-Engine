using System.Drawing;
using System.IO;
using System.Web;

namespace E_Shop_Engine.Utilities
{
    public static class ConvertImage
    {
        public static byte[] ToByteArray(Image image)
        {
            ImageConverter _imageConverter = new ImageConverter();
            byte[] byteArray = (byte[])_imageConverter.ConvertTo(image, typeof(byte[]));
            return byteArray;
        }

        public static byte[] ToByteArray(string url)
        {
            return File.ReadAllBytes(HttpContext.Current.Server.MapPath(url));
        }
    }
}
