using Microsoft.EntityFrameworkCore;

namespace DynaApi.NET.Data.Factories
{
    public interface IDbContextOptionsFactory<T> where T : DbContext
    {
        DbContextOptionsBuilder<T> GetDbContextOptionsBuilder(string connectionString);
        DbContextOptions<T> GetOptions(string connectionString);
    }
}