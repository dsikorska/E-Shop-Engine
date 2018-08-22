using System.Web;

namespace E_Shop_Engine.Utilities
{
    public static class ConvertPostedFile
    {
        public static byte[] ToByteArray(HttpPostedFileBase image)
        {
            if (image == null)
            {
                return null;
            }
            byte[] img = new byte[image.ContentLength];
            image.InputStream.Read(img, 0, image.ContentLength);
            return img;
        }

        public static HttpPostedFileBase ToHttpPostedFileBase(byte[] array)
        {
            if (array == null)
            {
                return null;
            }
            return new MemoryPostedFile(array);
        }
    }
}
