using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Constants;
using TechStore.Common.Helpers;
using TechStore.Data.Entities;
using TechStore.Model.DTOs.User;

namespace TechStore.Service.Mappers
{
    public static class UserMappers
    {
        public static UserListItemResponseModel ToUserListItemModel(this User user)
        {
            return new UserListItemResponseModel
            {
                UserId = user.PublicId,
                LastName = user.LastName,
                FirstName = user.FirstName,

                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,

                PictureUrl = user.PictureUrl??CloudinaryFolders.DefaultImage,

                Status = user.Status,

                CreatedAt = user.CreatedAt
            };
        }

        public static List<UserListItemResponseModel> ToListUserListItemModels(this List<User> users)
        {
            var models = new List<UserListItemResponseModel>();

            foreach (var user in users)
            {
                models.Add(ToUserListItemModel(user));
            }

            return models;
        }

        public static UserResponseModel ToUserResponseModel(this User user, string roleName)
        {
            return new UserResponseModel
            {
                Id = user.PublicId,
                LastName = user.LastName,
                FirstName = user.FirstName,

                Address = user.Address,
                City = user.City,
                District = user.District,

                Email = user.Email ?? "",
                PhoneNumber = user.PhoneNumber,

                PictureUrl = user.PictureUrl,
                Gender = user.Gender,
                Birthday = user.Birthday,

                RoleId = user.RoleId,
                RoleName = roleName,
                Status = user.Status,

                CreatedAt = user.CreatedAt
            };
        }

        public static CustomerListItemModel ToCustomersLisItemModel(this User user, List<OrderHistoryModel> orders)
        {
            return new CustomerListItemModel
            {
                UserId = user.PublicId,
                LastName = user.LastName,
                FirstName = user.FirstName,

                Email = user.Email ?? "",
                PhoneNumber = user.PhoneNumber ?? "",
                Address = user.Address,

                PictureUrl = user.PictureUrl ?? CloudinaryFolders.DefaultUserImage,
                Gender = user.Gender,
                Birthday = user.Birthday,

                Status = user.Status,
                CreatedAt = user.CreatedAt,

                Orders = orders
            };
        }

        public static StaffListItemModel ToStaffLisItemModel(this User user)
        {
            return new StaffListItemModel
            {
                UserId = user.PublicId,
                LastName = user.LastName,
                FirstName = user.FirstName,

                Email = user.Email ?? "",
                PhoneNumber = user.PhoneNumber ?? "",
                Address = user.Address,

                PictureUrl = user.PictureUrl ?? CloudinaryFolders.DefaultUserImage,
                Gender = user.Gender,
                Birthday = user.Birthday,

                Status = user.Status,
                CreatedAt = user.CreatedAt
            };
        }
    }
}
