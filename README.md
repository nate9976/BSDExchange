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
