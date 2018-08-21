using System.Web;

namespace E_Shop_Engine.Utilities
{
    public static class ConvertPostedFile
    {
        public static byte[] ToByteArray(HttpPostedFileBase image)
        {
            return new byte[image.ContentLength];
        }

        public static HttpPostedFileBase ToHttpPostedFileBase(byte[] array)
        {
            return new MemoryPostedFile(array);
        }
    }
}
