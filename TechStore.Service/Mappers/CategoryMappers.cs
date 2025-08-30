using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Data.Entities;
using TechStore.Model.DTOs.Brand;
using TechStore.Model.DTOs.Category;

namespace TechStore.Service.Mappers
{
    public static class CategoryMappers
    {
        public static CategoryResponseModel ToCategoryResponseModel(this Category category)
        {
            return new CategoryResponseModel
            {
                CategoryId = category.PublicId,
                Name = category.Name,
                Description = category.Description,
                Slug = category.Slug,
                IconImageUrl = category.IconImageUrl
            };
        }

        public static List<CategoryResponseModel> ToListCategoryResponseModels(this List<Category> categories)
        {
            var models = new List<CategoryResponseModel>();

            foreach (var category in categories)
            {
                models.Add(ToCategoryResponseModel(category));
            }

            return models;
        }
    }
}
