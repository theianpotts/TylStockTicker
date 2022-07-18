namespace TylStockTicker.Models
{
    public class StockTransaction
    {
        public int? StockTransactionId { get; set; }
        public string StockTickerSymbol { get; set; }

        public decimal Price { get; set; }

        public decimal NumberOfShares { get; set; }

        public string BrokerId { get; set; }
    }
}