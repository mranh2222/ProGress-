@ModelType User
@Code
    ViewData("Title") = "Hồ sơ cá nhân"
End Code

<div class="container-fluid">
    <div class="row justify-content-center">
        <div class="col-12 col-lg-8 col-xl-6">
            <div class="card">
                <div class="card-header d-flex align-items-center justify-content-between">
                    <span><i class="fas fa-user me-2"></i>Hồ sơ cá nhân</span>
                </div>
                <div class="card-body">
                    @If Not ViewData.ModelState.IsValid Then
                        @<div class="alert alert-danger">
                            @Html.ValidationSummary(True, "")
                        </div>
                    End If

                    @Using Html.BeginForm("UserProfile", "Account", FormMethod.Post)
                        @Html.AntiForgeryToken()

                        @<div class="mb-3">
                            <label class="form-label">Tên đăng nhập</label>
                            <input type="text" class="form-control" value="@Model.Username" disabled />
                        </div>

                        @<div class="mb-3">
                            <label class="form-label">Họ và tên</label>
                            @Html.TextBoxFor(Function(m) m.FullName, New With {.class = "form-control", .placeholder = "Nhập họ và tên"})
                        </div>

                        @<div class="mb-3">
                            <label class="form-label">Email</label>
                            @Html.TextBoxFor(Function(m) m.Email, New With {.class = "form-control", .placeholder = "Nhập email", .type = "email", .required = "required"})
                            @Html.ValidationMessageFor(Function(m) m.Email, "", New With {.class = "text-danger small"})
                        </div>

                        @<div class="d-flex gap-2">
                            <button type="submit" class="btn btn-primary">
                                <i class="fas fa-save me-1"></i>Lưu thay đổi
                            </button>
                            <a class="btn btn-outline-secondary" href="@Url.Action("Index", "Dashboard")">
                                <i class="fas fa-arrow-left me-1"></i>Quay lại
                            </a>
                        </div>
                    End Using
                </div>
            </div>
        </div>
    </div>
</div>


