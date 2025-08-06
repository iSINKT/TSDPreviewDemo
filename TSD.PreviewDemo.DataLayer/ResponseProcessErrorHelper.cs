using System;
using TSD.PreviewDemo.Common.Errors;
using TSD.PreviewDemo.Common.Logging;
using TSD.PreviewDemo.Config;
// ReSharper disable UnusedMember.Local
// ReSharper disable StringLiteralTypo

namespace TSD.PreviewDemo.DataLayer
{
    // ReSharper disable UnusedMember.Global
    public class ResponseProcessErrorHelper(ILogger logger) : IDeviceError
    {
        private readonly ILogger _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        public string ErrorMsg { get; set; }
        public Error DeviceError { get; private set; }

        IDeviceError IDeviceError.GetErrorByCode(string errorCode, string errorMsg)
        {
            switch (errorCode)
            {
                case GlobalConstantValues.CONNECTION_ERROR:
                    ErrorMsg = errorMsg;
                    DeviceError = new Error
                    {
                        ErrorType = ErrorType.ServiceError,
                        ErrorTypeAction = ErrorTypeAction.ExitRetry
                    };
                    break;
                case GlobalConstantValues.SERVICE_ERROR + "_401":
                    ErrorMsg = "Запрос не авторизован";
                    DeviceError = new Error
                    {
                        ErrorType = ErrorType.ServiceError,
                        ErrorTypeAction = ErrorTypeAction.ExitRetry
                    };
                    break;
                case GlobalConstantValues.SERVICE_ERROR + "_408":
                    ErrorMsg = "Превышен лимит ожидания";
                    DeviceError = new Error
                    {
                        ErrorType = ErrorType.ServiceError,
                        ErrorTypeAction = ErrorTypeAction.ExitRetry
                    };
                    break;
                case GlobalConstantValues.SERVICE_ERROR + "_503":
                    ErrorMsg = "Сервер недоступен";
                    DeviceError = new Error
                    {
                        ErrorType = ErrorType.ServiceError,
                        ErrorTypeAction = ErrorTypeAction.ExitRetry
                    };
                    break;
                case GlobalConstantValues.SERVICE_ERROR + "_500":
                    ErrorMsg = "Ошибка обработки запроса на сервере";
                    DeviceError = new Error
                    {
                        ErrorType = ErrorType.ServiceError,
                        ErrorTypeAction = ErrorTypeAction.ExitRetry
                    };
                    break;
                case GlobalConstantValues.SERVICE_ERROR + "_404":
                    ErrorMsg = "Ошибка подключения к серверу. Некорректный адрес или протокол";
                    DeviceError = new Error
                    {
                        ErrorType = ErrorType.ServiceErrorResolveConnection,
                        ErrorTypeAction = ErrorTypeAction.ExitRetry
                    };
                    break;
                case GlobalConstantValues.SERVICE_ERROR + "_2017":
                    ErrorMsg = "Ошибка выполнения запроса";
                    DeviceError = new Error
                    {
                        ErrorType = ErrorType.ServiceError,
                        ErrorTypeAction = ErrorTypeAction.ExitRetry
                    };

                    break;
                case GlobalConstantValues.SERVICE_ERROR + "_2018":
                    ErrorMsg = "Ошибка доступа к серверу";
                    DeviceError = new Error
                    {
                        ErrorType = ErrorType.ServiceError,
                        ErrorTypeAction = ErrorTypeAction.ExitRetry
                    };
                    break;
                case GlobalConstantValues.SERVICE_ERROR + "_2019":
                    ErrorMsg = "Ошибка подключения к серверу";
                    DeviceError = new Error
                    {
                        ErrorType = ErrorType.ServiceError,
                        ErrorTypeAction = ErrorTypeAction.ExitRetry
                    };
                    break;
                case GlobalConstantValues.SERVICE_ERROR + "_4081":
                    ErrorMsg = "Превышен лимит ожидания выполнения запроса";
                    DeviceError = new Error
                    {
                        ErrorType = ErrorType.ServiceError,
                        ErrorTypeAction = ErrorTypeAction.ExitRetry
                    };
                    break;
                case GlobalConstantValues.SERVICE_ERROR + "_2020":
                    ErrorMsg = "Ошибка веб сервиса";
                    DeviceError = new Error
                    {
                        ErrorType = ErrorType.ServiceError,
                        ErrorTypeAction = ErrorTypeAction.ExitRetry
                    };
                    break;
                case GlobalConstantValues.SERVICE_ERROR + "_2021":
                    ErrorMsg = "Ошибка получения данных авторизации DAX";
                    DeviceError = new Error
                    {
                        ErrorType = ErrorType.ServiceError,
                        ErrorTypeAction = ErrorTypeAction.ExitRetry
                    };
                    break;
                case GlobalConstantValues.SERVICE_ERROR + "_2022":
                    ErrorMsg = "Некорректный адрес сервиса или нет подключения к сети";
                    DeviceError = new Error
                    {
                        ErrorType = ErrorType.ServiceErrorResolveConnection,
                        ErrorTypeAction = ErrorTypeAction.ExitRetry
                    };
                    break;
                case "COMMAND_IS_NOT_FOUND":
                    ErrorMsg = Equals(errorMsg, "") ? "Mетод не реализован в ДАХ" : errorMsg;
                    DeviceError = new Error
                    {
                        ErrorType = ErrorType.AxReturnServiceError,
                        ErrorTypeAction = ErrorTypeAction.CloseDialogLogOut
                    };
                    break;
                case "APPLICATION_VERSION_IS_UNDEFINED":
                    ErrorMsg = Equals(errorMsg, "") ? "Версия приложения не определена" : errorMsg;
                    DeviceError = new Error
                    {
                        ErrorType = ErrorType.AxReturnServiceError,
                        ErrorTypeAction = ErrorTypeAction.CloseDialogLogOut
                    };
                    break;
                case "CONFIRMATION_IS_NEEDED":
                    ErrorMsg = errorMsg;
                    DeviceError = new Error
                    {
                        ErrorType = ErrorType.AxSpecialCase,
                        ErrorTypeAction = ErrorTypeAction.Confirm
                    };
                    break;
                case "CONFIRMATION_IS_NEEDED_NO_YES":
                    ErrorMsg = errorMsg;
                    DeviceError = new Error
                    {
                        ErrorType = ErrorType.AxSpecialCase,
                        ErrorTypeAction = ErrorTypeAction.ConfirmYesNo
                    };
                    break;
                case "CONTEXT_IS_BUSY":
                    ErrorMsg = Equals(errorMsg, "") ? "Сеанс обрабатывает предыдущую команду" : errorMsg;
                    DeviceError = new Error
                    {
                        ErrorType = ErrorType.AxReturnServiceError,
                        ErrorTypeAction = ErrorTypeAction.CloseDialogRetry
                    };
                    break;
                case "DESERIALIZATION_ERROR":
                    ErrorMsg = Equals(errorMsg, "") ? "Ошибка распознования данных запроса" : errorMsg;
                    DeviceError = new Error
                    {
                        ErrorType = ErrorType.AxReturnServiceError,
                        ErrorTypeAction = ErrorTypeAction.CloseDialogLogOut
                    };
                    break;
                case "DEVICE_IS_BEING_USED_NOW":
                    ErrorMsg = Equals(errorMsg, "") ? "Устройство занято другим пользователем" : errorMsg;
                    DeviceError = new Error
                    {
                        ErrorType = ErrorType.AxReturnServiceError,
                        ErrorTypeAction = ErrorTypeAction.LogOut
                    };
                    break;
                case "ENTRY_POINT_IS_UNDEFINED":
                    ErrorMsg = Equals(errorMsg, "") ? "Не указан сервис DAX в профиле пользователя " : errorMsg;
                    DeviceError = new Error
                    {
                        ErrorType = ErrorType.AxReturnServiceError,
                        ErrorTypeAction = ErrorTypeAction.LogOut
                    };
                    break;
                case "UNEXPECTED_ERROR":
                    ErrorMsg = Equals(errorMsg, "") ? "Ошибка выполнения запроса" : errorMsg;
                    DeviceError = new Error
                    {
                        ErrorType = ErrorType.AxReturnServiceError,
                        ErrorTypeAction = ErrorTypeAction.CloseDialogLogOut
                    };
                    break;
                case "UNSUPPORTED_FUNCTIONAL_CALL":
                    ErrorMsg = Equals(errorMsg, "") ? "Запрос не применим для данного БП" : errorMsg;
                    DeviceError = new Error
                    {
                        ErrorType = ErrorType.AxReturnServiceError,
                        ErrorTypeAction = ErrorTypeAction.CloseDialogLogOut
                    };
                    break;
                case "USER_CONTEXT_INITIALIZATION_ERROR":
                    ErrorMsg = Equals(errorMsg, "") ? "Ошибка создания сеанса пользвателя" : errorMsg;
                    DeviceError = new Error
                    {
                        ErrorType = ErrorType.AxReturnServiceError,
                        ErrorTypeAction = ErrorTypeAction.CloseDialogLogOut
                    };
                    break;
                case "USER_DEFINITION_ERROR":
                    ErrorMsg = Equals(errorMsg, "") ? "Ошибка опеделения пользователя" : errorMsg;
                    DeviceError = new Error
                    {
                        ErrorType = ErrorType.AxReturnServiceError,
                        ErrorTypeAction = ErrorTypeAction.LogOut
                    };
                    break;
                case "USER_HAS_LOGGED_FROM_OTHER_DEVICE":
                    ErrorMsg = Equals(errorMsg, "") ? "Пользователь активен на другом устройстве" : errorMsg;
                    DeviceError = new Error
                    {
                        ErrorType = ErrorType.AxReturnServiceError,
                        ErrorTypeAction = ErrorTypeAction.LogOut
                    };
                    break;
                case "SESSION_IS_CLOSED":
                    ErrorMsg = Equals(errorMsg, "") ? "Сеанс пользователя завершен" : errorMsg;
                    DeviceError = new Error
                    {
                        ErrorType = ErrorType.AxReturnServiceError,
                        ErrorTypeAction = ErrorTypeAction.Exit
                    };
                    break;
                case "SESSION_IS_NOT_DEFINED":
                    ErrorMsg = Equals(errorMsg, "") ? "Сеанс не задан" : errorMsg;
                    DeviceError = new Error
                    {
                        ErrorType = ErrorType.AxReturnServiceError,
                        ErrorTypeAction = ErrorTypeAction.ExitRetry
                    };
                    break;
                case "VERSION_IS_EMPTY":
                    ErrorMsg = Equals(errorMsg, "") ? "В запросе не указан код версии" : errorMsg;
                    DeviceError = new Error
                    {
                        ErrorType = ErrorType.AxReturnServiceError,
                        ErrorTypeAction = ErrorTypeAction.LogOut
                    };
                    break;
                case "VERSION_IS_OUT_OF_DATE":
                    ErrorMsg = Equals(errorMsg, "") ? "Не актуальная версия приложения" : errorMsg;
                    DeviceError = new Error
                    {
                        ErrorType = ErrorType.AxReturnServiceError,
                        ErrorTypeAction = ErrorTypeAction.LogOut
                    };
                    break;
                case GlobalConstantValues.PROCESS_REQUEST_ERROR:
                    ErrorMsg = "Ошибка выполнения запроса " + errorMsg;
                    DeviceError = new Error
                    {
                        ErrorType = ErrorType.ProcessRequestError,
                        ErrorTypeAction = ErrorTypeAction.CloseDialogRetry
                    };
                    break;
                case GlobalConstantValues.PROCESS_AUTHORIZE_ERROR:
                    ErrorMsg = "Ошибка авторизации " + errorMsg;
                    DeviceError = new Error
                    {
                        ErrorType = ErrorType.ProcessRequestError,
                        ErrorTypeAction = ErrorTypeAction.ExitRetry
                    };
                    break;
                case GlobalConstantValues.INIT_ERROR:
                    ErrorMsg = "Ошибка " + errorMsg;
                    DeviceError = new Error
                    {
                        ErrorType = ErrorType.ProcessRequestError,
                        ErrorTypeAction = ErrorTypeAction.Exit
                    };
                    break;
                case "Task_Scheduled_Request":
                    ErrorMsg = Equals(errorMsg, "") ? "Операция проверки выполнения задания" : errorMsg;
                    DeviceError = new Error
                    {
                        ErrorType = ErrorType.AxSpecialCase,
                        ErrorTypeAction = ErrorTypeAction.ScheduledRequest
                    };
                    break;
                case "VERSION_HASH_IS_OUT_OF_DATE":
                    ErrorMsg = Equals(errorMsg, "") ? "Необходимо обновить приложение." : errorMsg;
                    DeviceError = new Error
                    {
                        ErrorType = ErrorType.AxSpecialCase,
                        ErrorTypeAction = ErrorTypeAction.RestartRequired
                    };
                    break;
                default:
                    ErrorMsg = errorMsg;
                    DeviceError = new Error
                    {
                        ErrorType = ErrorType.ProcessRequestError,
                        ErrorTypeAction = ErrorTypeAction.CloseDialogRetry
                    };
                    break;
            }
            return this;
        }

        void IDeviceError.ShowInfoMessage(string msg)
        {
            throw new NotImplementedException("During code clean, need to be implemented in other module.");
        }

        void IDeviceError.ShowInfoMessage(string msg, string caption)
        {
            throw new 
                NotImplementedException("During code clean, need to be implemented in other module.");
        }

        bool IDeviceError.ShowErrorMessage(IDeviceError error)
        {
            throw new NotImplementedException("During code clean, need to be implemented in other module.");
        }

        bool IDeviceError.ShowErrorMessage(IDeviceError error, string caption)
        {
            throw new NotImplementedException("During code clean, need to be implemented in other module.");
        }

        public void StopBackgroundRequests()
        {
#if ANDROID
            throw new NotImplementedException("Platform depended code: TSD.PreviewDemo.Common.TsdErrorHlp, ResponseProcessErrorHelper, StopBackgroundRequests()");
#endif
#if WindowsCE
            SessionDateTime.StopTimeRefresh();
            var keepAliveSender = RootWorkItem.Services.Get<IKeepAliveSender>();
            keepAliveSender.Stop();
#endif
        }

        void IDeviceError.CleanError()
        {
            ErrorMsg = string.Empty;
            DeviceError = new Error
            {
                ErrorType = ErrorType.None,
                ErrorTypeAction = ErrorTypeAction.None
            };
        }
    }
}
