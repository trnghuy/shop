using Microsoft.EntityFrameworkCore;
using Shop.Models;

namespace Shop.Repository
{
    public class SeedData
    {
        public static void SeedingData(DataContext _context)
        {
            //_context.Database.Migrate();
            //if (!_context.Products.Any())
            //{
            //    _context.Categories.AddRange(
            //        new CategoryModel { Name = "Kem Chống Nắng", Slug = "Kem-Chống-Nắng", Description = "", Status = 1 },
            //        new CategoryModel { Name = "Kem Dưỡng Da", Slug = "Kem-Dưỡng-Da", Description = "", Status = 1 },
            //        new CategoryModel { Name = "Kem Trị Mụn", Slug = "Kem-Trị-Mụn", Description = "", Status = 1 },
            //        new CategoryModel { Name = "Kem Trị Thâm Nám", Slug = "Kem-Trị-Thâm-Nám", Description = "", Status = 1 },
            //        new CategoryModel { Name = "Sữa Rửa Mặt", Slug = "Sữa-Rửa-Mặt", Description = "", Status = 1 },
            //        new CategoryModel { Name = "Sữa Tắm", Slug = "Sữa-Tắm", Description = "", Status = 1 }
            //    );
            //    _context.Brands.AddRange(
            //        new BrandModel { Name = "REDWIN", Slug = "REDWIN", Description = "", Status = 1 },
            //        new BrandModel { Name = "Innisfree", Slug = "Innisfree", Description = "", Status = 1 },
            //        new BrandModel { Name = "3CE", Slug = "3CE", Description = "", Status = 1 }
            //    );

            //    //_context.Products.AddRange(
            //    //    new ProductModel { Name = "Sữa tắm Sandras Shower Gel", Slug = "suatamsandrasshowergel", Description = "", Image = "suaTamSandrasShowerGel.jpg", Category = suatam, Brand = redwin, Price = 180000},
            //    //    new ProductModel { Name = "Kem Dưỡng Trắng Da Ngày vs Đêm", Slug = "kemduongtrangdangayvadem", Description = "", Image = "kemDuongTrangDaNgayVaDem.jpg", Category = kemduongda, Brand = redwin, Price = 265000}
            //    //);
            //    _context.SaveChanges();
            //}
        }
    }
}
