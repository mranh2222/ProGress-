@ModelType Task
@Code
    ViewData("Title") = "Tạo công việc mới"
    Dim technicians = TryCast(ViewBag.Technicians, List(Of Technician))
    If technicians Is Nothing Then
        technicians = New List(Of Technician)
    End If
    Dim saleManagers = TryCast(ViewBag.SaleManagers, List(Of SaleManager))
    If saleManagers Is Nothing Then
        saleManagers = New List(Of SaleManager)
    End If
    Dim software = TryCast(ViewBag.Software, List(Of Software))
    If software Is Nothing Then
        software = New List(Of Software)
    End If
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
        font-weight: 700;
        font-size: 2rem;
        margin-bottom: 0;
    }

    .form-section {
        background: linear-gradient(135deg, #ffffff 0%, #f8f9fa 100%);
        border-radius: 15px;
        padding: 2rem;
        margin-bottom: 2rem;
        border: 2px solid #e5f7ed;
        box-shadow: 0 4px 15px rgba(0, 0, 0, 0.05);
        transition: all 0.3s ease;
    }

    .form-section:hover {
        box-shadow: 0 6px 20px rgba(34, 197, 94, 0.1);
        border-color: #22c55e;
        transform: translateY(-2px);
    }

    .form-section-title {
        color: var(--primary-darker);
        font-weight: 700;
        font-size: 1.1rem;
        margin-bottom: 1.5rem;
        display: flex;
        align-items: center;
        gap: 12px;
        padding-bottom: 1rem;
        border-bottom: 2px solid #e5f7ed;
    }

    .form-section-title i {
        font-size: 1.3rem;
        color: var(--primary-color);
        background: linear-gradient(135deg, #f0fdf4, #e5f7ed);
        padding: 10px;
        border-radius: 10px;
        width: 40px;
        height: 40px;
        display: flex;
        align-items: center;
        justify-content: center;
    }

    .form-label {
        font-weight: 600;
        color: #374151;
        margin-bottom: 0.75rem;
        font-size: 0.95rem;
        display: flex;
        align-items: center;
        gap: 6px;
    }

    .form-label i {
        color: var(--primary-color);
        font-size: 0.9rem;
    }

    .form-control,
    .form-select {
        border: 2px solid #e5e7eb;
        border-radius: 10px;
        padding: 0.75rem 1rem;
        font-size: 0.95rem;
        transition: all 0.3s ease;
        background: white;
    }

    .form-control:focus,
    .form-select:focus {
        border-color: var(--primary-color);
        box-shadow: 0 0 0 4px rgba(34, 197, 94, 0.1);
        outline: none;
        background: #f0fdf4;
    }

    .form-control:hover,
    .form-select:hover {
        border-color: #cbd5e1;
    }

    textarea.form-control {
        min-height: 120px;
        resize: vertical;
    }

    input[type="file"].form-control {
        padding: 0.5rem;
        cursor: pointer;
    }

    input[type="file"].form-control:hover {
        background: #f8f9fa;
    }

    .form-text {
        font-size: 0.85rem;
        color: #6b7280;
        margin-top: 0.5rem;
        display: flex;
        align-items: center;
        gap: 6px;
    }

    .form-text i {
        color: var(--primary-color);
    }

    .btn {
        border-radius: 10px;
        padding: 0.75rem 2rem;
        font-weight: 600;
        font-size: 1rem;
        transition: all 0.3s ease;
        border: none;
    }

    .btn-primary {
        background: linear-gradient(135deg, var(--primary-color), var(--primary-dark));
        box-shadow: 0 4px 15px rgba(34, 197, 94, 0.3);
    }

    .btn-primary:hover {
        background: linear-gradient(135deg, var(--primary-dark), var(--primary-darker));
        box-shadow: 0 6px 20px rgba(34, 197, 94, 0.4);
        transform: translateY(-2px);
    }

    .btn-secondary {
        background: #6b7280;
        box-shadow: 0 4px 15px rgba(107, 114, 128, 0.2);
    }

    .btn-secondary:hover {
        background: #4b5563;
        box-shadow: 0 6px 20px rgba(107, 114, 128, 0.3);
        transform: translateY(-2px);
    }

    .btn-lg {
        padding: 1rem 2.5rem;
        font-size: 1.1rem;
    }

    .action-buttons {
        background: linear-gradient(135deg, #f8f9fa, #ffffff);
        border-radius: 15px;
        padding: 1.5rem 2rem;
        margin-top: 2rem;
        border-top: 3px solid var(--primary-color);
        box-shadow: 0 -4px 15px rgba(0, 0, 0, 0.05);
    }

    .required-field::after {
        content: " *";
        color: #ef4444;
        font-weight: 700;
    }

    @@media (max-width: 768px) {
        .form-wrapper {
            padding: 1.5rem;
        }

        .form-section {
            padding: 1.5rem;
        }
    }
</style>

<div class="container-fluid">
    <div class="row mb-4">
        <div class="col-12 text-center">
            <h1><i class="fas fa-plus-circle me-2"></i>Tạo công việc mới</h1>
            <p class="text-muted mt-2">Điền thông tin để tạo công việc mới trong hệ thống</p>
        </div>
    </div>

    <div class="row">
        <div class="col-xl-10 col-lg-11 mx-auto">
            <div class="form-wrapper">
                @Using Html.BeginForm("Create", "Tasks", FormMethod.Post, New With {.enctype = "multipart/form-data"})
                    @Html.AntiForgeryToken()
                    
                    @<div>
                    @* Thông tin cơ bản *@
                    <div class="form-section">
                        <div class="form-section-title">
                            <i class="fas fa-info-circle"></i>
                            <span>Thông tin cơ bản</span>
                        </div>
                        <div class="row">
                            <div class="col-md-6 mb-4">
                                <label class="form-label">
                                    <i class="fas fa-tag"></i>Tag
                                </label>
                                @Html.TextBoxFor(Function(m) m.Tag, New With {.class = "form-control", .placeholder = "Nhập tag công việc"})
                            </div>
                            <div class="col-md-6 mb-4">
                                <label class="form-label">
                                    <i class="fas fa-calendar-check"></i>Ngày nhận file
                                </label>
                                <input type="date" name="FileReceivedDate" class="form-control" />
                            </div>
                        </div>
                    </div>

                    @* Nền tảng & Sale *@
                    <div class="form-section">
                        <div class="form-section-title">
                            <i class="fas fa-headset"></i>
                            <span>Nền tảng hỗ trợ & Sale quản lý</span>
                        </div>
                        <div class="row">
                            <div class="col-md-6 mb-4">
                                <label class="form-label required-field">
                                    <i class="fas fa-headset"></i>Nền tảng hỗ trợ
                                </label>
                                <select name="SupportPlatform" class="form-select" required>
                                    <option value="0">📱 Zalo</option>
                                    <option value="1">💬 Member Support</option>
                                    <option value="2">👥 Khách liên hệ Sale</option>
                                </select>
                            </div>
                            <div class="col-md-6 mb-4">
                                <label class="form-label required-field">
                                    <i class="fas fa-user-tie"></i>Sale quản lý
                                </label>
                                <select name="SaleManagerId" class="form-select" required>
                                    <option value="">-- Chọn Sale quản lý --</option>
                                    @For Each sale In saleManagers
                                        @<option value="@sale.Id">@sale.Name</option>
                                    Next
                                </select>
                            </div>
                        </div>
                    </div>

                    @* Thông tin khách hàng *@
                    <div class="form-section">
                        <div class="form-section-title">
                            <i class="fas fa-user-tie"></i>
                            <span>Thông tin khách hàng</span>
                        </div>
                        <div class="row">
                            <div class="col-md-6 mb-4">
                                <label class="form-label required-field">
                                    <i class="fas fa-user-tie"></i>Khách hàng
                                </label>
                                @Html.TextBoxFor(Function(m) m.CustomerName, New With {.class = "form-control", .required = "required", .placeholder = "Nhập tên khách hàng"})
                                @Html.ValidationMessageFor(Function(m) m.CustomerName, "", New With {.class = "text-danger"})
                            </div>
                            <div class="col-md-6 mb-4">
                                <label class="form-label required-field">
                                    <i class="fas fa-laptop-code"></i>Phần mềm sử dụng
                                </label>
                                <select name="SoftwareId" class="form-select" id="softwareSelect" required>
                                    <option value="">-- Chọn phần mềm --</option>
                                    @For Each soft In software
                                        @<option value="@soft.Id">@soft.Name</option>
                                    Next
                                </select>
                            </div>
                        </div>
                    </div>

                    @* Nội dung hỗ trợ *@
                    <div class="form-section">
                        <div class="form-section-title">
                            <i class="fas fa-file-alt"></i>
                            <span>Nội dung hỗ trợ</span>
                        </div>
                        <div class="mb-4">
                            <label class="form-label required-field">
                                <i class="fas fa-file-alt"></i>Mô tả lỗi / nội dung hỗ trợ
                            </label>
                            @Html.TextAreaFor(Function(m) m.Description, New With {.class = "form-control", .rows = "6", .required = "required", .placeholder = "Nhập mô tả chi tiết về lỗi hoặc nội dung cần hỗ trợ..."})
                            @Html.ValidationMessageFor(Function(m) m.Description, "", New With {.class = "text-danger"})
                        </div>
                    </div>

                    @* Phân công & Trạng thái *@
                    <div class="form-section">
                        <div class="form-section-title">
                            <i class="fas fa-user-cog"></i>
                            <span>Phân công & Trạng thái</span>
                        </div>
                        <div class="row">
                            <div class="col-md-6 mb-4">
                                <label class="form-label required-field">
                                    <i class="fas fa-user-cog"></i>Kỹ thuật phụ trách
                                </label>
                                <select name="AssignedTo" class="form-select" required>
                                    <option value="">-- Chọn kỹ thuật phụ trách --</option>
                                    @For Each tech In technicians
                                        @<option value="@tech.Id">@tech.Name</option>
                                    Next
                                </select>
                            </div>
                            <div class="col-md-6 mb-4">
                                <label class="form-label required-field">
                                    <i class="fas fa-info-circle"></i>Tình trạng
                                </label>
                                <select name="Status" class="form-select" required>
                                    <option value="0">🟡 Chưa xử lý</option>
                                    <option value="1">🔵 Đang xử lý</option>
                                    <option value="2">🟠 Chờ phản hồi khách hàng</option>
                                    <option value="3">🟢 Đã hoàn thành</option>
                                    <option value="4">🔴 Tạm dừng / quá hạn</option>
                                </select>
                            </div>
                        </div>
                    </div>

                    @* Thời gian *@
                    <div class="form-section">
                        <div class="form-section-title">
                            <i class="fas fa-calendar-alt"></i>
                            <span>Thời gian</span>
                        </div>
                        <div class="row">
                            <div class="col-md-6 mb-4">
                                <label class="form-label">
                                    <i class="fas fa-calendar-alt"></i>Ngày dự kiến hoàn thành
                                </label>
                                <input type="date" name="ExpectedCompletionDate" class="form-control" />
                            </div>
                            <div class="col-md-6 mb-4">
                                <label class="form-label">
                                    <i class="fas fa-check-circle"></i>Ngày thực tế hoàn thành
                                </label>
                                <input type="date" name="CompletedDate" class="form-control" />
                            </div>
                        </div>
                    </div>

                    @* File đính kèm *@
                    <div class="form-section">
                        <div class="form-section-title">
                            <i class="fas fa-paperclip"></i>
                            <span>File đính kèm</span>
                        </div>
                        <div class="row">
                            <div class="col-md-6 mb-3">
                                <label class="form-label">
                                    <i class="fas fa-image me-2"></i>Hình ảnh đính kèm
                                </label>
                                <input type="file" name="uploadedImages" class="form-control" multiple accept="image/*" />
                                <small class="form-text text-muted">
                                    <i class="fas fa-info-circle me-1"></i>Có thể chọn nhiều hình ảnh
                                </small>
                            </div>
                            <div class="col-md-6 mb-3">
                                <label class="form-label">
                                    <i class="fas fa-file me-2"></i>File đính kèm
                                </label>
                                <input type="file" name="uploadedFiles" class="form-control" multiple />
                                <small class="form-text text-muted">
                                    <i class="fas fa-info-circle me-1"></i>Có thể chọn nhiều file (PDF, DOC, LOG, ...)
                                </small>
                            </div>
                        </div>
                    </div>

                    @* Nút hành động *@
                    <div class="action-buttons d-flex justify-content-between align-items-center">
                        <a href="@Url.Action("Index", "Tasks")" class="btn btn-secondary btn-lg">
                            <i class="fas fa-arrow-left me-2"></i>Quay lại
                        </a>
                        <button type="submit" class="btn btn-primary btn-lg">
                            <i class="fas fa-save me-2"></i>Lưu công việc
                        </button>
                    </div>
                </div>
            End Using
            </div>
        </div>
    </div>
</div>