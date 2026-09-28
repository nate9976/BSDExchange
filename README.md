# BSDExample

A meta-exchange that finds the best way to buy or sell a given amount of BTC across many crypto exchanges.

## How to run

### Run the WebApi

```sh
BSDExchange <buy|sell> <amount in BTC>
```

### Run the console app

```sh
BSDExchange <buy|sell> <amount in BTC>
```

Examples:

```sh
BSDExchange buy 1
BSDExchange sell 2
```

## Data

Data is stored in the `order_books_data` folder .

**`order_books_data/order_books_data`**: provided file with sample data.

Lines that cannot be parsed are skipped with a message.

**`order_books_data/balances.json`**: generated file with example data which includes starting balance for each exchange:

```json
[
  { "ExchangeId": "1548759600.25189", "EurBalance": 2818.15, "BtcBalance": 2.61382138 }
]
```

## Task

---
BACKGROUND KNOWLEDGE
To understand the task, educate yourself about the order book (basic knowledge is more than enough).
For simplicity, let's assume that we are dealing with a cryptoexchange that only offers Bitcoin (BTC) and you can sell or buy it only for EUR. At any given time, an order book is the bid-ask state of this cryptoexchange. In other words, an order book is a bunch of "bids" and "asks". In our example, a bid is the price at which the buyer is willing to buy a certain amount of BTC. An ask is the price at which the seller is willing to sell a certain amount of BTC. See Figure 1. When a bid and an ask are matched, a trade is made, but this is not relevant for solving this task.
Take a look at Figure 2. How much EUR do you need to buy 9 BTC at the lowest possible price? The answer is 7 x 3k EUR + 2 x 3.3k EUR = 27,600 EUR.
THE TASK
- Part 1 -
Your task is to implement a meta-exchange that always gives the user the best possible price if he is buying or selling a certain amount of BTC. Technically, you will be given n order books [from n different cryptoexchanges], the type of order (buy or sell), and the amount of BTC that our user wants to buy or sell. Your algorithm needs to output one or more buy or sell orders that then our system called Hedger issues to one or more of these n cryptoexchanges. In effect, our user buys the specified amount of BTC for the lowest possible price, or sells the specified amount of BTC for the highest possible price.

To make life a bit more complicated, each cryptoexchange has EUR and BTC balance. Your algorithm needs to achieve the best price within these constraints. The algorithm cannot transfer any money and/or crypto between cryptoexchanges.
Attached to this e-mail, you will find a bunch of JSON files with order books. Use these to test your solution (there is one order book per line in the file, take as many as you need from there). Your solution should be a relatively simple .NET console application, which reads the order books, balance constraints, and order type / size, and outputs (to console) a set of orders to execute against the given order books (exchanges). Please prepare a function that solves the task.
- Part 2 -
Implement a Web service (Kestrel, .NET API), and expose the implemented functionality through it. Implement an endpoint that will receive the required parameters (the type of order and the amount of BTC that our user wants to buy or sell), and return a JSON response with the "best execution" plan. Please also provide SwaggerUI or provide some other means for easy API invocation.
BONUS TASKS
- Write some tests, on relatively simple input data (e.g., order books with only a few bids and asks), to test your solution on typical and edge cases.
- Deploy your Web service locally with Docker.
OTHER NOTES
Please use GIT while developing the code. Keep it as simple as the nature of this problem allows it. Be ready to explain / demonstrate your solution in our office.
May the force be with you.