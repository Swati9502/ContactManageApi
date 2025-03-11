

namespace ContactManagementApi.Repository
{
    public class FileHelper
    {
        public static byte[] ConvertToByteArray(IFormFile formFile)
        {
            using (var memoryStream = new MemoryStream())
            {
                formFile.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}