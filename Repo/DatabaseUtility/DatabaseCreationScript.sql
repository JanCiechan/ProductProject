drop table if exists Product;
CREATE TABLE Product (
    ID VARCHAR(50) PRIMARY KEY,
    SKU VARCHAR(50) UNIQUE,
    name VARCHAR(255),
    EAN VARCHAR(50),
    producer_name VARCHAR(255),
    category VARCHAR(255),
    is_wire BIT,
    available BIT,
    is_vendor BIT,
    default_image VARCHAR(511)
);

drop table if exists Inventory;
CREATE TABLE Inventory (
    product_id VARCHAR(50) PRIMARY KEY,
    sku VARCHAR(50),
    unit VARCHAR(50),
    qty INT,
    manufacturer_name VARCHAR(255),
    shipping VARCHAR(100),
    shipping_cost DECIMAL(10, 2)
);


drop table if exists Prices;
CREATE TABLE Prices (
    InternalID VARCHAR(50) PRIMARY KEY,
    SKU VARCHAR(50),
    NettPrice DECIMAL(10, 2),
    NettPriceAfterDiscount DECIMAL(10, 2),
    VATRate DECIMAL(5, 2),
    NettPriceAfterDiscountForLogisticUnit DECIMAL(10, 2)
);
