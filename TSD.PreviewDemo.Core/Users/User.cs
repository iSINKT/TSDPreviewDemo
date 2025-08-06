using System.Collections.Generic;
using TSD.PreviewDemo.Core.Shops;

namespace TSD.PreviewDemo.Core.Users
{
    public class User : Entity
    {
        public string BarCode { get; set; }
        public LoginState LoginState { get; set; }
        public List<Shop> Shops { get; set; }
    }
}