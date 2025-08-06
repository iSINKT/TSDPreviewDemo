using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using TSD.PreviewDemo.Core;

namespace TSD.PreviewDemo.App.ViewData
{
    public interface IActivitySharedDataManager
    {
        TData Get<TData>() where TData : class, IEntity, new();
        void Add<TData>(TData data) where TData : class, IEntity, new();
        void AddList<TData>(IEnumerable<TData> dataList) where TData : class, IEntity;
        // ReSharper disable once UnusedMember.Global
        List<TData> GetList<TData>() where TData : class, IEntity;
    }

    internal class ActivitySharedDataManager : IActivitySharedDataManager
    {
        public TData Get<TData>() where TData : class, IEntity, new()
        {
            if (!_data.ContainsKey(typeof(TData)))
                _data.Add(typeof(TData), new TData());

            var data = _data[typeof(TData)];
            return (TData)data;
        }

        public void Add<TData>(TData data) where TData : class, IEntity, new()
        {
            if (_data.ContainsKey(data.GetType()))
                _data[data.GetType()] = data;
            else
                _data.Add(data.GetType(), data);
        }

        public List<TData> GetList<TData>() where TData : class, IEntity
        {
            if (!_data.ContainsKey(typeof(List<TData>)))
                _data.Add(typeof(List<TData>), new List<TData>());
            var data = _data[typeof(List<TData>)];
            return (List<TData>)data;
        }

        public void AddList<TData>(IEnumerable<TData> dataList) where TData : class, IEntity
        {
            if (dataList == null)
                return;
            var enumerable = dataList as TData[] ?? dataList.ToArray();
            if (!enumerable.Any())
                return;

            var dataType = enumerable[0].GetType();
            var listType = typeof(List<>).MakeGenericType(dataType);
            var list = Activator.CreateInstance(listType);
            var addMethod = listType.GetMethod("Add");
            
            foreach (var data in enumerable)
                addMethod?.Invoke(list, [data]);

            _data.Add(list.GetType(), list);
        }

        private readonly IDictionary<Type, object> _data = new ConcurrentDictionary<Type, object>();
    }
}