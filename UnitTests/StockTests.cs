using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TylStockTicker.Data;
using TylStockTicker.Models;
using TylStockTicker.Services;

namespace UnitTests
{
    public class StockTests
    {           
        StockService _subject;
        StockTransaction _stockTransaction;
        Mock<ILogger<StockService>> _logger;

        [SetUp]
        public void SetUp()
        {
            _logger = new Mock<ILogger<StockService>>();

            _stockTransaction = new StockTransaction()
            {
                StockTickerSymbol = "XRO",
                Price = 89.0M,
                NumberOfShares = 100.0M,
                BrokerId = "Bob"
            };
        }

        [Test]
        async public Task Can_Add_A_Transaction()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<StockContext>()
                .UseInMemoryDatabase(databaseName: "StockTickerDatabase")
                .Options;

            using (var context = new StockContext(options))
            {
                _subject = new StockService(context, _logger.Object);

                // Act
                var result = await _subject.AddTransaction(_stockTransaction);

                // Assert
                Assert.That(result, Is.EqualTo(true));
            }
        }


        [Test]
        async public Task Can_Get_Stock_Value()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<StockContext>()
                .UseInMemoryDatabase(databaseName: "StockTickerDatabase2")
                .Options;

            using (var context = new StockContext(options))
            {
                _subject = new StockService(context, _logger.Object);

                // Act
                await _subject.AddTransaction(_stockTransaction);
                var result = await _subject.GetStockValue("XRO");

                // Assert
                Assert.That(result, Is.EqualTo(89.0M));
            }
        }

        [Test]
        async public Task Can_Get_Stock_Value_Average()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<StockContext>()
                .UseInMemoryDatabase(databaseName: "StockTickerDatabase3")
                .Options;

            using (var context = new StockContext(options))
            {
                _subject = new StockService(context, _logger.Object);

                // Act
                _stockTransaction.StockTransactionId = null;
                _stockTransaction.Price = 20.0M;
                await _subject.AddTransaction(_stockTransaction);
                _stockTransaction.StockTransactionId = null;
                _stockTransaction.Price = 40.0M;
                await _subject.AddTransaction(_stockTransaction);
                var result = await _subject.GetStockValue("XRO");

                // Assert
                Assert.That(result, Is.EqualTo(30.0M));
            }
        }

        [Test]
        async public Task Can_Get_Stock_Value_Average_Across_Many()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<StockContext>()
                .UseInMemoryDatabase(databaseName: "StockTickerDatabase4")
                .Options;

            using (var context = new StockContext(options))
            {
                _subject = new StockService(context, _logger.Object);

                // Act
                _stockTransaction.StockTransactionId = null;
                _stockTransaction.Price = 20.0M;
                await _subject.AddTransaction(_stockTransaction);
                _stockTransaction.StockTransactionId = null;
                _stockTransaction.Price = 40.0M;
                await _subject.AddTransaction(_stockTransaction);
                _stockTransaction.StockTransactionId = null;
                _stockTransaction.Price = 60.0M;
                await _subject.AddTransaction(_stockTransaction);
                _stockTransaction.StockTransactionId = null;
                _stockTransaction.Price = 80.0M;
                await _subject.AddTransaction(_stockTransaction);
                var result = await _subject.GetStockValue("XRO");

                // Assert
                Assert.That(result, Is.EqualTo(50.0M));
            }
        }

        [Test]
        async public Task Can_Get_All_Stock_Value_Averages()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<StockContext>()
                .UseInMemoryDatabase(databaseName: "StockTickerDatabase5")
                .Options;

            using (var context = new StockContext(options))
            {
                _subject = new StockService(context, _logger.Object);

                // Act
                _stockTransaction.StockTransactionId = null;
                _stockTransaction.Price = 20.0M;
                await _subject.AddTransaction(_stockTransaction);
                _stockTransaction.StockTransactionId = null;
                _stockTransaction.Price = 40.0M;
                await _subject.AddTransaction(_stockTransaction);
                _stockTransaction.StockTransactionId = null;
                _stockTransaction.StockTickerSymbol = "IBM";
                _stockTransaction.Price = 60.0M;
                await _subject.AddTransaction(_stockTransaction);
                _stockTransaction.StockTransactionId = null;
                _stockTransaction.Price = 80.0M;
                await _subject.AddTransaction(_stockTransaction);
                var result = _subject.GetAllStockValues();

                // Assert
                Assert.That(result, Is.Not.EqualTo(null));
                Assert.That(result[0].StockTickerSymbol, Is.EqualTo("XRO"));
                Assert.That(result[0].Value, Is.EqualTo(30.0M));
                Assert.That(result[1].StockTickerSymbol, Is.EqualTo("IBM"));
                Assert.That(result[1].Value, Is.EqualTo(70.0M));
            }
        }

        [Test]
        async public Task Can_Get_Range_Stock_Value_Averages()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<StockContext>()
                .UseInMemoryDatabase(databaseName: "StockTickerDatabase6")
                .Options;

            using (var context = new StockContext(options))
            {
                _subject = new StockService(context, _logger.Object);

                // Act
                _stockTransaction.StockTransactionId = null;
                _stockTransaction.Price = 20.0M;
                await _subject.AddTransaction(_stockTransaction);
                _stockTransaction.StockTransactionId = null;
                _stockTransaction.Price = 40.0M;
                await _subject.AddTransaction(_stockTransaction);
                _stockTransaction.StockTransactionId = null;
                _stockTransaction.StockTickerSymbol = "IBM";
                _stockTransaction.Price = 60.0M;
                await _subject.AddTransaction(_stockTransaction);
                _stockTransaction.StockTransactionId = null;
                _stockTransaction.Price = 80.0M;
                await _subject.AddTransaction(_stockTransaction);

                _stockTransaction.StockTransactionId = null;
                _stockTransaction.StockTickerSymbol = "BBC";
                _stockTransaction.Price = 50.0M;
                await _subject.AddTransaction(_stockTransaction);
                _stockTransaction.StockTransactionId = null;
                _stockTransaction.Price = 70.0M;
                await _subject.AddTransaction(_stockTransaction);

                string[] tickers = new string[2] { "XRO", "BBC" };
                var result = _subject.GetStockValues(tickers);

                // Assert
                Assert.That(result, Is.Not.EqualTo(null));
                Assert.That(result[0].StockTickerSymbol, Is.EqualTo("XRO"));
                Assert.That(result[0].Value, Is.EqualTo(30.0M));
                Assert.That(result[1].StockTickerSymbol, Is.EqualTo("BBC"));
                Assert.That(result[1].Value, Is.EqualTo(60.0M));
            }
        }
    }
}