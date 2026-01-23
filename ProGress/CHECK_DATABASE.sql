-- Script để kiểm tra database có tồn tại không
-- Chạy query này trong SSMS để xem danh sách tất cả databases

SELECT name 
FROM sys.databases 
WHERE name LIKE '%ProGress%' OR name LIKE '%testwefl%'
ORDER BY name;

-- Hoặc xem tất cả databases:
-- SELECT name FROM sys.databases ORDER BY name;

