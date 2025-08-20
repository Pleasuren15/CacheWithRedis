CREATE DATABASE PsDevs;
GO

USE PsDevs;
GO

CREATE TABLE Subscriber (
    SubscriberID INT PRIMARY KEY IDENTITY(1,1), -- Auto-incrementing ID
    FullName NVARCHAR(200) NOT NULL,
    Email NVARCHAR(255) NOT NULL,
    SubscriptionDate DATETIME DEFAULT GETDATE()
);
GO

INSERT INTO Subscriber (FullName, Email, SubscriptionDate) VALUES
('Katie Brown', 'katie.brown86@example.com', '2025-08-02 12:18:26'),
('Emily Brown', 'emily.brown61@example.com', '2025-03-15 12:18:26'),
('Chris Brown', 'chris.brown80@example.com', '2025-04-04 12:18:26'),
('John Harris', 'john.harris33@example.com', '2024-12-20 12:18:26'),
('Sophia Jackson', 'sophia.jackson6@example.com', '2024-08-31 12:18:26'),
('David Jackson', 'david.jackson82@example.com', '2024-09-22 12:18:26'),
('Jane Jackson', 'jane.jackson91@example.com', '2025-05-09 12:18:26'),
('Mike Smith', 'mike.smith82@example.com', '2025-04-17 12:18:26'),
('Laura Jackson', 'laura.jackson6@example.com', '2025-05-18 12:18:26'),
('John Martin', 'john.martin98@example.com', '2025-02-23 12:18:26'),
('Jane White', 'jane.white88@example.com', '2025-06-07 12:18:26'),
('Jane Smith', 'jane.smith90@example.com', '2025-04-20 12:18:26'),
('David Martin', 'david.martin37@example.com', '2025-04-07 12:18:26'),
('Mike Thomas', 'mike.thomas50@example.com', '2024-11-05 12:18:26'),
('David Martin', 'david.martin42@example.com', '2024-09-09 12:18:26'),
('David Johnson', 'david.johnson23@example.com', '2025-07-16 12:18:26'),
('Mike Harris', 'mike.harris57@example.com', '2024-12-28 12:18:26'),
('John Brown', 'john.brown52@example.com', '2025-08-13 12:18:26'),
('Chris Brown', 'chris.brown4@example.com', '2024-09-17 12:18:26'),
('Jane Jackson', 'jane.jackson29@example.com', '2024-12-11 12:18:26'),
('Chris Smith', 'chris.smith11@example.com', '2025-07-13 12:18:26'),
('Katie Anderson', 'katie.anderson27@example.com', '2024-10-20 12:18:26'),
('Laura Thomas', 'laura.thomas65@example.com', '2025-07-02 12:18:26'),
('John White', 'john.white57@example.com', '2024-10-03 12:18:26'),
('Sophia Thomas', 'sophia.thomas7@example.com', '2025-07-29 12:18:26'),
('Sophia Brown', 'sophia.brown75@example.com', '2025-06-19 12:18:26'),
('Alex Taylor', 'alex.taylor85@example.com', '2025-04-29 12:18:26'),
('Alex Brown', 'alex.brown16@example.com', '2025-06-10 12:18:26'),
('David Jackson', 'david.jackson22@example.com', '2024-12-28 12:18:26'),
('Mike Anderson', 'mike.anderson40@example.com', '2025-08-12 12:18:26'),
('Laura White', 'laura.white60@example.com', '2025-07-31 12:18:26'),
('John Taylor', 'john.taylor41@example.com', '2025-02-23 12:18:26'),
('Sophia Harris', 'sophia.harris4@example.com', '2025-02-08 12:18:26'),
('Chris Taylor', 'chris.taylor79@example.com', '2025-08-06 12:18:26'),
('Jane Johnson', 'jane.johnson71@example.com', '2024-09-24 12:18:26'),
('Emily Thomas', 'emily.thomas22@example.com', '2025-03-13 12:18:26'),
('Katie Johnson', 'katie.johnson31@example.com', '2024-11-09 12:18:26'),
('Laura Jackson', 'laura.jackson70@example.com', '2024-12-02 12:18:26'),
('Chris Brown', 'chris.brown61@example.com', '2025-05-26 12:18:26'),
('Laura Anderson', 'laura.anderson6@example.com', '2024-12-17 12:18:26'),
('Chris Harris', 'chris.harris76@example.com', '2024-10-26 12:18:26'),
('John Harris', 'john.harris45@example.com', '2025-04-17 12:18:26'),
('Alex Harris', 'alex.harris87@example.com', '2025-05-12 12:18:26'),
('Laura Martin', 'laura.martin58@example.com', '2025-07-17 12:18:26'),
('Emily Harris', 'emily.harris77@example.com', '2025-07-17 12:18:26'),
('Jane Anderson', 'jane.anderson64@example.com', '2025-03-05 12:18:26'),
('Emily Taylor', 'emily.taylor44@example.com', '2024-09-28 12:18:26'),
('Jane Anderson', 'jane.anderson64@example.com', '2025-08-09 12:18:26'),
('Laura Smith', 'laura.smith62@example.com', '2025-03-10 12:18:26'),
('Emily Martin', 'emily.martin15@example.com', '2025-06-28 12:18:26'),
('Katie Smith', 'katie.smith61@example.com', '2025-03-23 12:18:26'),
('Laura White', 'laura.white46@example.com', '2025-08-09 12:18:26'),
('Mike Harris', 'mike.harris67@example.com', '2024-11-25 12:18:26'),
('Sophia White', 'sophia.white91@example.com', '2025-02-16 12:18:26'),
('Sophia Martin', 'sophia.martin77@example.com', '2025-08-04 12:18:26'),
('Sophia Taylor', 'sophia.taylor57@example.com', '2025-01-10 12:18:26'),
('Sophia Thomas', 'sophia.thomas10@example.com', '2024-12-14 12:18:26'),
('Alex Anderson', 'alex.anderson16@example.com', '2025-03-19 12:18:26'),
('Emily White', 'emily.white37@example.com', '2025-06-30 12:18:26'),
('Emily Johnson', 'emily.johnson26@example.com', '2025-02-16 12:18:26'),
('Alex Jackson', 'alex.jackson11@example.com', '2025-01-10 12:18:26'),
('John White', 'john.white95@example.com', '2024-11-10 12:18:26'),
('Emily Thomas', 'emily.thomas100@example.com', '2024-11-24 12:18:26'),
('Alex Smith', 'alex.smith59@example.com', '2024-11-11 12:18:26'),
('Chris Johnson', 'chris.johnson32@example.com', '2024-12-03 12:18:26'),
('Alex Jackson', 'alex.jackson36@example.com', '2024-09-08 12:18:26'),
('Alex Brown', 'alex.brown6@example.com', '2025-07-09 12:18:26'),
('Emily Brown', 'emily.brown53@example.com', '2024-11-12 12:18:26'),
('John Smith', 'john.smith87@example.com', '2024-09-16 12:18:26'),
('Katie Jackson', 'katie.jackson73@example.com', '2024-09-11 12:18:26'),
('Laura Harris', 'laura.harris72@example.com', '2025-05-25 12:18:26'),
('Sophia Anderson', 'sophia.anderson75@example.com', '2025-07-02 12:18:26'),
('Emily Johnson', 'emily.johnson70@example.com', '2025-05-22 12:18:26'),
('Katie White', 'katie.white25@example.com', '2025-05-04 12:18:26'),
('Mike Thomas', 'mike.thomas86@example.com', '2024-11-08 12:18:26'),
('David Anderson', 'david.anderson83@example.com', '2025-06-24 12:18:26'),
('Mike Harris', 'mike.harris85@example.com', '2025-07-27 12:18:26'),
('Alex Taylor', 'alex.taylor6@example.com', '2025-03-09 12:18:26'),
('Chris Brown', 'chris.brown72@example.com', '2025-05-26 12:18:26'),
('Katie Thomas', 'katie.thomas69@example.com', '2025-07-26 12:18:26'),
('Laura White', 'laura.white23@example.com', '2024-11-01 12:18:26'),
('Emily Johnson', 'emily.johnson59@example.com', '2025-04-06 12:18:26'),
('John Thomas', 'john.thomas40@example.com', '2024-08-29 12:18:26'),
('Alex White', 'alex.white55@example.com', '2025-03-26 12:18:26'),
('John Brown', 'john.brown54@example.com', '2024-11-25 12:18:26'),
('Alex Taylor', 'alex.taylor27@example.com', '2025-03-23 12:18:26'),
('Mike Johnson', 'mike.johnson6@example.com', '2025-06-09 12:18:26'),
('Laura Anderson', 'laura.anderson69@example.com', '2024-09-20 12:18:26'),
('John Taylor', 'john.taylor20@example.com', '2025-06-14 12:18:26'),
('Sophia Jackson', 'sophia.jackson2@example.com', '2024-11-07 12:18:26'),
('Laura Jackson', 'laura.jackson48@example.com', '2024-12-06 12:18:26'),
('John Jackson', 'john.jackson4@example.com', '2024-11-01 12:18:26'),
('Emily Brown', 'emily.brown87@example.com', '2025-04-15 12:18:26'),
('Jane Taylor', 'jane.taylor93@example.com', '2025-05-08 12:18:26'),
('Jane Jackson', 'jane.jackson57@example.com', '2025-07-14 12:18:26'),
('Jane White', 'jane.white93@example.com', '2024-08-21 12:18:26'),
('Sophia Jackson', 'sophia.jackson40@example.com', '2024-09-29 12:18:26'),
('Mike Smith', 'mike.smith63@example.com', '2025-04-25 12:18:26'),
('Mike Jackson', 'mike.jackson81@example.com', '2024-09-13 12:18:26'),
('Laura White', 'laura.white83@example.com', '2024-12-25 12:18:26');
GO


CREATE PROCEDURE GetAllSubscribers
AS
BEGIN
    SET NOCOUNT ON;
    SELECT SubscriberID, FullName, Email, SubscriptionDate
    FROM Subscriber;
END;
GO