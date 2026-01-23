-- Script để tạo database testwefl_ProGress
-- Chạy query này trong SSMS (cần quyền CREATE DATABASE)

-- Kiểm tra database đã tồn tại chưa
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'testwefl_ProGress')
BEGIN
    CREATE DATABASE [testwefl_ProGress];
    PRINT 'Database testwefl_ProGress created successfully.';
END
ELSE
BEGIN
    PRINT 'Database testwefl_ProGress already exists.';
END
GO

