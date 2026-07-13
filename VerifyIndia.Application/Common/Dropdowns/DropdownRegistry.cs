using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Upgrow.Domain.Entities;
using Upgrow.Domain.Entities.Auth;

namespace Upgrow.Application.Common.Dropdowns
{
    public static class DropdownRegistry
    {
        public static readonly IReadOnlyDictionary<string, DropdownDefinition> Definitions
                = new Dictionary<string, DropdownDefinition>
                {
                    [DropdownKey.Country] = new()
                    {
                        Key = DropdownKey.Country,
                        EntityType = typeof(Master_Country)

                    },

                    [DropdownKey.State] = new()
                    {
                        Key = DropdownKey.State,
                        EntityType = typeof(Master_State),
                        Filters =
                        [
                            new FilterRule
                        {
                            ParentKey = DropdownKey.Country,
                            EntityProperty = nameof(Master_State.CountryUUID)
                        }
                        ]
                    },

                    [DropdownKey.City] = new()
                    {
                        Key = DropdownKey.City,
                        EntityType = typeof(Master_City),
                        Filters =
                        [
                            new FilterRule
                        {
                            ParentKey = DropdownKey.State,
                            EntityProperty = nameof(Master_City.StateUUID)
                        }
                        ]
                    },
                    [DropdownKey.TenantId] = new()
                    {
                        Key = DropdownKey.TenantId,
                        EntityType = typeof(Tenant)

                    },
                    [DropdownKey.PageTitle] = new()
                    {
                        Key = DropdownKey.PageTitle,
                        EntityType = typeof(WL_MasterCMS),
                        Filters =
                        [
                            new FilterRule
                        {
                            ParentKey = DropdownKey.TenantId,
                            EntityProperty = nameof(WL_MasterCMS.TenantId)
                        }
                        ]
                    },

                    [DropdownKey.Category] = new DropdownDefinition
                    {
                        Key = DropdownKey.Category,
                        EntityType = typeof(Master_FAQCategory)
                    },

                    [DropdownKey.SubCategory] = new DropdownDefinition
                    {
                        Key = DropdownKey.SubCategory,
                        EntityType = typeof(Master_FAQSubCategory),
                        Filters = new List<FilterRule>
                        {
                            // when requesting SubCategory dropdown, caller can provide parent Category UUID
                            new FilterRule
                            {
                                ParentKey = DropdownKey.Category,
                                EntityProperty = nameof(Master_FAQSubCategory.FAQCategoryUUID)
                            }
                        }
                    },

                    [DropdownKey.ApiCategory] = new DropdownDefinition
                    {
                        Key = DropdownKey.ApiCategory,
                        EntityType = typeof(Api_Category)
                    },

                    [DropdownKey.ApiXCategory] = new DropdownDefinition
                    {
                        Key = DropdownKey.ApiXCategory,
                        EntityType = typeof(ApiXCategory),
                        Filters = new List<FilterRule>
                        {
                            new FilterRule
                            {
                                ParentKey = DropdownKey.ApiCategory,
                                EntityProperty = nameof(ApiXCategory.CategoryUUID)
                            }
                        }
                    },


                };
    }
}


