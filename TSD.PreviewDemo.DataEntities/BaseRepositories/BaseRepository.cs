using System.Collections.Generic;
using AutoMapper;
using TSD.PreviewDemo.Core.Interfaces.Repositories;

namespace TSD.PreviewDemo.DataEntities.BaseRepositories
{
    public abstract class BaseRepository<T> : IRepository<T> where T : class
    {
        protected Mapper Mapper;

        protected BaseRepository(Mapper mapper)
        {
            Mapper = mapper;
        }

        public abstract IEnumerable<T> List();
    }
}
