using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Model.DTOs.Authentication
{
    public class LoginRequestModel
    {
        [StringLength(30, MinimumLength = 6, ErrorMessage = "Độ dài LoginIdentifier phải từ 6 đến 50 ký tự.")]
        public required string LoginIdentifier { get; set; }

        [StringLength(20, MinimumLength = 6, ErrorMessage = "Độ dài Password phải từ 6 đến 50 ký tự.")]
        public required string Password { get; set; }
    }
}
