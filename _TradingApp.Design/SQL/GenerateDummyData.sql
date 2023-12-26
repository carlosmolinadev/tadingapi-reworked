-- Dummy data for exchange table
INSERT INTO exchange (name) VALUES
  ('Binance'),
  ('Okex'),
  ('BingX');

-- Dummy data for account table
INSERT INTO account (id, balance, risk_per_trade, user_id, exchange_id) VALUES
  ('11111111-1111-1111-1111-111111111111', 10000.0000, 2.5, '22222222-2222-2222-2222-222222222222', 1),
  ('33333333-3333-3333-3333-333333333333', 5000.0000, 1.8, '44444444-4444-4444-4444-444444444444', 2),
  ('55555555-5555-5555-5555-555555555555', 7500.0000, 3.0, '66666666-6666-6666-6666-666666666666', 3);

-- Dummy data for account_api table
INSERT INTO account_api (private_key, public_key, account_id, exchange_id) VALUES
  ('privatekey1', 'publickey1', '11111111-1111-1111-1111-111111111111', 1),
  ('privatekey2', 'publickey2', '33333333-3333-3333-3333-333333333333', 2),
  ('privatekey3', 'publickey3', '55555555-5555-5555-5555-555555555555', 3);

-- Dummy data for derivate table
INSERT INTO derivate (id, value, exchange_id) VALUES
  (1, 'Spot', 1),
  (2, 'Futures', 2),
  (3, 'Coin', 3);

-- Dummy data for symbol table
INSERT INTO symbol (value, exchange_id) VALUES
  ('SymbolValue1', 1),
  ('SymbolValue2', 2),
  ('SymbolValue3', 3);

-- Dummy data for order_side table
INSERT INTO order_side (id, value) VALUES
  (1, 'Buy'),
  (2, 'Sell');

-- Dummy data for order_status table
INSERT INTO order_status (id, value) VALUES
  (1, 'Pending'),
  (2, 'Filled'),
  (3, 'Cancelled');

-- Dummy data for order_type table
INSERT INTO order_type (id, value) VALUES
  (1, 'Market'),
  (2, 'Limit'),
  (3, 'Stop');

-- Dummy data for trade_status table
INSERT INTO trade_status (id, value) VALUES
  (1, 'Open'),
  (2, 'Closed'),
  (3, 'Cancelled');

-- Dummy data for trade table
INSERT INTO trade (id, risk_reward, activation_price, candle_close_entry, retry_attempt, percentage_entry, symbol, account_id, trade_status_id) VALUES
  (1, 2.0, 105.0000, true, 1, false, 'SymbolValue1', '11111111-1111-1111-1111-111111111111', 1),
  (2, 1.5, 50.0000, false, 2, true, 'SymbolValue2', '33333333-3333-3333-3333-333333333333', 2),
  (3, 3.0, 75.0000, true, 1, false, 'SymbolValue3', '55555555-5555-5555-5555-555555555555', 3);

-- Dummy data for trade_order table
INSERT INTO trade_order (id, quantity, stop_price, open_price, parent_id, created_date, updated_date, side_id, type_id, trade_id, status_id) VALUES
  (1, 10.0000, 95.0000, 100.0000, NULL, '2023-10-14 12:00:00', '2023-10-14 12:05:00', 1, 1, 1, 1),
  (2, 5.0000, 48.0000, 55.0000, NULL, '2023-10-14 12:10:00', '2023-10-14 12:15:00', 2, 2, 2, 2),
  (3, 7.0000, 70.0000, 80.0000, NULL, '2023-10-14 12:20:00', '2023-10-14 12:25:00', 1, 3, 3, 3);

-- Dummy data for trade_confirmation_order table
INSERT INTO trade_confirmation_order (id, exchange_order_number, filled_price, fee, confirmation_date, trade_order_id) VALUES
  (1, 'XO123456', 105.5000, 2.0000, '2023-10-14 12:30:00', 1),
  (2, 'XO789012', 55.2000, 1.5000, '2023-10-14 12:35:00', 2),
  (3, 'XO345678', 80.5000, 3.0000, '2023-10-14 12:40:00', 3);
