using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Models;
using TechStore.Model.DTOs.Comment;

namespace TechStore.Service.Interfaces
{
    public interface ICommentService
    {
        Task<ServiceResult<List<CommentResponseModel>>> GetProductComments(string productId);
        Task<ServiceResult<string>> AddProductComment(string userId, CommentCreateModel model);
        Task<ServiceResult<bool>> UpdateProductComment(string userId, string commentId, CommentUpdateModel model);
        Task<ServiceResult<bool>> DeleteProductComment(string commentId);
    }
}
