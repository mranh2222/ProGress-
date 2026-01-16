@ModelType Technician
@Code
    ViewData("Title") = "Thêm kỹ thuật viên mới"
End Code

<style>
    body {
        background: linear-gradient(135deg, #f0fdf4 0%, #ffffff 100%);
        min-height: 100vh;
    }

    .form-wrapper {
        background: white;
        border-radius: 20px;
        box-shadow: 0 10px 40px rgba(34, 197, 94, 0.15);
        padding: 2.5rem;
        margin-top: 1rem;
    }

    h1 {
        color: var(--primary-darker);
        font-weight: 600;
        font-size: 1.2rem;
        margin-bottom: 0.5rem;
    }

    .form-section {
        background: #f8f9fa;
        border-radius: 8px;
        padding: 1rem;
        margin-bottom: 1rem;
        border: 1px solid #e5e7eb;
    }

    .form-label {
        font-weight: 500;
        color: #374151;
        margin-bottom: 0.4rem;
        font-size: 0.8rem;
    }

    .form-control {
        border: 1px solid #d1d5db;
        border-radius: 6px;
        padding: 0.4rem 0.65rem;
        font-size: 0.8rem;
        transition: all 0.2s ease;
    }

    .form-control:focus {
        border-color: var(--primary-color);
        box-shadow: 0 0 0 4px rgba(34, 197, 94, 0.1);
        outline: none;
        background: #f0fdf4;
    }

    .btn {
        border-radius: 6px;
        padding: 0.4rem 1rem;
        font-weight: 500;
        font-size: 0.8rem;
    }

    .btn-primary {
        background: linear-gradient(135deg, var(--primary-color), var(--primary-dark));
        box-shadow: 0 4px 15px rgba(34, 197, 94, 0.3);
    }

    .btn-primary:hover {
        background: linear-gradient(135deg, var(--primary-dark), var(--primary-darker));
        transform: translateY(-2px);
    }

    .required-field::after {
        content: " *";
        color: #ef4444;
        font-weight: 700;
    }
</style>

<div class="container-fluid">
    <div class="row mb-4">
        <div class="col-12 text-center">
            <h1><i class="fas fa-user-plus me-2"></i>Thêm kỹ thuật viên mới</h1>
            <p class="text-muted mt-2">Điền thông tin để thêm kỹ thuật viên mới vào hệ thống</p>
        </div>
    </div>

    <div class="row">
        <div class="col-xl-8 col-lg-10 mx-auto">
            <div class="form-wrapper">
                @If TempData("SuccessMessage") IsNot Nothing Then
                    @<div class="alert alert-success alert-dismissible fade show" role="alert">
                        <i class="fas fa-check-circle me-2"></i>@TempData("SuccessMessage")
                        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                    </div>
                End If
                
                @If Not ViewData.ModelState.IsValid Then
                    @<div class="alert alert-danger">
                        @Html.ValidationSummary(True, "", New With {.class = "text-danger"})
                    </div>
                End If
                
                @Using Html.BeginForm("Create", "Technicians", FormMethod.Post)
                    @Html.AntiForgeryToken()
                    
                    @<div>
                        <div class="form-section">
                            <h5 class="mb-4" style="color: var(--primary-darker); font-weight: 700;">
                                <i class="fas fa-user me-2"></i>Thông tin kỹ thuật viên
                            </h5>
                            
                            <div class="mb-4">
                                <label class="form-label required-field">Tên kỹ thuật viên</label>
                                @Html.TextBoxFor(Function(m) m.Name, New With {.class = "form-control", .required = "required", .placeholder = "Nhập tên kỹ thuật viên"})
                                @Html.ValidationMessageFor(Function(m) m.Name, "", New With {.class = "text-danger"})
                            </div>

                            <div class="mb-4">
                                <label class="form-label">
                                    <i class="fas fa-envelope me-2"></i>Email
                                </label>
                                @Html.TextBoxFor(Function(m) m.Email, New With {.class = "form-control", .type = "email", .placeholder = "Nhập email (tùy chọn)"})
                                @Html.ValidationMessageFor(Function(m) m.Email, "", New With {.class = "text-danger"})
                            </div>

                            <div class="mb-4">
                                <label class="form-label">
                                    <i class="fas fa-phone me-2"></i>Số điện thoại
                                </label>
                                @Html.TextBoxFor(Function(m) m.Phone, New With {.class = "form-control", .placeholder = "Nhập số điện thoại (tùy chọn)"})
                                @Html.ValidationMessageFor(Function(m) m.Phone, "", New With {.class = "text-danger"})
                            </div>
                        </div>

                        <div class="d-flex justify-content-between align-items-center mt-4 pt-3 border-top">
                            <a href="@Url.Action("Index", "Dashboard")" class="btn btn-secondary">
                                <i class="fas fa-arrow-left me-2"></i>Quay lại
                            </a>
                            <button type="submit" class="btn btn-primary">
                                <i class="fas fa-save me-2"></i>Lưu kỹ thuật viên
                            </button>
                        </div>
                    </div>
                End Using
            </div>
        </div>
    </div>
</div>

