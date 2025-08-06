namespace TSD.PreviewDemo.Common.Errors
{
    public struct Error
    {
        public ErrorType ErrorType;
        public ErrorTypeAction ErrorTypeAction;
    }

    public enum ErrorTypeAction
    {
        None,
        CloseDialogRetry,
        CloseDialogLogOut,
        LogOut,
        Exit,
        ExitRetry,
        Confirm,
        ConfirmYesNo,
        ScheduledRequest,
        RestartRequired
    }

    public enum ErrorType
    {
        None,
        AxReturnServiceError,
        AxSpecialCase,
        ServiceError,
        ServiceErrorResolveConnection,
        ProcessRequestError
    }
}
