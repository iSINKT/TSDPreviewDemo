using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Reflection;
using TSD.PreviewDemo.Common.Platform;
// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

namespace TSD.PreviewDemo.Config
{
    public class GlobalConstantValues
    {
        private GlobalConstantValues()
        {

        }

        public const string ApplicationName = "TSD.PreviewDemo";
        public const int MaxLogDays = 14;

        // ReSharper disable once UnusedMember.Local
        private static IPAddress _deviceIpAddress = IPAddress.Any;

        public static IPAddress DeviceIpAddress => throw new NotImplementedException("During code clean, need to be implemented in other module.");

        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        public static IDeviceIdentityProvider DeviceMacAddress { get; private set; }

        public static DateTime DateTimeAxNull = new DateTime(1900, 1, 1, 0, 0, 0);

        #region Colors

        public static Color ColorGray = Color.FromArgb(230, 230, 230);
        public static Color ColorWhite = Color.White;
        public static Color FormBackColor = Color.White;
        public static Color LabelBackColor = Color.White;
        
        //public static Font LabelFont = new Font("Tahoma", 9, FontStyle.Regular);
        public static Color ButtonBackColor = Color.FromArgb(206, 223, 239);
        // public static Font ButtonFont = new Font("Tahoma", 9, FontStyle.Bold);
        // public static Font ButtonFontReg8 = new Font("Tahoma", 8, FontStyle.Regular);
        // public static Font ButtonFontReg9 = new Font("Tahoma", 9, FontStyle.Regular);
        // public static Font DataGridFont = new Font("Tahoma", 8, FontStyle.Regular);
        public static Color GridSelectBackColor = Color.FromArgb(128, 128, 255);

        public static int HeaderColorInt { get; set; } = 5263440;

        public static Color HeaderColor => Color.FromArgb(HeaderColorInt);

        #endregion


        #region Device info: Screen dimensions, Scanner, OS


        /*public static string DeviceOs = Platform.IsWindowsMobile ? "WinMobile" : "WinCE";
        public static int ScreenBoundsHeight = Screen.PrimaryScreen.Bounds.Height;
        public static int ScreenBoundsWidth = Screen.PrimaryScreen.Bounds.Width;
        public static int ScreenWorkingAreaHeight = Screen.PrimaryScreen.WorkingArea.Height;
        public static int ScreenWorkingAreaWidth = Screen.PrimaryScreen.WorkingArea.Width;*/

        #endregion


        // ReSharper disable once UnassignedGetOnlyAutoProperty
        // ReSharper disable once UnusedMember.Global
        public static bool IsEmulator
        {
            get;
        }

        public static string DaxServiceUrl { get; set; }

        /// <summary>
        /// кнопки используемые для навигации
        /// вверх
        /// ввниз
        /// </summary>
        public static int[] NavigationUpKeyValue { get; set; } = {38};

        public static int[] NavigationDownKeyValue { get; set; } = {40};

     

        #region error messages

        public const string ErrorLogPathToDelete = "/Program Files/Log/";
        public const string ErrorLogFileBaseName = "tsdaplog.txt";
        private const string ErrorLogDir = "/Program Files/TSD/TsdLog/";

        public static string ErrorLogPath(string sessionId)
        {
            if (string.IsNullOrEmpty(sessionId))
                return ErrorLogDir + ErrorLogFileBaseName;
            return ErrorLogDir + sessionId + ErrorLogFileBaseName;
        }

        public static string MsgBeginingOfWork = "Подготовка к работе!";
        public static string MsgAttention = "Внимание";
        public static string NoSelect = "Нет выбора";
        public static string NoData = "Нет данных!";
        public static string ErrSystem = "Ошибка системы!"; //"Неверное числовое значение!"
        public static string ErrNoSelect = "Не выбрано значение!";
        public static string ErrNumberFormat = "Неверное числовое значение!";
        public static string ErrNoValueEntered = "Введите значение!";

        public const string NoDataReturned = "Нет данных для отображения";
        public const string ErrMsgNoWebSrvProxies = "Не заданны прокси адреса сервиса.";
        public const string ErrMsgNoWebSrvProxyAddres = "Некорректный Url веб сервиса.";
        public const string ErrMsgNoConnectionMsg = "Нет подключения к сети.";
        public const string ErrMsgNoXmlElemenMsg = "Контракт не имеет необходимый xml элемент";
        public const string ErrMsgNoServiceAddressesMsg = "Не определены адреса сервисов. Необходимо перезапустить приложение!";
        public const string ErrMsgNoErrorCode = ""; //"Ошибка выполнения запроса";
        public const string ErrMsgIncorrectLauncherPath = "Не найден исполнительный файл запуска приложения. Приложение будет закрыто.";
        public const string ErrMsgNoReasonSelected = "Не выбрана причина отмены";



        //  error code that returned from AH when user 
        //  has to confirm prior send request data
        //Other error codes
        public const string PROCESS_REQUEST_ERROR = "PROCESS_REQUEST_ERROR";
        public const string PROCESS_AUTHORIZE_ERROR = "PROCESS_AUTHORIZE_ERROR";
        public const string SERVICE_ERROR = "SERVICE_ERROR";
        public const string CONNECTION_ERROR = "CONNECTION_ERROR";
        public const string INIT_ERROR = "INIT_ERROR";

        public const string LogOutState = "Сеанс.Конец";

        #endregion

        #region intervals

        public static int WebRequestTimeOut = 10000;
        public static int WebServiceRequestTimeOut = 600000;
        public static int FormDataRefreshInterval = 60000;
        public static int KeepAliveInterval = 60000;

        #endregion

        #region controls naming

        public const string BtnRetry = "Повторить";
        public const string BtnExit = "Выход";
        public const string BtnClose = "Закрыть";
        public const string BtnConfirm = "Подтвердить";
        public const string BtnCancel = "Отменить";
        public const string BtnOk = "OK";
        public const string BtnReport = "Сообщить об ошибке";
        public const string BtnRestart = "Обновить";
        public const string BtnSchedule = "Отложить";

        #endregion

        #region xml elements

        public const string XmlIsConfirmed = "IsConfirmed";

        #endregion

        #region enums / enums like

        
        public enum WebSrvProxyChangePoints
        {
            None,
            UserInit,
            PreAuthorize,
            PostAuthorize
        }
         public enum NoYes
        {
            None,
            No,
            Yes,
        }
         


        #endregion


        private static IEnumerable<string> _versionInfo;

        public static IEnumerable<string> TsdAppVersionInfo
        {
            get
            {
                if (_versionInfo != null)
                    return _versionInfo;
                var versionInfo = new List<string>
                {
                    Assembly.GetExecutingAssembly().GetName().Name,
                    Assembly.GetExecutingAssembly().GetName().Version.ToString()
                };
                _versionInfo = versionInfo;
                return _versionInfo;
            }
            set => _versionInfo = value;
        }

        public static void TsdAppVersionInfoLoad(IEnumerable<string> versionInfo)
        {
            _versionInfo = versionInfo;
        }
    }
}
