
        
CREATE TABLE account
(
  id             uuid          NOT NULL,
  balance        numeric(10,4) NOT NULL,
  risk_per_trade numeric(5,2) ,
  user_id        uuid          NOT NULL,
  exchange_id    int           NOT NULL,
  derivate_id    int           NOT NULL,
  PRIMARY KEY (id)
);

CREATE TABLE account_api
(
  private_key text NOT NULL,
  public_key  text NOT NULL,
  account_id  uuid NOT NULL,
  PRIMARY KEY (private_key, public_key)
);

CREATE TABLE default_trade_parameter
(
  account_id         uuid         NOT NULL,
  risk_reward        numeric(5,2) NOT NULL,
  candle_close_entry int          NOT NULL,
  delayed_entry      int          NOT NULL,
  close_condition    text         NOT NULL,
  close_value        numeric(5,2) NOT NULL,
  retry_attempt      int          NOT NULL
);

CREATE TABLE derivate
(
  id          int  NOT NULL GENERATED ALWAYS AS IDENTITY,
  name        text NOT NULL,
  exchange_id int  NOT NULL,
  symbol_id   int  NOT NULL,
  PRIMARY KEY (id)
);

CREATE TABLE exchange
(
  id   int  NOT NULL GENERATED ALWAYS AS IDENTITY,
  name text NOT NULL,
  PRIMARY KEY (id)
);

CREATE TABLE order_side
(
  id    int  NOT NULL,
  value text NOT NULL,
  PRIMARY KEY (id)
);

CREATE TABLE order_status
(
  id    int  NOT NULL,
  value text NOT NULL,
  PRIMARY KEY (id)
);

CREATE TABLE order_type
(
  id    int  NOT NULL,
  value text NOT NULL,
  PRIMARY KEY (id)
);

CREATE TABLE symbol
(
  id                     int  NOT NULL GENERATED ALWAYS AS IDENTITY,
  value                  text NOT NULL,
  price_decimal_digit    int  NOT NULL,
  quantity_decimal_digit int  NOT NULL,
  PRIMARY KEY (id)
);

CREATE TABLE trade
(
  id                 int          NOT NULL GENERATED ALWAYS AS IDENTITY,
  risk_reward        numeric(5,2),
  candle_close_entry boolean     ,
  retry_attempt      int          NOT NULL,
  percentage_entry   boolean      NOT NULL,
  symbol             text         NOT NULL,
  account_id         uuid         NOT NULL,
  trade_status_id    int          NOT NULL,
  PRIMARY KEY (id)
);

CREATE TABLE trade_confirmation_order
(
  id                    int           NOT NULL GENERATED ALWAYS AS IDENTITY,
  exchange_order_number text          NOT NULL,
  filled_price          numeric(10,4) NOT NULL,
  fee                   numeric(10,4) NOT NULL,
  confirmation_date     timestamp    ,
  trade_order_id        int           NOT NULL,
  PRIMARY KEY (id)
);

CREATE TABLE trade_order
(
  id           int           NOT NULL GENERATED ALWAYS AS IDENTITY,
  quantity     numeric(10,4) NOT NULL,
  stop_price   numeric(10,4),
  open_price   numeric(10,4) NOT NULL,
  parent_id    int          ,
  created_date timestamp    ,
  updated_date timestamp    ,
  side_id      int           NOT NULL,
  type_id      int           NOT NULL,
  trade_id     int           NOT NULL,
  status_id    int           NOT NULL,
  PRIMARY KEY (id)
);

CREATE TABLE trade_status
(
  id    int  NOT NULL,
  value text NOT NULL,
  PRIMARY KEY (id)
);

ALTER TABLE account
  ADD CONSTRAINT FK_exchange_TO_account
    FOREIGN KEY (exchange_id)
    REFERENCES exchange (id);

ALTER TABLE account_api
  ADD CONSTRAINT FK_account_TO_account_api
    FOREIGN KEY (account_id)
    REFERENCES account (id);

ALTER TABLE trade
  ADD CONSTRAINT FK_account_TO_trade
    FOREIGN KEY (account_id)
    REFERENCES account (id);

ALTER TABLE trade_order
  ADD CONSTRAINT FK_order_side_TO_trade_order
    FOREIGN KEY (side_id)
    REFERENCES order_side (id);

ALTER TABLE trade_order
  ADD CONSTRAINT FK_order_type_TO_trade_order
    FOREIGN KEY (type_id)
    REFERENCES order_type (id);

ALTER TABLE trade_order
  ADD CONSTRAINT FK_trade_TO_trade_order
    FOREIGN KEY (trade_id)
    REFERENCES trade (id);

ALTER TABLE trade_order
  ADD CONSTRAINT FK_order_status_TO_trade_order
    FOREIGN KEY (status_id)
    REFERENCES order_status (id);

ALTER TABLE trade_confirmation_order
  ADD CONSTRAINT FK_trade_order_TO_trade_confirmation_order
    FOREIGN KEY (trade_order_id)
    REFERENCES trade_order (id);

ALTER TABLE trade
  ADD CONSTRAINT FK_trade_status_TO_trade
    FOREIGN KEY (trade_status_id)
    REFERENCES trade_status (id);

ALTER TABLE derivate
  ADD CONSTRAINT FK_exchange_TO_derivate
    FOREIGN KEY (exchange_id)
    REFERENCES exchange (id);

ALTER TABLE account
  ADD CONSTRAINT FK_derivate_TO_account
    FOREIGN KEY (derivate_id)
    REFERENCES derivate (id);

ALTER TABLE default_trade_parameter
  ADD CONSTRAINT FK_account_TO_default_trade_parameter
    FOREIGN KEY (account_id)
    REFERENCES account (id);

ALTER TABLE derivate
  ADD CONSTRAINT FK_symbol_TO_derivate
    FOREIGN KEY (symbol_id)
    REFERENCES symbol (id);

        
      