﻿namespace DynaApi.NET.Interface.Entity
{
    public interface IUserProfile : IBaseEntity
    {
        string Address1 { get; set; }

        string Address2 { get; set; }

        string City { get; set; }

        string FirstName { get; set; }

        string LastName { get; set; }

        string MiddleName { get; set; }

        string Phone { get; set; }

        string State { get; set; }

        long UserID { get; set; }

        long UserProfileID { get; set; }

        string ZipCode { get; set; }
    }
}