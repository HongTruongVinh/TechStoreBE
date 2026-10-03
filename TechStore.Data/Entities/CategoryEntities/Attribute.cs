using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStore.Data.Entities.CategoryEntities
{
    public class Attribute : BaseEntity
    {
        public required string Name { get; set; }
        public required string Code { get; set; }
        public required string DataType { get; set; }

            //Text
            //Number
            //Boolean
            //SingleSelect
            //MultiSelect
    }
}
