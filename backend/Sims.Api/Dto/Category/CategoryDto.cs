﻿using System.ComponentModel.DataAnnotations;

namespace Sims.Api.Dto.Category
{

    public class CreateOrUpdateCategoryDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public long ShopId { get; set; }
    }
    public class GetCategoryByIdDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public long ShopId { get; set; }
    }

    public class CategoryLandingDataDto
    {
        public int Sl { get; set; }
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = String.Empty;
    }
}

