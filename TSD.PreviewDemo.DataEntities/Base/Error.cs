namespace TSD.PreviewDemo.DataEntities.Base
{
    // ReSharper disable UnusedMember.Global
    public class Error
    {
        public Error(string code, string message)
        {
            Code = code;
            Message = message;
        }
        public string Code { get; }
        public string Message { get; }

        public static Error Empty { get; } = new Error(string.Empty, string.Empty);
        public static Error Ok { get; } = new Error("OK", string.Empty);
    }
}