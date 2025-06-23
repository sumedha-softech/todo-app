IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'ReactToDoAppDb')
BEGIN
    CREATE DATABASE ReactToDoAppDb;
END
GO

USE ReactToDoAppDb;
GO

-- Create Group table
IF OBJECT_ID('dbo.[Group]', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[Group](
        [GroupId] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [GroupName] NVARCHAR(100) NULL,
        [IsEnableShow] BIT NOT NULL CONSTRAINT DF_Group_IsEnableShow DEFAULT(1),
        [SortBy] NVARCHAR(50) NOT NULL CONSTRAINT DF_Group_SortBy DEFAULT('My order')
    );
END
GO

-- Create Task table
IF OBJECT_ID('dbo.[Task]', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[Task](
        [TaskId] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [Title] NVARCHAR(MAX) NOT NULL,
        [Description] NVARCHAR(MAX) NULL,
        [ToDoDate] DATETIME NULL,
        [CreateDate] DATETIME NOT NULL CONSTRAINT DF_Task_CreateDate DEFAULT(GETDATE()),
        [CompleteDate] DATETIME NULL,
        [IsStarred] BIT NOT NULL,
        [IsCompleted] BIT NOT NULL,
        [TaskGroupId] INT NOT NULL
    );
END

-- Create SubTask table
IF OBJECT_ID('dbo.[SubTask]', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[Task](
       [SubTaskId] [int] IDENTITY(1,1) NOT NULL,
		[Title] [nvarchar](max) NOT NULL,
		[Description] [nvarchar](max) NULL,
		[ToDoDate] [datetime] NULL,
        [CreateDate] DATETIME NOT NULL CONSTRAINT DF_Task_CreateDate DEFAULT(GETDATE()),
        [CompleteDate] [datetime] NULL,
		[IsStarred] [bit] NOT NULL,
		[IsCompleted] [bit] NOT NULL,
		[TaskId] [int] NOT NULL
    );
END
GO