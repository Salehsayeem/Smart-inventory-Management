using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sims.Api.Dto.Shop
{
    public class CreateOrUpdateShopDto
    {
        public long Id { get; set; }
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [StringLength(100)]
        public string Address { get; set; } = string.Empty;
    }
    public class GetShopByIdDto
    {
        public long Id { get; set; }
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [StringLength(100)]
        public string Address { get; set; } = string.Empty;
    }
}