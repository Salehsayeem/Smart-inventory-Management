using Sims.Api.Dto;
using Sims.Api.Helper;
using Sims.Api.IRepositories;
using Sims.Api.StoredProcedure;
using static Sims.Api.Helper.CommonHelper;

namespace Sims.Api.Repositories
{
    public class CommonRepository : ICommonRepository
    {
        private readonly CallStoredProcedure _spCaller;
        public CommonRepository(CallStoredProcedure spCaller)
        {
            _spCaller = spCaller;
        }

        public async Task<List<CommonDdlDto>> AllCategoriesOfShopDdl(long shopId, string search)
        {
            try
            {
                if (shopId <= 0)
                    throw new ArgumentException("Shop ID must be a positive number", nameof(shopId));

                return await _spCaller.CallFunctionAsync<List<CommonDdlDto>>(
                    StoredProcedureNames.GetCategoryListOfShopDdl,
                    new { p0 = shopId, p1 = search }
                );

            }
            catch (Exception e)
            {
                throw new Exception($"Error in {nameof(AllCategoriesOfShopDdl)}: {e.Message}", e);
            }
        }

        public async Task<List<CommonDdlDto>> AllProductsOfShopDdl(long shopId, string search)
        {
            try
            {
                if (shopId <= 0)
                    throw new ArgumentException("Shop ID must be a positive number", nameof(shopId));

                return await _spCaller.CallFunctionAsync<List<CommonDdlDto>>(
                    StoredProcedureNames.GetProductListOfShopDdl,
                    new { p0 = shopId, p1 = search }
                );
            }
            catch (Exception e)
            {
                throw new Exception($"Error in {nameof(AllProductsOfShopDdl)}: {e.Message}", e);
            }
        }

        public async Task<List<CommonDdlDto>> AllWarehousesOfShopDdl(long shopId, string search)
        {
            try
            {
                if (shopId <= 0)
                    throw new ArgumentException("Shop ID must be a positive number", nameof(shopId));

                return await _spCaller.CallFunctionAsync<List<CommonDdlDto>>(
                    StoredProcedureNames.GetWarehouseListDdl,
                    new { p0 = shopId, p1 = search }
                );
            }
            catch (Exception e)
            {
                throw new Exception($"Error in {nameof(AllWarehousesOfShopDdl)}: {e.Message}", e);
            }
        }
    }
}
