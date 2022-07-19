using Microsoft.AspNetCore.Mvc;
using TylStockTicker.Models;
using TylStockTicker.Services;

namespace TylStockTicker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockTickerController : ControllerBase
    {
        private readonly ILogger<StockTickerController> _logger;
        private readonly IStockService _stockService;

        public StockTickerController(ILogger<StockTickerController> logger, IStockService stockService)
        {
            _logger = logger;
            _stockService = stockService;
        }

        /// <summary>
        /// Create a stock ticker transaction. Returns 200 if successful.
        /// Data passed in as body of request as JSON (using the StockTransaction model)
        /// POST api/<ValuesController>
        /// </summary>
        /// <param name="stockTransaction"></param>
        /// <returns></returns>
        [HttpPost("AddTransaction")]
        public async Task<ActionResult> AddTransaction(StockTransaction stockTransaction)
        {
            if (string.IsNullOrEmpty(stockTransaction.StockTickerSymbol) ||
                string.IsNullOrEmpty(stockTransaction.BrokerId))
            {
                _logger.LogError("AddTransaction bad request with missing stockticker or broker id");
                return BadRequest();
            }

            // Call service and return result
            if (await _stockService.AddTransaction(stockTransaction))
                return Ok();
            else
                return Problem();
        }

        /// <summary>
        /// Get the value of a given stock ticker
        /// </summary>
        /// <param name="stockTickerId"></param>
        /// <returns></returns>
        [HttpGet("GetStockValue")]
        public async Task<ActionResult> GetStockValue(string stockTickerSymbol)
        {
            if (string.IsNullOrEmpty(stockTickerSymbol))
            {
                _logger.LogError("GetStockValue bad request with missing stockticker id");
                return BadRequest();
            }

            // Call service and return result
            var value = await _stockService.GetStockValue(stockTickerSymbol);
            if (value.HasValue)
                return Ok(value);
            else
                return Problem("Failed to obtain stock value for " + stockTickerSymbol);
        }

        /// <summary>
        /// Get the values of all stock tickers
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllStockValues")]
        public ActionResult GetAllStockValues()
        {
            // Call service and return result
            var values = _stockService.GetAllStockValues();
            if (values != null)
                return Ok(values);
            else
                return Problem("Failed to obtain stock values");
        }

        /// <summary>
        /// Get the values of the given stock tickers
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetStockValues")]
        public ActionResult GetStockValues(string[] stockTickerSymbols)
        {
            // Call service and return result
            var values = _stockService.GetStockValues(stockTickerSymbols);
            if (values != null)
                return Ok(values);
            else
                return Problem("Failed to obtain stock values");
        }
    }
}
