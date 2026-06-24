

using Microsoft.EntityFrameworkCore;
using WebProjectAPI.Models;

namespace WebProjectAPI.Data.Seed
{
    public class PermissionSeeder : IDataSeeder
    {
        public int Order => 3;

        public async Task SeedAsync(AppDbContext db, IServiceProvider sp)
        {
            if (await db.Permissions.AnyAsync()) return;

            var permissions = new List<Permission>
            {
                    // USER
                    new Permission { Name = "USER_CREATE", GroupName = "USER" },
                    new Permission { Name = "USER_VIEW", GroupName = "USER" },
                    new Permission { Name = "USER_EDIT", GroupName = "USER" },
                    new Permission { Name = "USER_DELETE", GroupName = "USER" },
                    new Permission { Name = "USER_STATUS", GroupName = "USER" },

                    // ROLE
                    new Permission { Name = "ROLE_CREATE", GroupName = "ROLE" },
                    new Permission { Name = "ROLE_VIEW", GroupName = "ROLE" },
                    new Permission { Name = "ROLE_EDIT", GroupName = "ROLE" },
                    new Permission { Name = "ROLE_DELETE", GroupName = "ROLE" },
                    new Permission { Name = "ROLE_STATUS", GroupName = "ROLE" },

                    // CATEGORY
                    new Permission { Name = "CATEGORY_CREATE", GroupName = "CATEGORY" },
                    new Permission { Name = "CATEGORY_VIEW", GroupName = "CATEGORY" },
                    new Permission { Name = "CATEGORY_EDIT", GroupName = "CATEGORY" },
                    new Permission { Name = "CATEGORY_DELETE", GroupName = "CATEGORY" },
                    new Permission { Name = "CATEGORY_STATUS", GroupName = "CATEGORY" },

                    // SUBCATEGORY
                    new Permission { Name = "SUBCATEGORY_CREATE", GroupName = "SUBCATEGORY" },
                    new Permission { Name = "SUBCATEGORY_VIEW", GroupName = "SUBCATEGORY" },
                    new Permission { Name = "SUBCATEGORY_EDIT", GroupName = "SUBCATEGORY" },
                    new Permission { Name = "SUBCATEGORY_DELETE", GroupName = "SUBCATEGORY" },
                    new Permission { Name = "SUBCATEGORY_STATUS", GroupName = "SUBCATEGORY" },

                    // BRAND
                    new Permission { Name = "BRAND_CREATE", GroupName = "BRAND" },
                    new Permission { Name = "BRAND_VIEW", GroupName = "BRAND" },
                    new Permission { Name = "BRAND_EDIT", GroupName = "BRAND" },
                    new Permission { Name = "BRAND_DELETE", GroupName = "BRAND" },
                    new Permission { Name = "BRAND_STATUS", GroupName = "BRAND" },

                    // PRODUCT
                    new Permission { Name = "PRODUCT_CREATE", GroupName = "PRODUCT" },
                    new Permission { Name = "PRODUCT_VIEW", GroupName = "PRODUCT" },
                    new Permission { Name = "PRODUCT_EDIT", GroupName = "PRODUCT" },
                    new Permission { Name = "PRODUCT_DELETE", GroupName = "PRODUCT" },
                    new Permission { Name = "PRODUCT_STATUS", GroupName = "PRODUCT" },

                    // PRODUCT IMAGE
                    new Permission { Name = "PRODUCT_IMAGE_CREATE", GroupName = "PRODUCT_IMAGE" },
                    new Permission { Name = "PRODUCT_IMAGE_VIEW", GroupName = "PRODUCT_IMAGE" },
                    new Permission { Name = "PRODUCT_IMAGE_EDIT", GroupName = "PRODUCT_IMAGE" },
                    new Permission { Name = "PRODUCT_IMAGE_DELETE", GroupName = "PRODUCT_IMAGE" },
                    new Permission { Name = "PRODUCT_IMAGE_STATUS", GroupName = "PRODUCT_IMAGE" },
                    // PRODUCT VARIANT
                    new Permission { Name = "PRODUCT_VARIANT_CREATE", GroupName = "PRODUCT_VARIANT" },
                    new Permission { Name = "PRODUCT_VARIANT_VIEW", GroupName = "PRODUCT_VARIANT" },
                    new Permission { Name = "PRODUCT_VARIANT_EDIT", GroupName = "PRODUCT_VARIANT" },
                    new Permission { Name = "PRODUCT_VARIANT_DELETE", GroupName = "PRODUCT_VARIANT" },
                    new Permission { Name = "PRODUCT_VARIANT_STATUS", GroupName = "PRODUCT_VARIANT" },

                    // COLOR
                    new Permission { Name = "COLOR_CREATE", GroupName = "COLOR" },
                    new Permission { Name = "COLOR_VIEW", GroupName = "COLOR" },
                    new Permission { Name = "COLOR_EDIT", GroupName = "COLOR" },
                    new Permission { Name = "COLOR_DELETE", GroupName = "COLOR" },
                    new Permission { Name = "COLOR_STATUS", GroupName = "COLOR" },

                    // SIZE
                    new Permission { Name = "SIZE_CREATE", GroupName = "SIZE" },
                    new Permission { Name = "SIZE_VIEW", GroupName = "SIZE" },
                    new Permission { Name = "SIZE_EDIT", GroupName = "SIZE" },
                    new Permission { Name = "SIZE_DELETE", GroupName = "SIZE" },
                    new Permission { Name = "SIZE_STATUS", GroupName = "SIZE" },
                        // ORDER
                    new Permission { Name = "ORDER_VIEW", GroupName = "ORDER" },
                    new Permission { Name = "ORDER_EDIT", GroupName = "ORDER" },
                    new Permission { Name = "ORDER_DELETE", GroupName = "ORDER" },
                    new Permission { Name = "ORDER_STATUS", GroupName = "ORDER" },

                    // COUPON
                    new Permission { Name = "COUPON_CREATE", GroupName = "COUPON" },
                    new Permission { Name = "COUPON_VIEW", GroupName = "COUPON" },
                    new Permission { Name = "COUPON_EDIT", GroupName = "COUPON" },
                    new Permission { Name = "COUPON_DELETE", GroupName = "COUPON" },
                    new Permission { Name = "COUPON_STATUS", GroupName = "COUPON" },

                    // BANNER
                    new Permission { Name = "BANNER_CREATE", GroupName = "BANNER" },
                    new Permission { Name = "BANNER_VIEW", GroupName = "BANNER" },
                    new Permission { Name = "BANNER_EDIT", GroupName = "BANNER" },
                    new Permission { Name = "BANNER_DELETE", GroupName = "BANNER" },
                    new Permission { Name = "BANNER_STATUS", GroupName = "BANNER" },

                    // VENDOR
                    new Permission { Name = "VENDOR_CREATE", GroupName = "VENDOR" },
                    new Permission { Name = "VENDOR_VIEW", GroupName = "VENDOR" },
                    new Permission { Name = "VENDOR_EDIT", GroupName = "VENDOR" },
                    new Permission { Name = "VENDOR_DELETE", GroupName = "VENDOR" },
                    new Permission { Name = "VENDOR_STATUS", GroupName = "VENDOR" },
            };


            foreach (var permission in permissions)
            {
                if (!await db.Permissions.AnyAsync(p => p.Name == permission.Name))
                {
                    db.Permissions.Add(permission);
                }
            }

            await db.SaveChangesAsync();

            
        }
    }
}