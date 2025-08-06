using System.Collections.Generic;
// ReSharper disable TypeParameterCanBeVariant

namespace TSD.PreviewDemo.Core.Interfaces.Repositories
{
    public interface IRepository<T>
    {
        IEnumerable<T> List();
    }
}
