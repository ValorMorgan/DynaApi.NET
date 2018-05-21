using System;

namespace DynaApi.NET.Interface.Entity
{
    public interface IBaseEntity
    {
        DateTime CreationDate { get; set; }

        DateTime? ModifiedDate { get; set; }
    }
}