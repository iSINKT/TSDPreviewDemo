using System;
using TSD.PreviewDemo.Core.Users;

namespace TSD.PreviewDemo.Core
{
    public interface IEntity
    {
        string BusinessProcessState { get; set; }
        string BusinessProcessCaption { get; set; }
        Session Session { get; set; }
        string InfoMessageText { get; set; }

    }

    public class Entity : IEntity, ICloneable
    {
        public string BusinessProcessState { get; set; }
        public string BusinessProcessCaption { get; set; }
        public Session Session { get; set; }
        public string InfoMessageText { get; set; }


        public Entity()
        {
            InfoMessageText = "";
        }

        static Entity()
        {
            Empty = new Entity();
        }
        
        public Entity(IEntity entity)
        {
            BusinessProcessCaption = entity.BusinessProcessCaption;
            BusinessProcessState = entity.BusinessProcessState;
            Session = entity.Session;
            InfoMessageText = entity.InfoMessageText;
        }

        public static IEntity Empty { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}