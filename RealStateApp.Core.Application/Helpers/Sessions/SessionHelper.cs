using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Text;

namespace RealStateApp.Core.Application.Helpers.Sessions
{
    public static class SessionHelper
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            session.SetString(key, JsonConvert.SerializeObject(value, settings));
            
        }

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonConvert.DeserializeObject<T>(value);

        }
    }
}
