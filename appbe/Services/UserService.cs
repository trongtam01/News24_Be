using appbe.Configuration;
using Newtonsoft.Json;
using System.Globalization;

namespace appbe.Services
{
    public class UserService
    {
        public const string JWTKEY = "jwt24";
        private readonly IHttpContextAccessor _context;
        private readonly HttpContext HttpContext;

        public UserService(IHttpContextAccessor context)
        {
            _context = context;
            HttpContext = context.HttpContext;
        }

        // Lấy cart từ Session (danh sách CartItem)
        public List<AuthResult> GetUserItems()
        {

            var session = HttpContext.Session;
            string jsoncart = session.GetString(JWTKEY);
            if (jsoncart != null)
            {
                return JsonConvert.DeserializeObject<List<AuthResult>>(jsoncart);
            }
            return new List<AuthResult>();
        }

        // Xóa cart khỏi session
        public void ClearJWT()
        {
            var session = HttpContext.Session;
            session.Remove(JWTKEY);
        }

        // Lưu Cart (Danh sách CartItem) vào session
        public void SaveJWTSession(List<AuthResult>? ls)
        {
            var session = HttpContext.Session;
            string jsoncart = JsonConvert.SerializeObject(ls, Formatting.Indented,
                new JsonSerializerSettings
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            session.SetString(JWTKEY, jsoncart);
        }
    }
}
