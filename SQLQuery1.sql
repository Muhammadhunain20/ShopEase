-- ================================================
-- ShopEase E-Commerce Database
-- SQL Server Script
-- ================================================

-- Create and use database
CREATE DATABASE ShopEaseDB;
GO

USE ShopEaseDB;
GO

-- ================================================
-- TABLE: Categories
-- ================================================
CREATE TABLE Categories (
    Id       INT IDENTITY(1,1) PRIMARY KEY,
    Name     NVARCHAR(100) NOT NULL
);
GO

-- ================================================
-- TABLE: Products
-- ================================================
CREATE TABLE Products (
    Id            INT IDENTITY(1,1) PRIMARY KEY,
    Name          NVARCHAR(200)   NOT NULL,
    Description   NVARCHAR(1000)  NOT NULL DEFAULT '',
    Price         DECIMAL(18,2)   NOT NULL,
    DiscountPrice DECIMAL(18,2)   NULL,
    Stock         INT             NOT NULL DEFAULT 0,
    ImageUrl      NVARCHAR(500)   NOT NULL DEFAULT '/images/placeholder.jpg',
    CategoryId    INT             NOT NULL,
    IsActive      BIT             NOT NULL DEFAULT 1,
    CreatedAt     DATETIME        NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_Products_Categories
        FOREIGN KEY (CategoryId) REFERENCES Categories(Id)
);
GO

-- ================================================
-- TABLE: Orders
-- ================================================
CREATE TABLE Orders (
    Id            INT IDENTITY(1,1) PRIMARY KEY,
    CustomerName  NVARCHAR(100)  NOT NULL,
    Email         NVARCHAR(150)  NOT NULL,
    Phone         NVARCHAR(15)   NOT NULL,
    Address       NVARCHAR(300)  NOT NULL,
    City          NVARCHAR(100)  NOT NULL,
    ZipCode       NVARCHAR(20)   NOT NULL,
    TotalAmount   DECIMAL(18,2)  NOT NULL,
    Status        NVARCHAR(30)   NOT NULL DEFAULT 'Pending',
    PaymentMethod NVARCHAR(30)   NOT NULL DEFAULT 'COD',
    OrderDate     DATETIME       NOT NULL DEFAULT GETDATE()
);
GO

-- ================================================
-- TABLE: OrderItems
-- ================================================
CREATE TABLE OrderItems (
    Id         INT IDENTITY(1,1) PRIMARY KEY,
    OrderId    INT            NOT NULL,
    ProductId  INT            NOT NULL,
    Quantity   INT            NOT NULL,
    UnitPrice  DECIMAL(18,2)  NOT NULL,

    CONSTRAINT FK_OrderItems_Orders
        FOREIGN KEY (OrderId) REFERENCES Orders(Id) ON DELETE CASCADE,

    CONSTRAINT FK_OrderItems_Products
        FOREIGN KEY (ProductId) REFERENCES Products(Id)
);
GO

-- ================================================
-- SEED: Categories
-- ================================================
INSERT INTO Categories (Name) VALUES
    ('Electronics'),
    ('Clothing'),
    ('Home & Garden'),
    ('Sports');
GO

-- ================================================
-- SEED: Products
-- ================================================
INSERT INTO Products (Name, Description, Price, DiscountPrice, Stock, ImageUrl, CategoryId, IsActive) VALUES

-- Electronics (CategoryId = 1)
(
    'Wireless Headphones',
    'Premium noise-cancelling wireless headphones with 30-hour battery life, built-in mic, and foldable design. Compatible with all Bluetooth devices.',
    8999.00, 6999.00, 50,
    'https://images.unsplash.com/photo-1505740420928-5e560c06d30e?w=400',
    1, 1
),
(
    'Smart Watch',
    'Feature-packed smartwatch with heart rate monitoring, GPS tracking, sleep analysis, and 7-day battery life. Water-resistant up to 50 meters.',
    15999.00, 12499.00, 30,
    'https://images.unsplash.com/photo-1523275335684-37898b6baf30?w=400',
    1, 1
),
(
    'Laptop Stand',
    'Ergonomic aluminum laptop stand with 6 adjustable height levels. Keeps your laptop cool and improves posture. Fits all 11-17 inch laptops.',
    2499.00, NULL, 100,
    'https://images.unsplash.com/photo-1527864550417-7fd91fc51a46?w=400',
    1, 1
),
(
    'Bluetooth Speaker',
    'Portable 360-degree waterproof Bluetooth speaker with 20W output, 12-hour battery, and IPX7 rating. Perfect for outdoor use.',
    5499.00, 3999.00, 70,
    'https://images.unsplash.com/photo-1608043152269-423dbba4e7e1?w=400',
    1, 1
),
(
    'USB-C Charging Hub',
    '7-in-1 USB-C hub with 4K HDMI, 3x USB 3.0, SD card reader, and 100W PD charging. Compatible with MacBook and Windows laptops.',
    3299.00, 2599.00, 80,
    'https://images.unsplash.com/photo-1625948515291-69613efd103f?w=400',
    1, 1
),
(
    'Mechanical Keyboard',
    'Compact TKL mechanical keyboard with blue switches, RGB backlight, and aluminum frame. Plug-and-play USB connection.',
    6999.00, NULL, 40,
    'https://images.unsplash.com/photo-1587829741301-dc798b83add3?w=400',
    1, 1
),

-- Clothing (CategoryId = 2)
(
    'Men''s Casual T-Shirt',
    '100% combed cotton premium casual t-shirt with reinforced stitching. Breathable, soft, and available in multiple colors. Machine washable.',
    999.00, 699.00, 200,
    'https://images.unsplash.com/photo-1521572163474-6864f9cf17ab?w=400',
    2, 1
),
(
    'Women''s Sneakers',
    'Lightweight and breathable running sneakers with memory foam insole and flexible rubber outsole. Ideal for daily wear and light exercise.',
    4599.00, 3299.00, 80,
    'https://images.unsplash.com/photo-1542291026-7eec264c27ff?w=400',
    2, 1
),
(
    'Denim Jacket',
    'Classic slim-fit denim jacket with button closure, chest pockets, and adjustable cuffs. Versatile enough for casual and semi-formal looks.',
    3999.00, NULL, 60,
    'https://images.unsplash.com/photo-1601333144130-8cbb312386b6?w=400',
    2, 1
),
(
    'Women''s Kurti',
    'Elegant printed cotton kurti with 3/4 sleeves and side slits. Perfect for office wear and casual outings. Available in sizes S to XXL.',
    1799.00, 1299.00, 150,
    'https://images.unsplash.com/photo-1610030469983-98e550d6193c?w=400',
    2, 1
),
(
    'Men''s Chino Pants',
    'Slim-fit chino pants in stretch cotton blend. Features a flat front, side pockets, and back welt pockets. Smart casual look.',
    2499.00, 1899.00, 90,
    'https://images.unsplash.com/photo-1624378439575-d8705ad7ae80?w=400',
    2, 1
),

-- Home & Garden (CategoryId = 3)
(
    'Coffee Maker',
    '12-cup programmable drip coffee maker with built-in grinder, auto-shut off, and brew-strength control. Makes coffee the way you like it.',
    6499.00, 4999.00, 40,
    'https://images.unsplash.com/photo-1495474472287-4d71bcdd2085?w=400',
    3, 1
),
(
    'Throw Pillow Set (4 pcs)',
    'Set of 4 decorative throw pillows with removable, machine-washable velvet covers. Adds a cozy touch to any sofa or bed.',
    1899.00, NULL, 150,
    'https://images.unsplash.com/photo-1586023492125-27b2c045efd7?w=400',
    3, 1
),
(
    'Air Fryer 5L',
    '5-litre digital air fryer with 8 preset cooking modes, 360° rapid air circulation, and non-stick basket. Up to 80% less oil than deep frying.',
    9999.00, 7499.00, 35,
    'https://images.unsplash.com/photo-1648170174685-44dbf47dce6c?w=400',
    3, 1
),
(
    'Scented Candle Set',
    'Set of 6 soy wax scented candles in lavender, vanilla, rose, sandalwood, jasmine, and ocean breeze. 40-hour burn time each.',
    1499.00, 1099.00, 200,
    'https://images.unsplash.com/photo-1603905009427-e4bb42e7ce5d?w=400',
    3, 1
),
(
    'Non-Stick Cookware Set',
    '5-piece non-stick cookware set including frying pan, saucepan, and stockpot with glass lids. Compatible with all cooktops including induction.',
    7499.00, 5999.00, 45,
    'https://images.unsplash.com/photo-1584990347449-a4d1fd4c5fb3?w=400',
    3, 1
),

-- Sports (CategoryId = 4)
(
    'Yoga Mat',
    'Extra thick 6mm non-slip TPE yoga mat with alignment lines and carrying strap. Sweat-resistant, eco-friendly, and easy to clean.',
    2299.00, 1799.00, 120,
    'https://images.unsplash.com/photo-1601925228965-4c0c0fd20e70?w=400',
    4, 1
),
(
    'Insulated Water Bottle',
    'Double-wall stainless steel insulated water bottle. Keeps drinks cold for 24 hours or hot for 12 hours. BPA-free, 750ml capacity.',
    1499.00, NULL, 200,
    'https://images.unsplash.com/photo-1602143407151-7111542de6e8?w=400',
    4, 1
),
(
    'Resistance Bands Set',
    'Set of 5 resistance bands in varying tensions (10–50 lbs). Made of 100% natural latex. Includes carry bag and exercise guide.',
    1299.00, 999.00, 180,
    'https://images.unsplash.com/photo-1598289431512-b97b0917affc?w=400',
    4, 1
),
(
    'Adjustable Dumbbells (Pair)',
    'Space-saving adjustable dumbbell set ranging from 2kg to 10kg per dumbbell. Quick-change dial mechanism. Ideal for home workouts.',
    12999.00, 9999.00, 25,
    'https://images.unsplash.com/photo-1584735935682-2f2b69dff9d2?w=400',
    4, 1
),
(
    'Jump Rope',
    'Speed jump rope with ball-bearing handles and adjustable cable length. Suitable for beginners and professionals. Lightweight and portable.',
    799.00, 599.00, 250,
    'https://images.unsplash.com/photo-1599058917765-a780eda07a3e?w=400',
    4, 1
);
GO

-- ================================================
-- SEED: Sample Orders
-- ================================================
INSERT INTO Orders (CustomerName, Email, Phone, Address, City, ZipCode, TotalAmount, Status, PaymentMethod, OrderDate)
VALUES
    ('Ahmed Raza',      'ahmed.raza@gmail.com',    '0300-1111111', 'House 12, Block A, Gulshan-e-Iqbal', 'Karachi',   '75300', 13998.00, 'Delivered',  'COD',       DATEADD(DAY, -15, GETDATE())),
    ('Sara Khan',       'sara.khan@yahoo.com',     '0321-2222222', 'Flat 5, F-7/2, Street 4',            'Islamabad', '44000', 6999.00,  'Shipped',    'EasyPaisa', DATEADD(DAY, -7,  GETDATE())),
    ('Usman Ali',       'usman.ali@hotmail.com',   '0333-3333333', 'Plot 88, DHA Phase 5',               'Lahore',    '54000', 22498.00, 'Processing', 'Card',      DATEADD(DAY, -3,  GETDATE())),
    ('Fatima Malik',    'fatima.malik@gmail.com',  '0311-4444444', 'House 7, Johar Town',                'Lahore',    '54782', 3798.00,  'Pending',    'COD',       DATEADD(DAY, -1,  GETDATE())),
    ('Bilal Hassan',    'bilal.hassan@gmail.com',  '0345-5555555', 'Shop 3, Saddar Bazaar',              'Karachi',   '74400', 9998.00,  'Pending',    'COD',       GETDATE()),
    ('Ayesha Siddiqui', 'ayesha.s@outlook.com',   '0312-6666666', 'House 44, G-9/3',                   'Islamabad', '44090', 5998.00,  'Delivered',  'Card',      DATEADD(DAY, -20, GETDATE())),
    ('Zara Noor',       'zara.noor@gmail.com',     '0301-7777777', 'Apartment 2B, Clifton Block 4',      'Karachi',   '75600', 7498.00,  'Cancelled',  'EasyPaisa', DATEADD(DAY, -5,  GETDATE()));
GO

-- ================================================
-- SEED: Order Items
-- ================================================

-- Order 1 (Ahmed): Wireless Headphones x2
INSERT INTO OrderItems (OrderId, ProductId, Quantity, UnitPrice) VALUES (1, 1, 2, 6999.00);

-- Order 2 (Sara): Wireless Headphones x1
INSERT INTO OrderItems (OrderId, ProductId, Quantity, UnitPrice) VALUES (2, 1, 1, 6999.00);

-- Order 3 (Usman): Smart Watch x1 + Bluetooth Speaker x1 + Yoga Mat x1
INSERT INTO OrderItems (OrderId, ProductId, Quantity, UnitPrice) VALUES
    (3, 2, 1, 12499.00),
    (3, 4, 1, 3999.00),
    (3, 17, 1, 1799.00);

-- Order 4 (Fatima): Women's Kurti x2 + Jump Rope x1
INSERT INTO OrderItems (OrderId, ProductId, Quantity, UnitPrice) VALUES
    (4, 10, 2, 1299.00),
    (4, 21, 1, 599.00);

-- Order 5 (Bilal): Adjustable Dumbbells x1
INSERT INTO OrderItems (OrderId, ProductId, Quantity, UnitPrice) VALUES (5, 20, 1, 9999.00);

-- Order 6 (Ayesha): Scented Candle Set x2 + Throw Pillow Set x1 + Jump Rope x1
INSERT INTO OrderItems (OrderId, ProductId, Quantity, UnitPrice) VALUES
    (6, 15, 2, 1099.00),
    (6, 13, 1, 1899.00),
    (6, 21, 1, 599.00);

-- Order 7 (Zara - Cancelled): Coffee Maker x1
INSERT INTO OrderItems (OrderId, ProductId, Quantity, UnitPrice) VALUES (7, 12, 1, 4999.00);
GO

-- ================================================
-- VERIFICATION QUERIES
-- ================================================

PRINT '=== Categories ==='
SELECT * FROM Categories;

PRINT '=== Products (Count) ==='
SELECT COUNT(*) AS TotalProducts FROM Products;

PRINT '=== Products with Category ==='
SELECT p.Id, p.Name, c.Name AS Category, p.Price, p.DiscountPrice, p.Stock, p.IsActive
FROM Products p
JOIN Categories c ON p.CategoryId = c.Id
ORDER BY c.Name, p.Name;

PRINT '=== Orders Summary ==='
SELECT o.Id, o.CustomerName, o.City, o.TotalAmount, o.Status, o.PaymentMethod, o.OrderDate
FROM Orders o
ORDER BY o.OrderDate DESC;

PRINT '=== Order Items Detail ==='
SELECT oi.OrderId, o.CustomerName, p.Name AS Product, oi.Quantity, oi.UnitPrice,
       (oi.Quantity * oi.UnitPrice) AS SubTotal
FROM OrderItems oi
JOIN Orders o ON oi.OrderId = o.Id
JOIN Products p ON oi.ProductId = p.Id
ORDER BY oi.OrderId;

PRINT ''
PRINT '✅ ShopEase Database setup complete!'
PRINT '   - 4 Categories'
PRINT '   - 21 Products'
PRINT '   - 7 Sample Orders'
PRINT '   - 13 Order Items'
GO
SELECT * FROM Orders ORDER BY OrderDate DESC;
