using System;

namespace DynaApi.NET.Interface.Shared
{
    public interface IStringConverter
    {
        T To<T>();

        dynamic To(Type type);
    }
}