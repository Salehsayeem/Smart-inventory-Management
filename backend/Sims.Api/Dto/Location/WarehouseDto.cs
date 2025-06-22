namespace Sims.Api.Dto.Location
{
    public class CreateOrUpdateWarehouseDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public long ShopId { get; set; }
    }
    public class GetWarehouseByIdDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public long ShopId { get; set; }
    }
    public class WarehouseLandingDataDto
    {
        public int Sl { get; set; }
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = String.Empty;
    }

}
