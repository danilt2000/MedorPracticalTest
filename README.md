[![Medor Logo](https://www.medor.cz/wp-content/uploads/2021/11/logo-1.svg)](https://www.medor.cz)

# Medor Practical Test

This project is a solution for the Medor practical test, aimed at implementing a system for retrieving, saving, and managing Bitcoin prices using C# and .NET technologies. The solution covers various aspects, including fetching real-time data, storing it in a database, and performing CRUD operations.

## Technologies Used

![Mediator CQRS Pattern](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/media/microservice-application-layer-implementation-web-api/mediator-cqrs-microservice.png)

- **Framework:** .NET 8
- **Database:** MSSQL with T-SQL scripts for stored procedures
- **Deployment Tools:** Docker and Nginx for hosting and deployment
- **API Documentation:** Swagger is available [here](https://medorbackend.hepatico.ru/swagger/index.html)
- **Frontend:** The live website is accessible [here](https://medor-seven.vercel.app/)
- **Design Pattern:** Mediator and CQRS (Command Query Responsibility Segregation) for handling requests and organizing the application architecture.

## Architecture

![Clean architecture](https://miro.medium.com/max/256/0*cKlf8Eymfs0hu8-2.png)

The project follows a clean architecture approach, ensuring a separation of concerns and making the codebase more maintainable and scalable. The solution was designed to adhere to best practices and coding standards, including:

- [REST best practices](https://www.vinaysahni.com/best-practices-for-a-pragmatic-restful-api?fbclid=IwAR2oTRiYnQI71XZnpRj4DrFHV-TJ5whT-NAGJEqmvENIfbsnXyGHUygQ2K4) for designing pragmatic and efficient APIs.
- [Structured logging](https://code-maze.com/structured-logging-in-asp-net-core-with-serilog/) using Serilog to ensure better traceability and monitoring.
- [SOLID design principles](https://en.wikipedia.org/wiki/HATEOAS#cite_note-Fielding-Ch5-2) for creating a robust and maintainable codebase.

## Features
### 1. Live Data
- The application regularly fetches Bitcoin prices from the [Coindesk API](https://api.coindesk.com/v1/bpi/currentprice.json).
- The prices for BTC/USD and BTC/EUR are converted to CZK using the exchange rates from the public CNB API.
- The retrieved information is displayed in a grid format.
- Provides a button to save the data to the database.

### 2. Saved Data
- Displays previously saved data in a grid format.
- The grid includes an editable column for notes.
- Features buttons for deleting selected records and saving updated entries.

### 3. Bonus Task
- The Bitcoin price is displayed both in a table and graph format, using a library suitable for graph rendering.

## API Endpoints Documentation

### 1. `GET /api/v1/BitcoinPrice/GetCurrentPrice`
- **Description:** Retrieves the current Bitcoin price from an external data source.
- **Response:** 
  - `200 OK`: Returns the current Bitcoin price.
- **Example Response:**
    ```json
    {
        "bitcoinPriceUSD": 26812.53,
        "bitcoinPriceEUR": 25214.78,
        "bitcoinPriceCZK": 586400.95,
        "timestamp": "2024-10-10T12:30:45Z"
    }
    ```

### 2. `GET /api/v1/BitcoinPrice/GetHistoricalData`
- **Description:** Retrieves historical Bitcoin price data starting from a specified date.
- **Query Parameter:**
  - `startDate` (required): The date from which to start retrieving historical data.
- **Response:** 
  - `200 OK`: Returns a list of historical Bitcoin price data.
- **Example Response:**
    ```json
    [
        {
            "id": 1,
            "bitcoinPriceUSD": 26750.12,
            "bitcoinPriceEUR": 25123.55,
            "bitcoinPriceCZK": 585000.00,
            "timestamp": "2024-10-09T14:30:45Z",
            "note": null
        },
        ...
    ]
    ```

### 3. `GET /api/v1/BitcoinPrice/GetSavedData`
- **Description:** Retrieves saved Bitcoin data that has been stored in the system.
- **Response:** 
  - `200 OK`: Returns a list of saved Bitcoin data records.
- **Example Response:**
    ```json
    [
        {
            "id": 1,
            "bitcoinPriceUSD": 26750.12,
            "bitcoinPriceEUR": 25123.55,
            "bitcoinPriceCZK": 585000.00,
            "timestamp": "2024-10-09T14:30:45Z",
            "note": "First entry"
        },
        ...
    ]
    ```

### 4. `POST /api/v1/BitcoinPrice/SaveLiveData`
- **Description:** Saves the current live Bitcoin price data to the system.
- **Request Body:**
    ```json
    {
        "bitcoinPriceUSD": 26812.53,
        "bitcoinPriceEUR": 25214.78,
        "bitcoinPriceCZK": 586400.95,
        "timestamp": "2024-10-10T12:30:45Z",
        "note": "Sample entry"
    }
    ```
- **Response:**
  - `200 OK`: Returns the ID of the saved Bitcoin data record.
- **Example Response:**
    ```json
    {
        "id": 5
    }
    ```

### 5. `PATCH /api/v1/BitcoinPrice/UpdateNote`
- **Description:** Updates a note associated with a saved Bitcoin data record.
- **Request Body:**
    ```json
    {
        "id": 5,
        "note": "Updated note"
    }
    ```
- **Response:**
  - `200 OK`: Returns if the update was successful.

### 6. `DELETE /api/v1/BitcoinPrice/DeleteSavedData`
- **Description:** Deletes a saved Bitcoin data record from the system.
- **Request Body:**
    ```json
    {
        "id": 5
    }
    ```
- **Response:**
  - `200 OK`: Returns if the deletion was successful.

## Deployment
- The application is hosted using Nginx for reverse proxy management and Docker for containerization. It ensures a scalable and isolated environment for the web and API components.
- **API Documentation**: Swagger is used for API documentation and can be accessed [here](https://medorbackend.hepatico.ru/swagger/index.html).

## Database Setup
To set up the database and required stored procedures, execute the following T-SQL scripts:

```sql
USE [MedorPracticalTestDb];
GO

/****** Create Bitcoins Table ******/
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
ALTER PROCEDURE [dbo].[CreateBitcoinsTable]
AS
BEGIN
    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Bitcoins')
    BEGIN
        CREATE TABLE Bitcoins (
            Id INT PRIMARY KEY IDENTITY(1,1),   
            BitcoinPriceUSD DECIMAL(18, 8) NOT NULL,   
            BitcoinPriceEUR DECIMAL(18, 8) NOT NULL,   
            BitcoinPriceCZK DECIMAL(18, 8) NOT NULL,   
            Timestamp DATETIME NOT NULL DEFAULT GETDATE(),
            Note NVARCHAR(255) NULL                
        );
        PRINT 'Table Bitcoins created successfully.';
    END
    ELSE
    BEGIN
        PRINT 'Table Bitcoins already exists.';
    END
END;

USE [MedorPracticalTestDb];
GO

/****** Delete Bitcoin Data ******/
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
ALTER PROCEDURE [dbo].[DeleteBitcoinData]
    @Id INT
AS
BEGIN
    IF EXISTS (SELECT 1 FROM bitcoins WHERE Id = @Id)
    BEGIN
        DELETE FROM bitcoins
        WHERE Id = @Id;
        
        PRINT 'Bitcoin record deleted successfully.';
    END
    ELSE
    BEGIN
        PRINT 'Bitcoin record not found.';
    END
END;

USE [MedorPracticalTestDb];
GO

/****** Get All Bitcoins ******/
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
ALTER PROCEDURE [dbo].[GetAllBitcoins]
AS
BEGIN
    SELECT Id, BitcoinPriceUSD, BitcoinPriceEUR, BitcoinPriceCZK, Timestamp, Note
    FROM Bitcoins;
END;

USE [MedorPracticalTestDb];
GO

/****** Save Bitcoin Data ******/
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
ALTER PROCEDURE [dbo].[SaveBitcoinData]
    @BitcoinPriceUSD DECIMAL(18, 8),
    @BitcoinPriceEUR DECIMAL(18, 8),
    @BitcoinPriceCZK DECIMAL(18, 8),
    @Timestamp DATETIME,
    @Note NVARCHAR(255) = NULL
AS
BEGIN
    INSERT INTO bitcoins (BitcoinPriceUSD, BitcoinPriceEUR, BitcoinPriceCZK, Timestamp, Note)
    VALUES (@BitcoinPriceUSD, @BitcoinPriceEUR, @BitcoinPriceCZK, @Timestamp, @Note);
    
    DECLARE @NewId INT;
    SET @NewId = SCOPE_IDENTITY();
    
    SELECT @NewId AS NewBitcoinDataId;
    
    PRINT 'Bitcoin data saved successfully.';
END;

USE [MedorPracticalTestDb];
GO

/****** Update Bitcoin Note ******/
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
ALTER PROCEDURE [dbo].[UpdateBitcoinNote]
    @Id INT,
    @Note NVARCHAR(255)
AS
BEGIN
    IF EXISTS (SELECT 1 FROM bitcoins WHERE Id = @Id)
    BEGIN
        UPDATE bitcoins
        SET Note = @Note
        WHERE Id = @Id;
        
        PRINT 'Note updated successfully.';
    END
    ELSE
    BEGIN
        PRINT 'Bitcoin record not found.';
    END
END;
```

The above scripts set up the database for storing and managing Bitcoin data, including creating, updating, retrieving, and deleting records.

---

The work was done by Danil Tkachenko Vadimovich 