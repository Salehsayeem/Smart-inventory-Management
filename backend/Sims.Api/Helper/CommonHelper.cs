using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Sims.Api.Models;

namespace Sims.Api.Helper
{
    public static class CommonHelper
    {
        public static void ApplyCommonConfigurations(ModelBuilder modelBuilder)
        {
            var ulidConverter = new UlidToStringConverter();

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var properties = entityType.ClrType.GetProperties();

                // Apply default value configuration for CreatedAt
                if (properties.Any(p => p.Name == "CreatedAt"))
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .Property("CreatedAt")
                        .HasDefaultValueSql("now()");
                }

                foreach (var property in properties.Where(p =>
                             p.PropertyType == typeof(Ulid) || p.PropertyType == typeof(Ulid?)))
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .Property(property.Name)
                        .HasConversion(ulidConverter);
                }
            }
        }

        public static Ulid StringToUlidConverter(string userId)
        {
            try
            {
                if (Ulid.TryParse(userId, out Ulid user))
                {
                    return user;
                }
                else
                {
                    throw new Exception("Invalid User ID format.");
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        public class UlidToStringConverter : ValueConverter<Ulid, string>
        {
            public UlidToStringConverter() : base(
                ulid => ulid.ToString(), // Convert Ulid to string
                value => Ulid.Parse(value) // Convert string to Ulid
            )
            {
            }
        }
        public static class StoredProcedureNames
        {
            public static readonly string GetCategoryPagination = "fn_get_categories_pagination";

        }
    }
}
