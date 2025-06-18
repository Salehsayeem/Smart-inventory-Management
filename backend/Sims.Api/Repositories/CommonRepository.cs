using Sims.Api.Dto;
using Sims.Api.Helper;
using Sims.Api.IRepositories;
using Sims.Api.StoredProcedure;

namespace Sims.Api.Repositories
{
    public class CommonRepository : ICommonRepository
    {
        public readonly AppSettings _appSettings;

        public CommonRepository(AppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public CommonResponseDto AllCategoriesOfShopDdl(long shopId)
        {
            try
            {
                CallStoredProcedure sp = new CallStoredProcedure();
                var data = sp.CategoryListOfShopDdl(shopId,_appSettings.ConnectionString);
                return new CommonResponseDto()
                {
                    Data = data,
                    StatusCode = 200,
                };

            }
            catch (Exception e)
            {
                throw new Exception($"Error in {nameof(AllCategoriesOfShopDdl)}: {e.Message}", e);
            }
        }

        public CommonResponseDto AllProductsOfShopDdl(long shopId)
        {
            try
            {
                CallStoredProcedure sp = new CallStoredProcedure();
                var data = sp.ProductListOfShopDdl(shopId, _appSettings.ConnectionString);
                return new CommonResponseDto()
                {
                    Data = data,
                    StatusCode = 200,
                };

            }
            catch (Exception e)
            {
                throw new Exception($"Error in {nameof(AllProductsOfShopDdl)}: {e.Message}", e);
            }
        }
    }
}
