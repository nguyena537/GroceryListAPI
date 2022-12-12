CREATE TABLE AppUser (
    id INT PRIMARY KEY IDENTITY (1, 1),
    username VARCHAR(30) NOT NULL,
    password VARCHAR(30) NOT NULL,
    firstName VARCHAR(50) NOT NULL,
    lastName VARCHAR(50) NOT NULL,
    email VARCHAR(255) NOT NULL
);

CREATE TABLE UserSetting (
    appUserId INT NOT NULL,
    showCustom BIT NOT NULL,
    darkMode BIT NOT NULL,
    FOREIGN KEY (appUserId) REFERENCES AppUser (id)
);

CREATE TABLE GroceryList (
    id INT PRIMARY KEY IDENTITY (1, 1),
    appUserId INT NOT NULL,
    name VARCHAR(60) NOT NULL,
    showCrossedOff BIT NOT NULL,
    itemsJson VARCHAR(4000) NOT NULL,
    FOREIGN KEY (appUserId) REFERENCES AppUser (id)
);

CREATE TABLE Category (
    id INT PRIMARY KEY IDENTITY (1, 1),
    name VARCHAR(60) NOT NULL,
    photoUrl VARCHAR(255),
    isCustom BIT NOT NULL
);

CREATE TABLE Item (
    id INT PRIMARY KEY IDENTITY (1, 1),
    name VARCHAR(60) NOT NULL,
    unit VARCHAR(50) NOT NULL,
    photoUrl VARCHAR(255),
    categoryId INT,
    FOREIGN KEY (categoryId) REFERENCES Category (id)
);
