using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Data.Entities;
using TechStore.Model.DTOs.Brand;

namespace TechStore.Service.Mappers
{
    public static class BrandMappers
    {
        public static BrandResponseModel ToBrandResponseModel(this Brand brand)
        {
            return new BrandResponseModel
            {
                BrandId = brand.PublicId,
                Name = brand.Name,
                Description = brand.Description,
                Slug = brand.Slug,
                IconImageUrl = brand.IconImageUrl
            };
        }

        public static List<BrandResponseModel> ToListBrandResponseModels(this List<Brand> brands)
        {
            var models = new List<BrandResponseModel>();

            foreach (var brand in brands)
            {
                models.Add(ToBrandResponseModel(brand));
            }

            return models;
        }
    }
}
