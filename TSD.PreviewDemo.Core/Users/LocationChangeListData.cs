using System.Collections.Generic;

namespace TSD.PreviewDemo.Core.Users
{
    public class LocationChangeListData : Entity
    {
        public List<ChangeListDataLocationData> LocationData { get; set; }
    }

    public class ChangeListDataLocationData
    {
        public string LocationId { get; set; }
        public string Name { get; set; }
    }
}
