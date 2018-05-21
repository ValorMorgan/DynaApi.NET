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
    public class UserProfileRepository : EntityRepository<UserProfile>, IRepository<IUserProfile>
    {
        #region CONSTRUCTORS
        public UserProfileRepository(IEntityDatabaseMapper<IUserProfile> mapper)
            : base(mapper?.MapEntityToContext()) { }

        internal UserProfileRepository(DbContext context, DbSet<UserProfile> entities)
            : base(context, entities) { }
        #endregion

        public void Delete(IUserProfile entity) =>
            base.Delete(entity as UserProfile);

        public IUserProfile Get(Func<IQueryable<IUserProfile>, IUserProfile> operation) =>
            base.Get(e => operation(e) as UserProfile);

        public IEnumerable<IUserProfile> GetMany(Func<IQueryable<IUserProfile>, IEnumerable<IUserProfile>> operation) =>
            base.GetMany(e => operation(e).Cast<UserProfile>());

        public void Insert(IUserProfile entity)
        {
            // Check collision with existing data
            if (entity != null && entity.UserProfileID != default)
            {
                var checkEntity = Get(e => e.FirstOrDefault(i => i.UserProfileID == entity.UserProfileID));
                if (checkEntity != null)
                    Update(entity);
            }

            base.Insert(entity as UserProfile);
        }

        public void Update(IUserProfile entity) =>
            base.Update(entity as UserProfile);

        public new void Dispose()
        {
            Log.Logger.LogEventDebug(LoggerEvents.DISPOSE, LoggerTemplates.DISPOSING, nameof(UserProfileRepository));

            base.Dispose();
        }
    }
}