using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStore.Common.Models;
using TechStore.Model.DTOs.Shipper;

namespace TechStore.Service.Interfaces
{
    public interface IShipperService
    {
        Task<ServiceResult<string>> AddShipper(ShipperCreateModel model);
        Task<ServiceResult<bool>> UpdateShipper(string id, ShipperUpdateModel model);
        Task<ServiceResult<bool>> DeleteShipper(string id);
        Task<ServiceResult<ShipperResponseModel>> GetShipperById(string id);
        Task<ServiceResult<List<ShipperResponseModel>>> GetAllShippers();
        Task<ServiceResult<List<ShipperResponseModel>>> GetShippersByStatus(bool status);
    }
}
