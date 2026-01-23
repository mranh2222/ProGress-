@Code
    Layout = Nothing
End Code

<!DOCTYPE html>
<html lang="vi">
<head>
    <meta name="viewport" content="width=device-width" />
    <title>Đăng nhập - ProGress</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.2.3/dist/css/bootstrap.min.css" rel="stylesheet">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" />
    <link rel="preconnect" href="https://fonts.googleapis.com">
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
    <link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700&display=swap" rel="stylesheet">
    <style>
        :root {
            --brand: #16a34a;
            --brand-dark: #15803d;
            --text: #0f172a;
            --muted: #64748b;
            --border: #e2e8f0;
            --bg: #f1f5f9;
        }

        html, body { height: 100%; }
        body {
            font-family: 'Inter', 'Segoe UI', sans-serif;
            background-image:
                linear-gradient(135deg, rgba(15, 23, 42, 0.35) 0%, rgba(15, 23, 42, 0.20) 100%),
                url('@Url.Content("~/Content/Images/background.png")');
            /* Full screen background (cover) */
            background-size: cover, cover;
            background-position: center center, center center;
            background-repeat: no-repeat, no-repeat;
            background-attachment: fixed, fixed;
            display: flex;
            align-items: center;
            justify-content: center;
            margin: 0;
            padding: 1.25rem;
        }

        .auth-shell {
            width: 100%;
            max-width: 400px;
            position: relative;
        }

        /* Dịch form lệch trái trên màn hình lớn, mobile vẫn căn giữa */
        @@media (min-width: 992px) {
            .auth-shell {
                transform: translateX(-220px);
            }
        }

        @@media (min-width: 1400px) {
            .auth-shell {
                transform: translateX(0px);
            }
        }

        .auth-card {
            background: rgba(255, 255, 255, 0.85);
            border: 1px solid rgba(226, 232, 240, 0.9);
            backdrop-filter: blur(10px);
            -webkit-backdrop-filter: blur(10px);
            border-radius: 18px;
            box-shadow: 0 18px 45px rgba(15, 23, 42, 0.10);
            overflow: hidden;
        }

        .auth-top {
            padding: 1.4rem 1.4rem 1rem;
            text-align: center;
        }

        .brand-badge {
            width: 46px;
            height: 46px;
            border-radius: 14px;
            margin: 0 auto 0.9rem;
            display: grid;
            place-items: center;
            background: rgba(255, 255, 255, 0.95);
            box-shadow: 0 10px 25px rgba(15, 23, 42, 0.10);
            overflow: hidden;
        }

        .brand-badge img {
            width: 100%;
            height: 100%;
            object-fit: contain;
            padding: 6px;
        }

        .auth-title {
            color: var(--text);
            font-weight: 800;
            letter-spacing: -0.02em;
            margin: 0 0 0.35rem;
            font-size: 1.55rem;
        }

        .auth-subtitle {
            color: var(--muted);
            font-size: 0.95rem;
            margin: 0;
        }

        .auth-body {
            padding: 0 1.4rem 1.4rem;
        }

        .field {
            margin-bottom: 0.9rem;
        }

        .input-group-text {
            background: #fff;
            border-color: var(--border);
            color: #94a3b8;
            border-radius: 12px;
        }

        .form-control {
            border-color: var(--border);
            border-radius: 12px;
            padding: 0.75rem 0.95rem;
            font-size: 0.95rem;
        }

        .form-control:focus {
            border-color: rgba(22, 163, 74, 0.55);
            box-shadow: 0 0 0 0.25rem rgba(22, 163, 74, 0.15);
        }

        .btn-login {
            width: 100%;
            border: none;
            border-radius: 12px;
            padding: 0.78rem 1rem;
            font-weight: 700;
            background: linear-gradient(135deg, var(--brand), var(--brand-dark));
            box-shadow: 0 12px 24px rgba(22, 163, 74, 0.22);
            transition: transform 0.15s ease, box-shadow 0.15s ease, filter 0.15s ease;
            color: white;
        }

        .btn-login:hover {
            filter: brightness(0.98);
            transform: translateY(-1px);
            box-shadow: 0 14px 30px rgba(22, 163, 74, 0.26);
        }

        .btn-login:active {
            transform: translateY(0);
        }

        .help-text {
            color: #94a3b8;
            font-size: 0.85rem;
            margin: 1rem 0 0;
        }

        .alert {
            border-radius: 12px;
            font-size: 0.9rem;
        }

        /* Reduce motion */
        @@media (prefers-reduced-motion: reduce) {
            .btn-login { transition: none; }
        }
    </style>
</head>
<body>
    <div class="auth-shell">
        <div class="auth-card">
            <div class="auth-top">
                <div class="brand-badge" aria-hidden="true">
                    <img src="@Url.Content("~/Content/Images/logo.png")" alt="KetcauSoft Logo" />
                </div>
                <h1 class="auth-title">Đăng nhập</h1>
                <p class="auth-subtitle">Chào mừng bạn quay lại hệ thống</p>
            </div>

            <div class="auth-body">
                @Code
                    If TempData("SuccessMessage") IsNot Nothing Then
                        @<div class="alert alert-success py-2 mb-3">
                            <i class="fas fa-check-circle me-2"></i>@TempData("SuccessMessage")
                        </div>
                    End If

                    Using Html.BeginForm("Login", "Account", FormMethod.Post, New With {.autocomplete = "on"})
                        @Html.AntiForgeryToken()
                
                        If Not ViewData.ModelState.IsValid Then
                            @<div class="alert alert-danger py-2 mb-3">
                                <i class="fas fa-triangle-exclamation me-2"></i>@Html.ValidationSummary(True, "")
                            </div>
                        End If
                End Code

                        <div class="field">
                            <label class="form-label fw-semibold mb-1" style="color: var(--text);">Tên đăng nhập</label>
                            <div class="input-group">
                                <span class="input-group-text"><i class="fas fa-user"></i></span>
                                <input type="text" name="username" class="form-control" placeholder="Nhập tên đăng nhập" required autocomplete="username" />
                            </div>
                        </div>

                        <div class="field">
                            <label class="form-label fw-semibold mb-1" style="color: var(--text);">Mật khẩu</label>
                            <div class="input-group">
                                <span class="input-group-text"><i class="fas fa-lock"></i></span>
                                <input id="password" type="password" name="password" class="form-control" placeholder="Nhập mật khẩu" required autocomplete="current-password" />
                                <button class="btn btn-outline-secondary" type="button" id="togglePassword" aria-label="Hiện/ẩn mật khẩu" style="border-radius: 12px; border-color: var(--border);">
                                    <i class="fas fa-eye"></i>
                                </button>
                            </div>
                        </div>

                        <button type="submit" class="btn btn-login">
                            <span>Vào hệ thống</span>
                        </button>
                
                        <p class="text-center help-text">Hệ thống đăng nhập bằng tài khoản nội bộ.</p>
                @Code
                    End Using
                End Code
            </div>
        </div>
    </div>

    <script>
        (function () {
            var btn = document.getElementById('togglePassword');
            var input = document.getElementById('password');
            if (!btn || !input) return;
            btn.addEventListener('click', function () {
                var isHidden = input.type === 'password';
                input.type = isHidden ? 'text' : 'password';
                var icon = btn.querySelector('i');
                if (icon) {
                    icon.classList.toggle('fa-eye', !isHidden);
                    icon.classList.toggle('fa-eye-slash', isHidden);
                }
            });
        })();
    </script>
</body>
</html>
