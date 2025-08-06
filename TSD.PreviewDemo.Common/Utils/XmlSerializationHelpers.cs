using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using TSD.PreviewDemo.Common.Logging;

namespace TSD.PreviewDemo.Common.Utils
{
    public static class XmlSerializationHelpers
    {
        private static ILogger _logger;//DependencyResolver.ResolveLogger();

        public static void Init(ILogger logger)
        {
            _logger = logger;
            _logger.Info("XmlSerializationHelpers инициализирован.");
        }

        public static string Serialize<T>(T value)
        {
            if (value == null)
            {
                return string.Empty;
            }
            try
            {
                var xmlSerializer = new XmlSerializer(value.GetType());

                using (var textWriter = new StringWriter())
                {
                    xmlSerializer.Serialize(textWriter, value);
                    return textWriter.ToString().Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "");
                }

            }
            catch (Exception ex)
            {   
                _logger.Error("XmlSerializationHelpers->Serialize<T>->Error: {0}", ex.Message);
                throw new Exception("Utils: Ошибка десериализации");
            }
        }

        public static MemoryStream GenerateStreamFromString(string value)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(value ?? ""));
        }

        public static T LoadData<T>(string data)
        {
            try
            {
                using (MemoryStream sr = GenerateStreamFromString(data))
                {
                    var xmlSerializer = new XmlSerializer(typeof (T));
                    return (T) xmlSerializer.Deserialize(sr);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("XmlSerializationHelpers->T LoadData<T>->Error: {0}", ex.Message);
                _logger.Error("XmlSerializationHelpers->T LoadData<T>->type: {0}", typeof(T).FullName);
                _logger.Error("XmlSerializationHelpers->T LoadData<T>->contract xml: {0}", data);
                throw new Exception("Utils: Ошибка десериализации");
            }
        }

        public static object LoadData(Type dataType, string data)
        {
            try
            {
                using (MemoryStream sr = GenerateStreamFromString(data))
                {
                    var xmlSerializer = new XmlSerializer(dataType);
                    return xmlSerializer.Deserialize(sr);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("XmlSerializationHelpers->T LoadData<T>->Error: {0}", ex.Message);
                _logger.Error("XmlSerializationHelpers->T LoadData<T>->type: {0}", dataType.FullName);
                _logger.Error("XmlSerializationHelpers->T LoadData<T>->contract xml: {0}", data);
                throw new Exception("Utils: Ошибка десериализации");
            }
        }
    }
}
