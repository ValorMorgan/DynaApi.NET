using System;
using DynaApi.NET.Data.Entities.DoWithYou;
using DynaApi.NET.Shared;
using DynaApi.NET.Shared.Constants;
using DynaApi.NET.Shared.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Serilog;

namespace DynaApi.NET.Data.Maps
{
    public static class UserProfileMap
    {
        public static void Map(EntityTypeBuilder<UserProfile> builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder), $"{nameof(EntityTypeBuilder)} cannot be NULL.");
            
            Log.Logger.LogEventVerbose(LoggerEvents.DATA, LoggerTemplates.DATA_MAP, nameof(UserProfile), nameof(EntityTypeBuilder));

            MapKeys(builder);
            MapProperties(builder);
            MapRelationships(builder);
        }

        #region PRIVATE
        private static void MapKeys(EntityTypeBuilder<UserProfile> builder)
        {
            Log.Logger.LogEventVerbose(LoggerEvents.DATA, LoggerTemplates.DATA_MAP_KEYS, nameof(UserProfile), nameof(EntityTypeBuilder));

            builder.HasKey(e => e.UserProfileID);
        }

        private static void MapProperties(EntityTypeBuilder<UserProfile> builder)
        {
            Log.Logger.LogEventVerbose(LoggerEvents.DATA, LoggerTemplates.DATA_MAP_PROPERTIES, nameof(UserProfile), nameof(EntityTypeBuilder));

            builder.Property(e => e.FirstName).IsRequired();
            builder.Property(e => e.LastName).IsRequired();
            builder.Property(e => e.Address1).IsRequired();
            builder.Property(e => e.City).IsRequired();
            builder.Property(e => e.State).IsRequired();
            builder.Property(e => e.ZipCode).IsRequired();
        }

        private static void MapRelationships(EntityTypeBuilder<UserProfile> builder)
        {
            Log.Logger.LogEventVerbose(LoggerEvents.DATA, LoggerTemplates.DATA_MAP_RELATIONSHIPS, nameof(UserProfile), nameof(EntityTypeBuilder));

            builder.HasOne(e => e.User)
                .WithOne(e => e.UserProfile)
                .HasForeignKey<User>(e => e.UserID);
        }
        #endregion
    }
}