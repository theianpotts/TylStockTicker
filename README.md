# TylStockTicker

This application implements a simple Stock Ticker API.
It makes uses of a SQL database generated using Entity Framework Core.
The API is structured with thin controller and a service layer. Linq is used to query the database.

A unit test project is included which makes use of InMemory DBs to test the service calls and ensure the right results are returned.

The application can be built in VisualStudio (2022 was used) and the unit tests run.

TO SCALE UP THIS APPLICATION

It would be better not to calculate the average ticker values via querying multiple rows as currently done. Either caching the results or making use of a second table which is updated with the current average value of a given ticker symbol would greatly increase performance.