@Code
    Layout = Nothing
End Code

<!DOCTYPE html>
<html>
<head>
    <meta name="viewport" content="width=device-width" />
    <title>Đăng nhập - ProGress</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.2.3/dist/css/bootstrap.min.css" rel="stylesheet">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" />
    <style>
        body { background: #f1f5f9; font-family: 'Segoe UI', sans-serif; display: flex; align-items: center; justify-content: center; height: 100vh; margin: 0; }
        .auth-card { background: white; padding: 2rem; border-radius: 16px; box-shadow: 0 10px 25px rgba(0,0,0,0.05); width: 100%; max-width: 400px; }
        .auth-header { text-align: center; margin-bottom: 2rem; }
        .auth-header h2 { color: #16a34a; font-weight: 700; margin-bottom: 0.5rem; }
        .form-control { border-radius: 10px; padding: 0.75rem 1rem; border: 1px solid #e2e8f0; margin-bottom: 1rem; }
        .btn-primary { background: #16a34a; border: none; border-radius: 10px; padding: 0.75rem; font-weight: 600; width: 100%; transition: all 0.3s; color: white; }
        .btn-primary:hover { background: #15803d; transform: translateY(-2px); }
    </style>
</head>
<body>
    <div class="auth-card">
        <div class="auth-header">
            <h2>Đăng nhập</h2>
            <p class="text-muted small">Chào mừng bạn quay lại hệ thống</p>
        </div>

        @Code
            If TempData("SuccessMessage") IsNot Nothing Then
                @<div class="alert alert-success small py-2">@TempData("SuccessMessage")</div>
            End If

            Using Html.BeginForm("Login", "Account", FormMethod.Post)
                @Html.AntiForgeryToken()
                
                If Not ViewData.ModelState.IsValid Then
                    @<div class="alert alert-danger small py-2">@Html.ValidationSummary(True, "")</div>
                End If
        End Code

                <div class="mb-3">
                    <input type="text" name="username" class="form-control" placeholder="Tên đăng nhập" required />
                </div>

                <div class="mb-3">
                    <input type="password" name="password" class="form-control" placeholder="Mật khẩu" required />
                </div>

                <button type="submit" class="btn btn-primary">Vào hệ thống</button>
                
                <div class="text-center mt-3">
                    <p class="small text-muted">Hệ thống đăng nhập bằng tài khoản nội bộ.</p>
                </div>
        @Code
            End Using
        End Code
    </div>
</body>
</html>
