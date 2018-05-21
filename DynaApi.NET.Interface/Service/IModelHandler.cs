using DynaApi.NET.Interface.Entity;
using DynaApi.NET.Interface.Model;

namespace DynaApi.NET.Interface.Service
{
    public interface IModelHandler<TModel, T1> : IModelRepository<TModel, T1>
        where TModel : IModel<T1>
        where T1 : IBaseEntity
    {
    }

    public interface IModelHandler<TModel, T1, T2> : IModelRepository<TModel, T1, T2>
        where TModel : IModel<T1, T2>
        where T1 : IBaseEntity
        where T2 : IBaseEntity
    {
    }
}