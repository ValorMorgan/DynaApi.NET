using DynaApi.NET.Interface.Entity;
using Microsoft.EntityFrameworkCore;

namespace DynaApi.NET.Data.Mappers
{
    public interface IEntityDatabaseMapper<T>
        where T : IBaseEntity
    {
        DbContext MapEntityToContext();
    }
}