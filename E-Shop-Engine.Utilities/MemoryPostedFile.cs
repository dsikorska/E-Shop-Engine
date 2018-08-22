using System.IO;
using System.Web;

namespace E_Shop_Engine.Utilities
{
    public class MemoryPostedFile : HttpPostedFileBase
    {
        private readonly byte[] _fileBytes;

        public MemoryPostedFile(byte[] fileBytes, string fileName = null)
        {
            if (fileBytes == null)
            {
                return;
            }
            _fileBytes = fileBytes;
            FileName = fileName;
            InputStream = new MemoryStream(fileBytes);
        }

        public override int ContentLength => _fileBytes.Length;

        public override string FileName { get; }

        public override Stream InputStream { get; }
    }
}
