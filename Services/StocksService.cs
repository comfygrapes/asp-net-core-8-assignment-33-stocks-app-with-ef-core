using Entities;
using ServiceContracts;
using ServiceContracts.DTOs;
using Services.Helpers;

namespace Services
{
    public class StocksService : IStocksService
    {
        private readonly StockMarketDbContext _stockMarketDbContext;

        public StocksService(StockMarketDbContext stockMarketDbContext)
        {
            _stockMarketDbContext = stockMarketDbContext;
        }

        public async Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest)
        {
            if (buyOrderRequest == null) throw new ArgumentNullException(nameof(buyOrderRequest));
            ValidationHelper.ValidateModel(buyOrderRequest);

            var buyOrder = buyOrderRequest.ToBuyOrder(Guid.NewGuid());

            await _stockMarketDbContext.AddAsync(buyOrder);
            await _stockMarketDbContext.SaveChangesAsync();

            return buyOrder.ToBuyOrderResponse();
        }

        public async Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest)
        {
            if (sellOrderRequest == null) throw new ArgumentNullException(nameof(sellOrderRequest));
            ValidationHelper.ValidateModel(sellOrderRequest);

            var sellOrder = sellOrderRequest.ToSellOrder(Guid.NewGuid());

            await _stockMarketDbContext.AddAsync(sellOrder);
            await _stockMarketDbContext.SaveChangesAsync();

            return sellOrder.ToSellOrderResponse();
        }

        public async Task<List<BuyOrderResponse>> GetAllBuyOrders()
        {
            var allBuyOrders = _stockMarketDbContext.BuyOrders.Select(buyOrder => buyOrder.ToBuyOrderResponse()).ToList();

            return allBuyOrders;
        }

        public async Task<List<SellOrderResponse>> GetAllSellOrders()
        {
            var allSellOrders = _stockMarketDbContext.SellOrders.Select(sellOrder => sellOrder.ToSellOrderResponse()).ToList();

            return allSellOrders;
        }
    }
}
