using System.Xml;
using System.Xml.Serialization;
using System;
using TSD.PreviewDemo.DataEntities.Base;

// ReSharper disable once CheckNamespace
namespace TSD.PreviewDemo.DataEntities
{
    [XmlType(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/Dynamics.Ax.Application"),
     XmlRoot(Namespace = "http://schemas.datacontract.org/2004/07/Dynamics.Ax.Application", IsNullable = false)]
    // ReSharper disable once InconsistentNaming
    public class MCIResponseBase
    {
        public string BusinessProcessCaption { get; set; }

        public bool DateTimeOfResponseSpecified { get; set; }

        public bool ResponseStatusSpecified { get; set; }

        public string BusinessProcessState { get; set; }

        public object ErrorCode { get; set; }

        public string TextMessage { get; set; }

        public DateTime DateTimeOfResponse { get; set; }

        public XmlNode PackedResponse { get; set; }

        public string PackedResponseStr { get; set; }

        public MCIResponseStatus ResponseStatus { get; set; }

        public string ServiceMethod { get; set; }

        public string SessionId { get; set; }

        public string AppVersionNum { get; set; }

        public bool Error { get; set; }
        public string Message { get; set; }

        #region methods
        public virtual void Clear()
        {
            PackedResponseStr = "";
            Error = false;
            Message = "";
        }
        public virtual MCIResponseBase SetError(string msg, bool errFlag)
        {
            Clear();
            Message = msg;
            Error = errFlag;
            return this;
        }

        #endregion
    }    
}
