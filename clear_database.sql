-- Clear Database Script for Q-itDB
-- Run this in SQL Server Management Studio or via sqlcmd

USE Q-itDB;
GO

-- Disable all foreign key constraints
EXEC sp_MSForEachTable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL';
GO

-- Delete all data from all tables
EXEC sp_MSForEachTable 'DELETE FROM ?';
GO

-- Reset identity columns (serial numbers) for all tables
EXEC sp_MSForEachTable '
    IF OBJECTPROPERTY(OBJECT_ID(''?''), ''TableHasIdentity'') = 1
    DBCC CHECKIDENT (''?'', RESEED, 0)
';
GO

-- Re-enable all foreign key constraints
EXEC sp_MSForEachTable 'ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL';
GO

PRINT 'Database cleared successfully!';
