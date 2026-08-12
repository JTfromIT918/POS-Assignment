CREATE DATABASE FourthWallCafe;

-- Add password to root user
ALTER USER 'root'@'localhost' IDENTIFIED BY 'SQLR0ck$';

USE FourthWallCafe;

CREATE TABLE Category (
  CategoryID INT AUTO_INCREMENT PRIMARY KEY,
  CategoryName VARCHAR(50) NOT NULL
);

CREATE TABLE Item (
  ItemID INT AUTO_INCREMENT PRIMARY KEY,
  CategoryID INT NOT NULL,
  ItemName NVARCHAR(50) NOT NULL,
  ItemDescription NVARCHAR(255) NOT NULL,
  FOREIGN KEY (CategoryID) REFERENCES Category(CategoryID)
);

CREATE TABLE TimeOfDay (
  TimeOfDayID INT AUTO_INCREMENT PRIMARY KEY,
  TimeOfDayName VARCHAR(50) NOT NULL
);

CREATE TABLE ItemPrice (
  ItemPriceID INT AUTO_INCREMENT PRIMARY KEY,
  ItemID INT NOT NULL,
  TimeOfDayID INT NOT NULL,
  Price DECIMAL(5, 2) NOT NULL,
  StartDate DATE NOT NULL,
  EndDate DATE NULL,
  FOREIGN KEY (ItemID) REFERENCES Item(ItemID),
  FOREIGN KEY (TimeOfDayID) REFERENCES TimeOfDay(TimeOfDayID)
);

CREATE TABLE Server (
  ServerID INT AUTO_INCREMENT PRIMARY KEY,
  FirstName NVARCHAR(25) NOT NULL,
  LastName NVARCHAR(25) NOT NULL,
  HireDate DATE NOT NULL,
  TermDate DATE NULL,
  DoB DATE NOT NULL
);

CREATE TABLE PaymentType (
  PaymentTypeID INT AUTO_INCREMENT PRIMARY KEY,
  PaymentTypeName VARCHAR(50) NOT NULL
);

CREATE TABLE CafeOrder (
  OrderID INT AUTO_INCREMENT PRIMARY KEY,
  ServerID INT NULL,
  PaymentTypeID INT NULL,
  OrderDate DATETIME(6) NOT NULL,
  SubTotal DECIMAL(6,2) NULL,
  Tax DECIMAL(6,2) NULL,
  Tip DECIMAL(6,2) NULL,
  AmountDue DECIMAL(6,2) NULL,
  FOREIGN KEY (ServerID) REFERENCES Server(ServerID),
  FOREIGN KEY (PaymentTypeID) REFERENCES PaymentType(PaymentTypeID)
);

CREATE TABLE OrderItem (
  OrderItemID INT AUTO_INCREMENT PRIMARY KEY,
  OrderID INT NOT NULL,
  ItemPriceID INT NOT NULL,
  Quantity TINYINT NOT NULL,
  ExtendedPrice DECIMAL(6,2) NOT NULL DEFAULT(0),
  FOREIGN KEY (OrderID) REFERENCES CafeOrder(OrderID),
  FOREIGN KEY (ItemPriceID) REFERENCES ItemPrice(ItemPriceID)
);

INSERT INTO Category (CategoryName)
VALUES
('Appetizers'),
('Beverages'),
('Wine'),
('Beer'),
('Sandwiches'),
('Soups & Salads'),
('Sides'),
('Pasta'),
('Dessert');

INSERT INTO Item (CategoryID, ItemName, ItemDescription)
VALUES
(1, 'Charcuterie Board', 'Assortment of cured meats, cheese, crackers, and fruits'),
(1, 'Hummus Platter', 'Homemade hummus with veggies and pita bread'),
(1, 'Bruschetta', 'Toasted bread topped with fresh tomatoes, basil, and garlic'),
(2, 'Coffee', 'Freshly brewed coffee'),
(2, 'Hot Chocolate', 'Rich and creamy hot chocolate'),
(2, 'Tea', 'A variety of tea flavors to choose from'),
(3, 'Cabernet Sauvignon', 'Full-bodied red wine with dark fruit flavors'),
(3, 'Chardonnay', 'Rich and buttery white wine'),
(3, 'Pinot Grigio', 'Light and crisp white wine with citrus notes'),
(4, 'IPA', 'Hoppy and bitter beer with a high alcohol content'),
(4, 'Lager', 'Crisp and refreshing beer'),
(4, 'Stout', 'Dark and malty beer with hints of coffee and chocolate'),
(5, 'Turkey Sandwich', 'Turkey breast, lettuce, tomato, and mayo on whole wheat bread'),
(5, 'Grilled Cheese', 'Melted cheddar cheese on grilled sourdough bread'),
(5, 'BLT', 'Bacon, lettuce, tomato, and mayo on toasted white bread'),
(6, 'Chicken Noodle Soup', 'Classic soup with chicken, carrots, celery, and egg noodles'),
(6, 'Caesar Salad', 'Romaine lettuce, croutons, and parmesan cheese in a Caesar dressing'),
(6, 'Garden Salad', 'Mixed greens, cherry tomatoes, cucumber, and red onion in a balsamic vinaigrette'),
(6, 'French Onion Soup', 'Classic soup with caramelized onions, beef broth, and gruyere cheese'),
(7, 'French Fries', 'Crispy and golden brown fries'),
(7, 'Onion Rings', 'Battered and deep-fried onion rings'),
(7, 'Sweet Potato Fries', 'Crispy sweet potato fries with a touch of cinnamon'),
(8, 'Spaghetti and Meatballs', 'Classic Italian dish with spaghetti and meatballs in tomato sauce'),
(8, 'Fettuccine Alfredo', 'Pasta with a creamy Alfredo sauce and parmesan cheese'),
(8, 'Lasagna', 'Layered pasta with tomato sauce, meat, and cheese'),
(9, 'Cheesecake', 'Creamy cheesecake with a graham cracker crust and raspberry sauce'),
(9, 'Chocolate Cake', 'Decadent chocolate cake with chocolate frosting'),
(9, 'Tiramisu', 'Italian dessert made with ladyfingers, coffee, and mascarpone cheese'),
(6, 'Fried Cod Sandwich', 'Fried cod fillet on a bun with tartar sauce and lettuce'),
(7, 'Fruit Salad', 'Fresh seasonal fruits with a sweet dressing'),
(1, 'Pumpkin Spice Latte', 'Coffee beverage with pumpkin spice flavors'),
(3, 'Eggnog Cheesecake', 'Cheesecake with eggnog flavors'),
(9, 'Strawberry Shortcake', 'Dessert item with fresh strawberries');

INSERT INTO TimeOfDay (TimeOfDayName)
VALUES
('Breakfast'),
('Lunch'),
('Happy Hour'),
('Dinner'),
('Seasonal');

INSERT INTO ItemPrice (ItemID, TimeOfDayID, Price, StartDate, EndDate)
VALUES
-- Breakfast
(2, 1, 3.50, '2022-01-01', NULL), -- Coffee
(3, 1, 4.50, '2022-01-01', NULL), -- Hot Chocolate
(4, 1, 3.50, '2022-01-01', NULL), -- Tea
(6, 1, 5.50, '2022-01-01', NULL), -- Caesar Salad
(7, 1, 4.50, '2022-01-01', NULL), -- Garden Salad
-- lunch
(1, 2, 12.50, '2022-01-01', NULL), -- Charcuterie Board
(2, 2, 9.50, '2022-09-01', NULL), -- Hummus Platter
(3, 2, 8.50, '2022-01-01', NULL), -- Bruschetta
(4, 2, 3.50, '2022-01-01', NULL), -- Coffee
(5, 2, 4.50, '2022-01-01', NULL), -- Hot Chocolate
(6, 2, 3.50, '2022-01-01', NULL), -- Tea
(7, 2, 9.50, '2022-01-01', NULL), -- Cabernet Sauvignon
(8, 2, 9.50, '2022-01-01', NULL), -- Chardonnay
(9, 2, 9.50, '2022-01-01', NULL), -- Pinot Grigio
(10, 2, 10.50, '2022-01-01', NULL), -- IPA
(11, 2, 10.50, '2022-01-01', NULL), -- Lager
(12, 2, 10.50, '2022-01-01', NULL), -- Stout
(13, 2, 8.50, '2022-01-01', NULL), -- Turkey Sandwich
(14, 2, 6.50, '2022-01-01', NULL), -- Grilled Cheese
(15, 2, 7.50, '2022-01-01', NULL), -- BLT
(16, 2, 8.50, '2022-01-01', NULL), -- Chicken Noodle Soup
(17, 2, 9.50, '2022-01-01', NULL), -- Caesar Salad
(18, 2, 6.50, '2022-01-01', NULL), -- Garden Salad
(19, 2, 8.50, '2022-01-01', NULL), -- French Onion Soup
(20, 2, 4.50, '2022-01-01', NULL), -- French Fries
(21, 2, 8.50, '2022-01-01', '2022-08-30'), -- Onion Rings
(22, 2, 6.50, '2022-09-01', NULL), -- Sweet Potato Fries
(23, 2, 12.50, '2022-01-01', NULL), -- Spaghetti and Meatballs
(24, 2, 11.50, '2022-01-01', NULL), -- Fettuccine Alfredo
(25, 2, 13.50, '2022-01-01', NULL), -- Lasagna
(26, 2, 7.50, '2022-01-01', NULL), -- Cheesecake
(27, 2, 8.50, '2022-09-01', NULL), -- Chocolate Cake
(28, 2, 9.50, '2022-01-01', NULL), -- Tiramisu
-- Happy Hour
(7, 3, 5.00, '2022-01-01', NULL), -- Cabernet Sauvignon
(8, 3, 5.00, '2022-01-01', NULL), -- Chardonnay
(9, 3, 5.00, '2022-01-01', NULL), -- Pinot Grigio
(10, 3, 3.00, '2022-01-01', NULL), -- IPA
(11, 3, 3.00, '2022-01-01', NULL), -- Lager
(12, 3, 3.00, '2022-01-01', NULL), -- Stout
-- Dinner
(1, 4, 12.50, '2022-01-01', NULL), -- Charcuterie Board
(2, 4, 9.50, '2022-09-01', NULL), -- Hummus Platter
(3, 4, 8.50, '2022-01-01', NULL), -- Bruschetta
(4, 4, 3.50, '2022-01-01', NULL), -- Coffee
(5, 4, 4.50, '2022-01-01', NULL), -- Hot Chocolate
(6, 4, 3.50, '2022-01-01', NULL), -- Tea
(7, 4, 9.50, '2022-01-01', NULL), -- Cabernet Sauvignon
(8, 4, 9.50, '2022-01-01', NULL), -- Chardonnay
(9, 4, 9.50, '2022-01-01', NULL), -- Pinot Grigio
(10, 4, 10.50, '2022-01-01', NULL), -- IPA
(11, 4, 10.50, '2022-01-01', NULL), -- Lager
(12, 4, 10.50, '2022-01-01', NULL), -- Stout
(13, 4, 8.50, '2022-01-01', NULL), -- Turkey Sandwich
(14, 4, 6.50, '2022-01-01', NULL), -- Grilled Cheese
(15, 4, 7.50, '2022-01-01', NULL), -- BLT
(16, 4, 8.50, '2022-01-01', NULL), -- Chicken Noodle Soup
(17, 4, 9.50, '2022-01-01', NULL), -- Caesar Salad
(18, 4, 6.50, '2022-01-01', NULL), -- Garden Salad
(19, 4, 8.50, '2022-01-01', NULL), -- French Onion Soup
(20, 4, 4.50, '2022-01-01', NULL), -- French Fries
(21, 4, 8.50, '2022-01-01', '2022-08-30'), -- Onion Rings
(22, 4, 6.50, '2022-09-01', NULL), -- Sweet Potato Fries
(23, 4, 16.00, '2022-01-01', NULL), -- Spaghetti and Meatballs
(24, 4, 14.00, '2022-01-01', NULL), -- Fettuccine Alfredo
(25, 4, 17.00, '2022-01-01', NULL), -- Lasagna
(26, 4, 7.50, '2022-01-01', NULL), -- Cheesecake
(27, 4, 8.50, '2022-09-01', NULL), -- Chocolate Cake
(28, 4, 9.50, '2022-01-01', NULL), -- Tiramisu
-- Seasonal
(29, 5, 9.50, '2022-02-17', '2022-04-01'), -- Fried Cod Sandwich
(30, 5, 7.50, '2022-05-01', '2022-09-30'), -- Fruit Salad
(31, 5, 5.50, '2022-10-01', '2022-11-30'), -- Pumpkin Spice Latte
(32, 5, 10.50, '2022-12-01', '2022-12-31'), -- Eggnog Cheesecake
(33, 5, 8.50, '2022-03-01', '2022-06-30'), -- Strawberry Shortcake
(29, 5, 10.00, '2023-02-17', '2023-04-01'), -- Fried Cod Sandwich
(30, 5, 8.00, '2023-05-01', '2023-09-30'), -- Fruit Salad
(31, 5, 6.00, '2023-10-01', '2023-11-30'), -- Pumpkin Spice Latte
(32, 5, 11.00, '2023-12-01', '2023-12-31'), -- Eggnog Cheesecake
(33, 5, 9.00, '2023-03-01', '2023-06-30'); -- Strawberry Shortcake

INSERT INTO `Server` (FirstName, LastName, HireDate, TermDate, DoB)
VALUES
('Emma', 'Garcia', '2022-01-01', NULL, '1995-02-12'),
('Jacob', 'Chen', '2022-01-01', NULL, '1994-06-19'),
('Sofia', 'Sato', '2022-01-01', NULL, '1997-09-28'),
('Oliver', 'Kim', '2022-01-01', '2022-04-15', '1992-12-31'),
('Mia', 'Patel', '2022-01-01', NULL, '1998-11-05'),
('William', 'Ali', '2022-01-01', NULL, '1996-01-22'),
('Isabella', 'Rao', '2022-01-01', NULL, '1999-07-08'),
('Ava', 'Ahmad', '2022-05-01', NULL, '1993-04-15'),
('Ethan', 'Nguyen', '2022-06-01', NULL, '2002-08-23'),
('Michael', 'Park', '2022-06-01', NULL, '2003-05-02');

INSERT INTO PaymentType (PaymentTypeName)
VALUES
('Cash'),
('Visa'),
('MasterCard'),
('Apple Pay'),
('PayPal');

UPDATE Item
SET CategoryID = 5
WHERE ItemName = 'Fried Cod Sandwich';

UPDATE Item
SET CategoryID = 6
WHERE ItemName = 'Fruit Salad';

UPDATE Item
SET CategoryID = 2
WHERE ItemName = 'Pumpkin Spice Latte';

UPDATE Item
SET CategoryID = 9
WHERE ItemName = 'Eggnog Cheesecake';