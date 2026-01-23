Imports System.Data.SqlClient
Imports System.IO
Imports System.Configuration
Imports System.Web.Mvc

Public Class SetupController
    Inherits Controller

    ''' <summary>
    ''' Đường dẫn: /Setup
    ''' Tự động tạo các bảng trong database dựa trên file DATABASE_SETUP.sql
    ''' </summary>
    Async Function Index() As Threading.Tasks.Task(Of ActionResult)
        Try
            ' Lấy chuỗi kết nối đang hoạt động (DefaultConnection)
            Dim connectionString = ConfigurationManager.ConnectionStrings("DefaultConnection").ConnectionString
            
            ' Tìm file SQL ở thư mục gốc của Solution (đi lên 1 cấp từ thư mục Web)
            Dim sqlFilePath = Server.MapPath("~/../DATABASE_SETUP.sql")
            
            ' Nếu không thấy ở trên, thử tìm ở ngay trong thư mục Web
            If Not System.IO.File.Exists(sqlFilePath) Then
                sqlFilePath = Server.MapPath("~/DATABASE_SETUP.sql")
            End If

            If Not System.IO.File.Exists(sqlFilePath) Then
                Return Content("Không tìm thấy file DATABASE_SETUP.sql. Vui lòng kiểm tra lại đường dẫn file.")
            End If

            Dim script = System.IO.File.ReadAllText(sqlFilePath)
            
            Using conn As New SqlConnection(connectionString)
                Await conn.OpenAsync()
                
                ' Tách script theo dấu chấm phẩy để chạy từng lệnh (tránh lỗi khóa ngoại do chạy sai thứ tự)
                ' Lưu ý: Script mới mình đã viết có IF NOT EXISTS nên rất an toàn
                Dim commands = script.Split(New String() {"GO" & vbCrLf, "GO" & vbLf, "GO "}, StringSplitOptions.RemoveEmptyEntries)
                
                ' Nếu không dùng GO, ta chạy nguyên khối (vì script mới đã tối ưu thứ tự)
                Using cmd As New SqlCommand(script, conn)
                    Await cmd.ExecuteNonQueryAsync()
                End Using

                Return Content("<h2 style='color:green'>Khởi tạo Database trên Server thành công!</h2><p>Các bảng đã được tạo và dữ liệu mẫu đã được chèn.</p><a href='/'>Quay lại trang chủ</a>", "text/html")
            End Using

        Catch ex As Exception
            Return Content("<h2 style='color:red'>Lỗi khi khởi tạo Database:</h2><p>" & ex.Message & "</p>", "text/html")
        End Try
    End Function
End Class

