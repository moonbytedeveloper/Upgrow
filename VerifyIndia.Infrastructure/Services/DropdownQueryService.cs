using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.Common.Dropdowns;
using VerifyIndia.Domain.Entities;
using VerifyIndia.Application.IServices.Common;

namespace VerifyIndia.Infrastructure.Services
{
    public class DropdownQueryService : IDropdownQueryService
    {
        private readonly AppDbContext _db;

        public DropdownQueryService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<DropdownItemDto>> GetAsync(DropdownRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Key))
                throw new ArgumentException("Dropdown key is required");

            // Validate key against registry
            if (!DropdownRegistry.Definitions.TryGetValue(request.Key, out var def))
                throw new InvalidOperationException(
                    $"Dropdown '{request.Key}' is not registered");

            IQueryable query = GetQueryable(def.EntityType);

            foreach (var rule in def.Filters)
            {
                if (!request.Parents.TryGetValue(rule.ParentKey, out var value))
                    throw new ArgumentException($"Missing parent: {rule.ParentKey}");

                query = ApplyFilter(query, def.EntityType, rule.EntityProperty, value);
            }

            if (def.ApplyActiveFilter)
                query = ApplyFilter(query, def.EntityType, nameof(Master_State.IsActive), "True");

            return await Project(query, def.EntityType).ToListAsync();
        }


        private IQueryable GetQueryable(Type entityType)
        {
            var method = typeof(DropdownQueryService)
                .GetMethod(nameof(GetQueryableInternal), BindingFlags.NonPublic | BindingFlags.Instance)!
                .MakeGenericMethod(entityType);

            return (IQueryable)method.Invoke(this, null)!;
        }

        private IQueryable<TEntity> GetQueryableInternal<TEntity>()
        where TEntity : class
        {
            return _db.Set<TEntity>().AsNoTracking();
        }


        private static IQueryable ApplyFilter(
        IQueryable source,
        Type entityType,
        string propertyName,
        string value)
        {
            var parameter = Expression.Parameter(entityType, "x");
            var property = Expression.Property(parameter, propertyName);

            // Handle nullable types (bool?, int?, etc.)
            var propertyType = Nullable.GetUnderlyingType(property.Type) ?? property.Type;

            // Convert string value to actual property type
            object typedValue = Convert.ChangeType(value, propertyType);

            var constant = Expression.Constant(typedValue, propertyType);

            // If property is nullable, convert constant to nullable
            Expression comparisonValue = property.Type != propertyType
                ? Expression.Convert(constant, property.Type)
                : constant;

            var equalExpression = Expression.Equal(property, comparisonValue);

            var lambda = Expression.Lambda(equalExpression, parameter);

            var whereMethod = typeof(Queryable)
                .GetMethods()
                .First(m => m.Name == "Where" && m.GetParameters().Length == 2)
                .MakeGenericMethod(entityType);

            return (IQueryable)whereMethod.Invoke(null, new object[] { source, lambda })!;
        }

        private static IQueryable<DropdownItemDto> Project(
            IQueryable source,
            Type entityType)
        {
            var param = Expression.Parameter(entityType, "x");

            var body = Expression.MemberInit(
                Expression.New(typeof(DropdownItemDto)),
                Expression.Bind(
                    typeof(DropdownItemDto).GetProperty(nameof(DropdownItemDto.Value))!,
                    Expression.Property(param, "UUID")
                ),
                Expression.Bind(
                    typeof(DropdownItemDto).GetProperty(nameof(DropdownItemDto.Text))!,
                    Expression.Property(param, "Title")
                )
            );

            var selector = Expression.Lambda(body, param);

            var selectMethod = typeof(Queryable)
                .GetMethods()
                .First(m => m.Name == "Select" && m.GetParameters().Length == 2)
                .MakeGenericMethod(entityType, typeof(DropdownItemDto));

            return (IQueryable<DropdownItemDto>)
                selectMethod.Invoke(null, new object[] { source, selector })!;
        }
    }

}
