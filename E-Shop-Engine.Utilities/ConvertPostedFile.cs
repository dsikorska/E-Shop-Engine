using System.Web;

namespace E_Shop_Engine.Utilities
{
    public static class ConvertPostedFile
    {
        /// <summary>
        /// Convert image to byte array.
        /// </summary>
        /// <param name="image">Image</param>
        /// <returns>Image as byte array.</returns>
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
    }
}
