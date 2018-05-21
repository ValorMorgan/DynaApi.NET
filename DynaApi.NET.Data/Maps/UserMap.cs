using System;
using DynaApi.NET.Data.Entities.DoWithYou;
using DynaApi.NET.Shared;
using DynaApi.NET.Shared.Constants;
using DynaApi.NET.Shared.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Serilog;

namespace DynaApi.NET.Data.Maps
{
    public static class UserMap
    {
        public static void Map(EntityTypeBuilder<User> builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder), $"{nameof(EntityTypeBuilder)} cannot be NULL.");
            
            Log.Logger.LogEventVerbose(LoggerEvents.DATA, LoggerTemplates.DATA_MAP, nameof(User), nameof(EntityTypeBuilder));

            MapKeys(builder);
            MapProperties(builder);
            MapRelationships(builder);
        }

        #region PRIVATE
        private static void MapKeys(EntityTypeBuilder<User> builder)
        {
            Log.Logger.LogEventVerbose(LoggerEvents.DATA, LoggerTemplates.DATA_MAP_KEYS, nameof(User), nameof(EntityTypeBuilder));

            builder.HasKey(e => e.UserID);
        }

        private static void MapProperties(EntityTypeBuilder<User> builder)
        {
            Log.Logger.LogEventVerbose(LoggerEvents.DATA, LoggerTemplates.DATA_MAP_PROPERTIES, nameof(User), nameof(EntityTypeBuilder));

            builder.Property(e => e.Email).IsRequired();
            builder.Property(e => e.Password).IsRequired();
            builder.Property(e => e.Username).IsRequired();
        }

        private static void MapRelationships(EntityTypeBuilder<User> builder)
        {
            Log.Logger.LogEventVerbose(LoggerEvents.DATA, LoggerTemplates.DATA_MAP_RELATIONSHIPS, nameof(User), nameof(EntityTypeBuilder));

            builder.HasOne(e => e.UserProfile)
                .WithOne(e => e.User)
                .HasForeignKey<UserProfile>(e => e.UserID);
        }
        #endregion
    }
}