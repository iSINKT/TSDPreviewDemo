using System.Collections.Generic;
using TSD.PreviewDemo.Core.Users;

namespace TSD.PreviewDemo.Core
{
    public class LoginData : User
    {
        public List<Entity> ListEntities { get; set; } = new List<Entity>();
        public Entity Entity { get; set; }
    }
}
