-- 001_InitialMigration.sql  

CREATE TABLE Items (
                       Id INT PRIMARY KEY,
                       Name NVARCHAR(255) NOT NULL,
                       Description NVARCHAR(MAX) NULL,
                       Metadata NVARCHAR(MAX) NULL
);

CREATE TABLE Auctions (
                          Id INT PRIMARY KEY,
                          ItemId INT NOT NULL,
                          CreatedDt DATETIME2 NOT NULL,
                          FinishedDt DATETIME2 NULL,
                          Price DECIMAL(18, 2) NOT NULL,
                          Status INT NOT NULL,
                          Seller NVARCHAR(255) NOT NULL,
                          Buyer NVARCHAR(255) NULL,
                          FOREIGN KEY (ItemId) REFERENCES Items(Id)
);  
