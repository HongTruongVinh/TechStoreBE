using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Constants;
using TechStore.Data.Entities;
using TechStore.Model.DTOs.Comment;

namespace TechStore.Service.Mappers
{
    public static class CommentMappers
    {
        public static CommentResponseModel ToCommentCustomerModel(this Comment comment, User user)
        {
            return new CommentResponseModel
            {
                CommentId = comment.PublicId,
                ProductId = comment.Product.PublicId,

                UserAvatar = user.PictureUrl ?? CloudinaryFolders.DefaultUserImage,
                UserId = user.PublicId,
                UserName = user.LastName + user.FirstName,

                Content = comment.Content,
                CreatedAt = comment.CreatedAt,
                UpdatedAt = comment.UpdatedAt,
                Like = comment.Like,
                Dislike = comment.Dislike,
                Stars = comment.Stars,
            };
        }
    }
}
