using Microsoft.EntityFrameworkCore;
using TylStockTicker.Data;
using TylStockTicker.Models;

namespace TylStockTicker.Services
{
    public class StockService : IStockService
    {
        private readonly StockContext _context;
        private readonly ILogger<StockService> _logger;

        public StockService(StockContext context, ILogger<StockService> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Add a transaction for a given stock ticker
        /// </summary>
        /// <param name="stockTransaction"></param>
        /// <returns></returns>
        public async Task<bool> AddTransaction(StockTransaction stockTransaction)
        {
            try
            {
                await _context.StockTransactions.AddAsync(stockTransaction);

                if (await _context.SaveChangesAsync() > 0)
                    return true;
            }
            catch (Exception ex)
            {
                // Log error
                _logger.LogError("AddTransaction failed: " + ex.Message);
            }

            return false;
        }

        /// <summary>
        /// Return the current value of a given stock ticker symbol
        /// </summary>
        /// <param name="stockTickerSymbol"></param>
        /// <returns></returns>
        public async Task<decimal?> GetStockValue(string stockTickerSymbol)
        {           
            try
            {
                return await _context.StockTransactions.Where(m => m.StockTickerSymbol == stockTickerSymbol)
                        .AverageAsync<StockTransaction>(m => m.Price);
                
            }
            catch (Exception ex)
            {
                // Log error
                _logger.LogError("GetStockValue failed: " + ex.Message);
            }

            return null;
        }

        /// <summary>
        /// Return the current values of all stock ticker symbols in an array
        /// </summary>
        /// <returns></returns>
        public StockValue[]? GetAllStockValues()
        {
            try
            {
                // NB. Whilst this works, the averaging of each value is intensive.
                // A sensible improvement would be to either cache average values per symbol,
                // or have another database table which contains the current values per symbol, which
                // should be updated whenever there is a transaction. Then this code would simply get the
                // value for the given symbol from that table

                // (By reducing the complexity of the Linq statement that would also allow us to make this method
                // async more easily)
                var symbols = _context.StockTransactions.Select(m => m.StockTickerSymbol).Distinct()
                    .Select(s => new StockValue()
                    { 
                       StockTickerSymbol = s,
                       Value = _context.StockTransactions.Where(m => m.StockTickerSymbol == s)
                                .Average<StockTransaction>(m => m.Price)
                    });

                return symbols.ToArray();

            }
            catch (Exception ex)
            {
                // Log error
                _logger.LogError("GetAllStockValues failed: " + ex.Message);
            }

            return null;
        }

        /// <summary>
        /// Return the current values of the given stock ticker symbols in an array
        /// </summary>
        /// <returns></returns>
        public StockValue[]? GetStockValues(string[] stockTickerSymbols)
        {
            try
            {
                // NB. Whilst this works, the averaging of each value is intensive.
                // A sensible improvement would be to either cache average values per symbol,
                // or have another database table which contains the current values per symbol, which
                // should be updated whenever there is a transaction. Then this code would simply get the
                // value for the given symbol from that table

                // (By reducing the complexity of the Linq statement that would also allow us to make this method
                // async more easily)
                var symbols = _context.StockTransactions.Select(m => m.StockTickerSymbol).Distinct()
                    .Where(x => stockTickerSymbols.Any(y => y == x))
                    .Select(s => new StockValue()
                    {
                        StockTickerSymbol = s,
                        Value = _context.StockTransactions.Where(m => m.StockTickerSymbol == s)
                                .Average<StockTransaction>(m => m.Price)
                    });

                return symbols.ToArray();

            }
            catch (Exception ex)
            {
                // Log error
                _logger.LogError("GetAllStockValues failed: " + ex.Message);
            }

            return null;
        }
    }
}