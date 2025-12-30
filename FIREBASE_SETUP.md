# Hướng dẫn cấu hình Firebase cho Tech Task Tracker

## Bước 1: Tạo Firebase Project

1. Truy cập https://console.firebase.google.com/
2. Tạo project mới hoặc chọn project có sẵn
3. Vào **Realtime Database** trong menu bên trái
4. Tạo database mới (chọn chế độ Test mode hoặc Production)

## Bước 2: Lấy Database URL

**Cách lấy URL từ Firebase Console:**

1. Từ trang **Project Overview** (trang bạn đang xem), click vào menu **"Build"** ở sidebar bên trái để mở menu
2. Chọn **"Realtime Database"** trong danh sách
3. Nếu chưa có database, click nút **"Create Database"**:
   - Chọn location (gần nhất với bạn, ví dụ: `asia-southeast1`)
   - Chọn chế độ:
     * **"Start in test mode"** ⭐ (Khuyên dùng) - Cho phép đọc/ghi trong 30 ngày, phù hợp để test nhanh
     * **"Start in locked mode"** - Chặn tất cả truy cập, cần cấu hình Rules ngay
     * **"Start in production mode"** - Cần cấu hình Rules ngay
   - Click **"Enable"**
   
   **Lưu ý:** Nếu bạn chọn **"Start in locked mode"**, bạn **PHẢI** cấu hình Rules ngay (xem Bước 4) để ứng dụng có thể hoạt động.
4. Sau khi tạo database, bạn sẽ thấy URL ở **phần trên cùng** của trang, có dạng:
   ```
   https://progress-edbf3-default-rtdb.asia-southeast1.firebasedatabase.app
   ```
   hoặc
   ```
   https://progress-edbf3.firebaseio.com
   ```
5. **Copy toàn bộ URL này** (bao gồm cả `https://` và phần cuối)

## Bước 3: Cấu hình trong Web.config

1. Mở file `ProGress/Web.config`
2. Tìm dòng:
   ```xml
   <add key="FirebaseUrl" value="https://your-project.firebaseio.com" />
   ```
3. Thay thế `https://your-project.firebaseio.com` bằng URL Firebase của bạn

## Bước 4: Cấu hình Rules cho Firebase (QUAN TRỌNG!)

**⚠️ BẮT BUỘC nếu bạn chọn "Start in locked mode" hoặc "Start in production mode"**

1. Vào **Realtime Database > Rules** (tab Rules ở trên cùng)
2. Thay thế toàn bộ nội dung trong ô Rules bằng code sau:

```json
{
  "rules": {
    "tasks": {
      ".read": true,
      ".write": true
    },
    "technicians": {
      ".read": true,
      ".write": true
    },
    "saleManagers": {
      ".read": true,
      ".write": true
    },
    "software": {
      ".read": true,
      ".write": true
    }
  }
}
```

3. Click nút **"Publish"** để lưu Rules

**Giải thích:**
- `.read: true` - Cho phép mọi người đọc dữ liệu
- `.write: true` - Cho phép mọi người ghi dữ liệu
- Phù hợp với yêu cầu "không phân quyền phức tạp, ai vào cũng xem được"

**Lưu ý:** 
- Nếu chọn "Start in test mode", bạn có thể bỏ qua bước này trong 30 ngày đầu
- Nếu chọn "Start in locked mode", bạn **PHẢI** làm bước này ngay, nếu không ứng dụng sẽ không hoạt động
- **Cấu hình Rules HOÀN TOÀN MIỄN PHÍ** - Rules chỉ là cấu hình bảo mật, không tính phí

## Bước 5: Khởi chạy ứng dụng

1. Build và chạy project
2. Truy cập: `https://localhost:44365/` (hoặc port của bạn)
3. Bạn sẽ thấy Dashboard với Kanban board

## Cấu trúc dữ liệu trong Firebase

Dữ liệu sẽ được lưu trữ theo cấu trúc:

```
{
  "tasks": {
    "task-id-1": { ... },
    "task-id-2": { ... }
  },
  "technicians": {
    "tech1": { ... },
    "tech2": { ... }
  }
}
```

## Thông tin về phí Firebase

### ⭐ Quan trọng: Cấu hình Rules KHÔNG MẤT PHÍ

**Cấu hình Rules là HOÀN TOÀN MIỄN PHÍ** - Rules chỉ là cấu hình bảo mật, không tính phí.

### Firebase Realtime Database có 2 gói:

#### 1. **Gói Spark (Miễn phí)** - Bạn đang dùng gói này
- **Lưu trữ:** 1 GB miễn phí
- **Băng thông:** 10 GB/tháng miễn phí
- **Operations:** 100,000 operations/ngày miễn phí
- **Phù hợp:** Ứng dụng nhỏ, team nhỏ, dự án test

#### 2. **Gói Blaze (Trả phí theo sử dụng)**
- **Free tier:** Vẫn có 1 GB lưu trữ + 10 GB băng thông miễn phí/tháng
- **Chỉ trả phí khi vượt quá:** Free tier
- **Phù hợp:** Ứng dụng lớn, nhiều người dùng

### Ước tính cho dự án Tech Task Tracker:

Với dự án quản lý công việc kỹ thuật (team nhỏ):
- **1 task** ≈ 2-5 KB dữ liệu
- **1000 tasks** ≈ 2-5 MB
- **10,000 tasks** ≈ 20-50 MB

→ **Hoàn toàn nằm trong gói miễn phí!** Bạn có thể lưu hàng chục nghìn tasks mà vẫn miễn phí.

### Khi nào cần nâng cấp?

Chỉ khi:
- Lưu trữ > 1 GB
- Băng thông > 10 GB/tháng
- Operations > 100,000/ngày

**Kết luận:** Với dự án này, bạn có thể dùng miễn phí lâu dài, không cần lo về phí! 🎉

## Xử lý lỗi

Nếu gặp lỗi kết nối Firebase:
- Kiểm tra URL trong Web.config
- Kiểm tra Rules trong Firebase Console
- Kiểm tra kết nối internet
- Xem Console trong trình duyệt để biết lỗi chi tiết

