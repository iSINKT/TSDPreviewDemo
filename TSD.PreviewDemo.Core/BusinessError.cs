using System;

// ReSharper disable UnusedMember.Global
namespace TSD.PreviewDemo.Core
{
    public class BusinessError : Exception
    {
        public BusinessError(string code,string message, string businessProcessState, string businessProcessCaption) : base(message)
        {
            Code = code;
            BusinessProcessCaption = businessProcessCaption;
            BusinessProcessState = businessProcessState;
        }

        public string BusinessProcessState { get; set; }
        public string BusinessProcessCaption { get; set; }
        public string Code { get; }
        public Exception PackedException { get; set; }
        public static BusinessError Ok { get; } = new BusinessError("OK", string.Empty, String.Empty, string.Empty);
    }
}