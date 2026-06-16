namespace SocialServices.Controllers
{
    public class GeneralClass
    {
        public static class FormFileHelper
        {
            public static byte[] ToByteArray(IFormFile file)
            {
                if (file == null || file.Length == 0)
                    return null!;

                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    return ms.ToArray();
                }
            }
        }
    }
}
