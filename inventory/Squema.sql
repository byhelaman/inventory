CREATE DATABASE inventory_db;
GO

USE inventory_db;
GO

CREATE TABLE users (
    user_id INT IDENTITY(1,1) PRIMARY KEY,
    username NVARCHAR(100) NOT NULL UNIQUE,
    password_hash NVARCHAR(255) NOT NULL,
    email NVARCHAR(255) NULL,
    full_name NVARCHAR(255) NULL,
    date_created DATETIME DEFAULT GETDATE()
);
GO

CREATE TABLE categories (
    category_id INT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(100) NOT NULL,
    description NVARCHAR(255) NULL
);
GO

CREATE TABLE products (
    product_id INT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(255) NOT NULL,
    description NVARCHAR(255) NULL,
    category_id INT,
    price DECIMAL(18, 2) NOT NULL,
    stock INT DEFAULT 0,
    date_added DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_Products_Categories FOREIGN KEY (category_id) REFERENCES categories(category_id)
);
GO

CREATE TABLE transactions (
    transaction_id INT IDENTITY(1,1) PRIMARY KEY,
    product_id INT,
    quantity INT NOT NULL,
    transaction_type NVARCHAR(50) NOT NULL,
    transaction_date DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_Transactions_Products FOREIGN KEY (product_id) REFERENCES products(product_id)
);
GO

CREATE TABLE suppliers (
    supplier_id INT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(255) NOT NULL,
    contact_name NVARCHAR(255) NULL,
    phone NVARCHAR(15) NULL,
    email NVARCHAR(255) NULL,
    address NVARCHAR(255) NULL
);
GO

CREATE TABLE product_suppliers (
    product_id INT,
    supplier_id INT,
    price DECIMAL(18, 2) NULL,
    PRIMARY KEY (product_id, supplier_id),
    CONSTRAINT FK_ProductSuppliers_Products FOREIGN KEY (product_id) REFERENCES products(product_id),
    CONSTRAINT FK_ProductSuppliers_Suppliers FOREIGN KEY (supplier_id) REFERENCES suppliers(supplier_id)
);
GO

CREATE VIEW inventory_status AS
SELECT 
    p.product_id,
    p.name AS product_name,
    c.name AS category,
    p.price,
    p.stock,
    (SELECT SUM(t.quantity) FROM Transactions t WHERE t.product_id = p.product_id AND t.transaction_type = 'Entrada') -
    (SELECT SUM(t.quantity) FROM Transactions t WHERE t.product_id = p.product_id AND t.transaction_type = 'Salida') AS current_stock
FROM 
    products p
JOIN 
    categories c ON p.category_id = c.category_id
GROUP BY 
    p.product_id, p.name, c.name, p.price, p.stock;
GO
