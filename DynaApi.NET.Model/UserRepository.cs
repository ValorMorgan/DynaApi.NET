using System;
using System.Collections.Generic;
using System.Linq;
using DynaApi.NET.Data.Entities.DoWithYou;
using DynaApi.NET.Data.Mappers;
using DynaApi.NET.Interface.Data;
using DynaApi.NET.Interface.Entity;
using DynaApi.NET.Model.Base;
using DynaApi.NET.Shared.Constants;
using DynaApi.NET.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace DynaApi.NET.Model
{
    public class UserRepository : EntityRepository<User>, IRepository<IUser>
    {
        #region CONSTRUCTORS
        public UserRepository(IEntityDatabaseMapper<IUser> mapper)
            : base(mapper?.MapEntityToContext()) { }

        internal UserRepository(DbContext context, DbSet<User> entities)
            : base(context, entities) { }
        #endregion

        public void Delete(IUser entity) =>
            base.Delete(entity as User);

        public IUser Get(Func<IQueryable<IUser>, IUser> operation) =>
            base.Get(e => operation(e) as User);

        public IEnumerable<IUser> GetMany(Func<IQueryable<IUser>, IEnumerable<IUser>> operation) =>
            base.GetMany(e => operation(e).Cast<User>());

        public void Insert(IUser entity)
        {
            // Check collision with existing data
            if (entity != null && entity.UserID != default)
            {
                var checkEntity = Get(e => e.FirstOrDefault(i => i.UserID == entity.UserID));
                if (checkEntity != null)
                    Update(entity);
            }

            base.Insert(entity as User);
        }

        public void Update(IUser entity) =>
            base.Update(entity as User);

        public new void Dispose()
        {
            Log.Logger.LogEventDebug(LoggerEvents.DISPOSE, LoggerTemplates.DISPOSING, nameof(UserRepository));

            base.Dispose();
        }
    }
}