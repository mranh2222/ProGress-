# Hướng dẫn tạo bảng trên Server bằng SQL Server Management Studio (SSMS)

## Bước 1: Mở SQL Server Management Studio (SSMS)

1. Tìm và mở **SQL Server Management Studio** từ Start Menu
2. Nếu chưa cài đặt, tải tại: https://aka.ms/ssmsfullsetup

## Bước 2: Kết nối với Server

1. Trong cửa sổ **"Connect to Server"** hiện ra:
   - **Server type**: Chọn **Database Engine** (mặc định)
   - **Server name**: Nhập `103.216.113.32` hoặc `test.wefly-str.com`
   - **Authentication**: Chọn **SQL Server Authentication**
   - **Login**: Nhập `testwefl_ProGress`
   - **Password**: Nhập `Po27$mQnpOegvu1%`
   - ✅ Đánh dấu **"Remember password"** nếu muốn lưu (tùy chọn)

2. Nhấn nút **"Connect"**

3. Nếu kết nối thành công, bạn sẽ thấy server hiển thị trong **Object Explorer** (bên trái)

## Bước 3: Chọn Database

1. Trong **Object Explorer** (bên trái), mở rộng thư mục **"Databases"** (click vào dấu ▶)
2. Tìm database **`testwefl_ProGress`**
3. **Click chuột phải** vào database `testwefl_ProGress` → Chọn **"New Query"**
   - Hoặc: Chọn database trong **dropdown** ở thanh công cụ phía trên (nơi có chữ "master" hoặc tên database khác)

## Bước 4: Mở file Script SQL

1. Trong cửa sổ Query mới vừa mở, vào menu **File** → **Open** → **File...** (hoặc nhấn **Ctrl+O**)
2. Tìm và chọn file **`DATABASE_SETUP_SERVER.sql`** trong thư mục dự án:
   - Đường dẫn: `ProGress/ProGress/DATABASE_SETUP_SERVER.sql`
3. File script sẽ được mở trong cửa sổ Query

## Bước 5: Chạy Script

1. **Kiểm tra lại** database đã được chọn đúng chưa:
   - Xem trong **dropdown** ở thanh công cụ (góc trên bên trái)
   - Phải là **`testwefl_ProGress`** (không phải "master" hay database khác)

2. Nhấn phím **F5** hoặc click nút **"Execute"** (▶) trên thanh công cụ

3. Đợi script chạy xong (có thể mất vài giây)

## Bước 6: Kiểm tra kết quả

1. Xem tab **"Messages"** ở phía dưới cửa sổ Query
2. Bạn sẽ thấy các thông báo:
   ```
   Table Users created successfully.
   Table Technicians created successfully.
   Table SaleManagers created successfully.
   ...
   Default Technicians data inserted.
   Default SaleManagers data inserted.
   Default Software data inserted.
   Database setup completed successfully!
   ```

3. Nếu có lỗi, sẽ hiển thị trong tab "Messages" với màu đỏ

4. **Kiểm tra bảng đã được tạo:**
   - Trong **Object Explorer**, mở rộng database `testwefl_ProGress`
   - Mở rộng **"Tables"**
   - Bạn sẽ thấy các bảng: Users, Technicians, SaleManagers, Software, Tasks, TaskAttachments, TaskImages, TaskHistory, Questions, QuestionAttachments, QuestionImages

## Lưu ý quan trọng:

- ✅ Script đã có kiểm tra `IF NOT EXISTS`, nên an toàn khi chạy nhiều lần
- ✅ Nếu bảng đã tồn tại, script sẽ bỏ qua và không gây lỗi
- ✅ Script sẽ tự động chèn dữ liệu mẫu (Technicians, SaleManagers, Software) nếu bảng còn trống
- ⚠️ Đảm bảo đã chọn đúng database `testwefl_ProGress` trước khi chạy script

## Xử lý lỗi thường gặp:

### Lỗi: "Cannot connect to server"
- Kiểm tra lại Server name, Login, Password
- Kiểm tra firewall có chặn port 1433 không
- Thử dùng IP `103.216.113.32` thay vì domain

### Lỗi: "Database does not exist"
- Đảm bảo database `testwefl_ProGress` đã được tạo trong Plesk
- Kiểm tra lại tên database có đúng không

### Lỗi: "Login failed"
- Kiểm tra lại username và password
- Đảm bảo user `testwefl_ProGress` có quyền truy cập database

## Sau khi hoàn thành:

Sau khi chạy script thành công, bạn có thể:
1. Kiểm tra dữ liệu: Click chuột phải vào bảng → "Select Top 1000 Rows"
2. Publish ứng dụng lên server và test kết nối database

