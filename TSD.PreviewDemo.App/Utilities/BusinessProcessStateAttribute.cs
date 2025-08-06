using System;

namespace TSD.PreviewDemo.App.Utilities
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class BusinessProcessStateAttribute : Attribute
    {
        public string ProcessState { get; set; }
        public bool SaveData { get; set; }
        public string DataKey { get; set; }

        public BusinessProcessStateAttribute(string processState)
        {
            ProcessState = processState;
            SaveData = false;
        }

        public BusinessProcessStateAttribute(string processState, bool saveData, string dataKey)
        {
            ProcessState = processState;
            SaveData = saveData;
            if (saveData)
            {
                DataKey = dataKey;
            }
        }
    }
}