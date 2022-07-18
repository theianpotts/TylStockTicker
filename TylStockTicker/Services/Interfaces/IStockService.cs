using TylStockTicker.Models;

namespace TylStockTicker.Services
{
    public interface IStockService
    {
        public Task<bool> AddTransaction(StockTransaction stockTransaction);

        public Task<decimal?> GetStockValue(string stockTickerSymbol);

        public StockValue[]? GetAllStockValues();

        public StockValue[]? GetStockValues(string[] stockTickerSymbols);
    }
}