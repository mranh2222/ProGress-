@ModelType Task
@Code
    ViewData("Title") = "Chi tiết công việc"
End Code

<style>
    .details-page {
        padding: 1rem;
    }
    
    .page-header {
        background: white;
        border-radius: 8px;
        padding: 0.75rem 1rem;
        margin-bottom: 1rem;
        box-shadow: 0 1px 3px rgba(0,0,0,0.1);
        display: flex;
        justify-content: space-between;
        align-items: center;
    }
    
    .page-header h1 {
        font-size: 1.1rem !important;
        font-weight: 600;
        margin: 0;
    }
    
    .info-grid {
        display: grid;
        grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
        gap: 0.75rem;
        margin-bottom: 0.75rem;
    }
    
    .info-item {
        display: flex;
        flex-direction: column;
    }
    
    .info-label {
        font-size: 0.75rem;
        color: #6b7280;
        font-weight: 500;
        margin-bottom: 0.25rem;
    }
    
    .info-value {
        font-size: 0.85rem;
        color: #1f2937;
        font-weight: 500;
    }
    
    .card {
        border: 1px solid #e5e7eb;
        border-radius: 8px;
        box-shadow: 0 1px 3px rgba(0,0,0,0.05);
        margin-bottom: 1rem;
    }
    
    .card-header {
        font-size: 0.85rem;
        padding: 0.625rem 0.875rem;
        background: #f9fafb;
        border-bottom: 1px solid #e5e7eb;
        font-weight: 600;
    }
    
    .card-body {
        font-size: 0.85rem;
        padding: 0.875rem;
    }
    
    .btn {
        font-size: 0.75rem;
        padding: 0.35rem 0.7rem;
    }
    
    .badge {
        font-size: 0.7rem;
        padding: 0.2rem 0.4rem;
    }
    
    .status-badge {
        font-size: 0.7rem;
        padding: 0.2rem 0.4rem;
    }
    
    .content-section {
        background: #f9fafb;
        border-left: 3px solid;
        padding: 0.75rem;
        border-radius: 4px;
        margin-bottom: 0.75rem;
    }
    
    .content-section h6 {
        font-size: 0.8rem;
        font-weight: 600;
        margin-bottom: 0.5rem;
    }
    
    .content-section p,
    .content-section div {
        font-size: 0.85rem;
        margin: 0;
        line-height: 1.5;
    }
    
    .timeline-item {
        padding: 0.5rem 0;
        border-bottom: 1px solid #e5e7eb;
    }
    
    .timeline-item:last-child {
        border-bottom: none;
    }
    
    .timeline-item strong {
        font-size: 0.8rem;
    }
    
    .timeline-item p {
        font-size: 0.75rem;
        margin: 0.25rem 0;
    }
    
    .timeline-item small {
        font-size: 0.7rem;
    }
    
    /* Image Lightbox */
    .image-zoom {
        transition: transform 0.2s ease;
    }
    
    .image-zoom:hover {
        transform: scale(1.05);
        opacity: 0.9;
    }
    
    .image-lightbox {
        display: none;
        position: fixed;
        z-index: 9999;
        left: 0;
        top: 0;
        width: 100%;
        height: 100%;
        background-color: rgba(0, 0, 0, 0.9);
        overflow: auto;
        align-items: flex-start;
        justify-content: center;
        padding-top: 5%;
    }
    
    .image-lightbox-content {
        position: relative;
        margin: 0 auto;
        padding: 20px;
        width: 90%;
        max-width: 1200px;
        display: flex;
        align-items: center;
        justify-content: center;
    }
    
    .image-lightbox img {
        width: 100%;
        max-height: 85vh;
        height: auto;
        object-fit: contain;
        border-radius: 8px;
        box-shadow: 0 4px 20px rgba(0, 0, 0, 0.5);
    }
    
    .image-lightbox-close {
        position: absolute;
        top: 20px;
        right: 40px;
        color: white;
        font-size: 2rem;
        font-weight: bold;
        cursor: pointer;
        z-index: 10000;
        background: rgba(0, 0, 0, 0.5);
        width: 40px;
        height: 40px;
        border-radius: 50%;
        display: flex;
        align-items: center;
        justify-content: center;
        transition: all 0.3s ease;
    }
    
    .image-lightbox-close:hover {
        background: rgba(0, 0, 0, 0.8);
        transform: rotate(90deg);
    }
</style>

<div class="container-fluid details-page">
    <div class="page-header">
        <h1><i class="fas fa-info-circle me-2"></i>Chi tiết công việc</h1>
        <div class="d-flex gap-2">
            <a href="@Url.Action("Edit", "Tasks", New With {.id = Model.Id})" class="btn btn-warning btn-sm">
                <i class="fas fa-edit me-1"></i>Chỉnh sửa
            </a>
            <a href="@Url.Action("Index", "Tasks")" class="btn btn-secondary btn-sm">
                <i class="fas fa-arrow-left me-1"></i>Quay lại
            </a>
        </div>
    </div>

    <div class="row g-3">
        <div class="col-xl-8 col-lg-7">
            <div class="card">
                <div class="card-header" style="color: #000000;">
                    <span style="color: #000000; font-weight: 600;">Câu hỏi / Mô tả lỗi</span>
                </div>
                <div class="card-body">
                    <div class="content-section" style="border-left-color: #3b82f6;">
                        @If Not String.IsNullOrEmpty(Model.Description) Then
                            @<div style="word-wrap: break-word; line-height: 1.6; margin-bottom: 1rem;">@Html.Raw(Model.Description)</div>
                        Else
                            @<p class="text-muted mb-3">Chưa có mô tả</p>
                        End If
                        
                        @* Hiển thị ảnh đính kèm của câu hỏi *@
                        @If Model.Images IsNot Nothing AndAlso Model.Images.Any() Then
                            @<div class="mt-3">
                                <h6 class="small mb-2" style="font-size: 0.75rem; font-weight: 600; color: #6b7280;">
                                    <i class="fas fa-image me-1"></i>Hình ảnh đính kèm
                                </h6>
                                <div class="row g-2">
                                    @For Each img In Model.Images
                                        @<div class="col-4 col-md-3">
                                            <a href="#" class="d-block image-zoom" data-image-src="@Url.Action("Preview", "File", New With {.filePath = img})" onclick="return false;">
                                                <img src="@Url.Action("Preview", "File", New With {.filePath = img})" class="img-fluid rounded" style="height: 100px; width: 100%; object-fit: cover; cursor: pointer; border: 1px solid #e5e7eb;" alt="Hình ảnh" />
                                            </a>
                                        </div>
                                    Next
                                </div>
                            </div>
                        End If
                        
                        @* Hiển thị file đính kèm của câu hỏi *@
                        @If Model.Attachments IsNot Nothing AndAlso Model.Attachments.Any() Then
                            @<div class="mt-3">
                                <h6 class="small mb-2" style="font-size: 0.75rem; font-weight: 600; color: #6b7280;">
                                    <i class="fas fa-file me-1"></i>File đính kèm
                                </h6>
                                <div class="d-flex flex-wrap gap-2">
                                    @For Each att In Model.Attachments
                                        @<a href="@Url.Action("Download", "File", New With {.filePath = att})" target="_blank" class="btn btn-outline-primary btn-sm" style="font-size: 0.75rem;">
                                            <i class="fas fa-download me-1"></i>@System.IO.Path.GetFileName(att.Split("?"c)(0))
                                        </a>
                                    Next
                                </div>
                            </div>
                        End If
                    </div>
                </div>
            </div>

            <div class="card">
                <div class="card-header" style="color: #000000;">
                    <span style="color: #000000; font-weight: 600;">Phản hồi khách hàng</span>
                </div>
                <div class="card-body">
                    @If Not String.IsNullOrEmpty(Model.ResponseToCustomer) Then
                        @<div class="content-section" style="border-left-color: #3b82f6;">
                            <div style="word-wrap: break-word; line-height: 1.6; margin-bottom: 1rem;">@Html.Raw(Model.ResponseToCustomer)</div>
                            
                            @* Hiển thị ảnh đính kèm của phản hồi *@
                            @If Model.ResponseImages IsNot Nothing AndAlso Model.ResponseImages.Any() Then
                                @<div class="mt-3">
                                    <h6 class="small mb-2" style="font-size: 0.75rem; font-weight: 600; color: #6b7280;">
                                        <i class="fas fa-image me-1"></i>Hình ảnh đính kèm
                                    </h6>
                                    <div class="row g-2">
                                        @For Each img In Model.ResponseImages
                                            @<div class="col-4 col-md-3">
                                                <a href="#" class="d-block image-zoom" data-image-src="@Url.Action("Preview", "File", New With {.filePath = img})" onclick="return false;">
                                                    <img src="@Url.Action("Preview", "File", New With {.filePath = img})" class="img-fluid rounded" style="height: 100px; width: 100%; object-fit: cover; cursor: pointer; border: 1px solid #e5e7eb;" alt="Hình ảnh phản hồi" />
                                                </a>
                                            </div>
                                        Next
                                    </div>
                                </div>
                            End If
                            
                            @* Hiển thị file đính kèm của phản hồi *@
                            @If Model.ResponseAttachments IsNot Nothing AndAlso Model.ResponseAttachments.Any() Then
                                @<div class="mt-3">
                                    <h6 class="small mb-2" style="font-size: 0.75rem; font-weight: 600; color: #6b7280;">
                                        <i class="fas fa-file me-1"></i>File đính kèm
                                    </h6>
                                    <div class="d-flex flex-wrap gap-2">
                                        @For Each att In Model.ResponseAttachments
                                            @<a href="@Url.Action("Download", "File", New With {.filePath = att})" target="_blank" class="btn btn-outline-primary btn-sm" style="font-size: 0.75rem;">
                                                <i class="fas fa-download me-1"></i>@System.IO.Path.GetFileName(att.Split("?"c)(0))
                                            </a>
                                        Next
                                    </div>
                                </div>
                            End If
                        </div>
                    Else
                        @<div class="text-center text-muted py-3">
                            <i class="fas fa-inbox fa-lg mb-2"></i>
                            <p class="mb-0" style="font-size: 0.8rem;">Chưa có phản hồi</p>
                        </div>
                    End If
                    
                    @If Not String.IsNullOrEmpty(Model.Solution) Then
                        @<div class="content-section mt-3" style="border-left-color: #10b981;">
                            <h6><i class="fas fa-wrench me-1"></i>Giải pháp</h6>
                            <p style="white-space: pre-wrap; word-wrap: break-word; margin: 0;">@Model.Solution</p>
                        </div>
                    End If
                </div>
            </div>

            @If Model.History IsNot Nothing AndAlso Model.History.Any() Then
                @<div class="card">
                    <div class="card-header">Lịch sử cập nhật</div>
                    <div class="card-body">
                        @For Each history In Model.History.OrderByDescending(Function(h) h.ChangedDate).ToList()
                            @<div class="timeline-item">
                                <div class="d-flex justify-content-between align-items-start">
                                    <div class="flex-grow-1">
                                        <strong>@history.Action</strong>
                                        @If Not String.IsNullOrEmpty(history.Description) Then
                                            @<p>@history.Description</p>
                                        End If
                                        @If Not String.IsNullOrEmpty(history.OldValue) OrElse Not String.IsNullOrEmpty(history.NewValue) Then
                                            @<small class="text-muted">
                                                @If Not String.IsNullOrEmpty(history.OldValue) Then
                                                    @<span>@history.OldValue</span>
                                                End If
                                                @If Not String.IsNullOrEmpty(history.NewValue) Then
                                                    @<span> → @history.NewValue</span>
                                                End If
                                            </small>
                                        End If
                                    </div>
                                    <div class="text-end ms-2">
                                        <small class="text-muted d-block">@history.ChangedByName</small>
                                        <small class="text-muted">@history.ChangedDate.ToString("dd/MM/yyyy HH:mm")</small>
                                    </div>
                                </div>
                            </div>
                        Next
                    </div>
                </div>
            End If
        </div>

        <div class="col-xl-4 col-lg-5">
            <div class="card">
                <div class="card-header">Thao tác nhanh</div>
                <div class="card-body">
                    <div class="d-grid gap-2">
                        <button type="button" class="btn btn-primary btn-sm w-100" data-bs-toggle="modal" data-bs-target="#replyModal">
                            <i class="fas fa-reply me-1"></i>Trả lời khách hàng
                        </button>
                        <a href="@Url.Action("Edit", "Tasks", New With {.id = Model.Id})" class="btn btn-outline-primary btn-sm w-100">
                            <i class="fas fa-edit me-1"></i>Chỉnh sửa
                        </a>
                        @Using Html.BeginForm("Delete", "Tasks", FormMethod.Post, New With {.style = "display: inline-block; width: 100%;", .onsubmit = "return confirm('Bạn có chắc chắn muốn xóa công việc này?');"})
                            @Html.AntiForgeryToken()
                            @Html.Hidden("id", Model.Id)
                            @<button type="submit" class="btn btn-outline-danger btn-sm w-100">
                                <i class="fas fa-trash me-1"></i>Xóa
                            </button>
                        End Using
                        <a href="@Url.Action("Index", "Tasks")" class="btn btn-outline-secondary btn-sm w-100">
                            <i class="fas fa-list me-1"></i>Danh sách
                        </a>
                        <a href="@Url.Action("Index", "Dashboard")" class="btn btn-outline-secondary btn-sm w-100">
                            <i class="fas fa-home me-1"></i>Dashboard
                        </a>
                        <a href="@Url.Action("Index", "Tasks")" class="btn btn-outline-secondary btn-sm w-100">
                            <i class="fas fa-arrow-left me-1"></i>Quay lại
                        </a>
                    </div>
                </div>
            </div>

            <div class="card">
                <div class="card-header">Thông tin cơ bản</div>
                <div class="card-body">
                    <div class="info-grid">
                        <div class="info-item">
                            <span class="info-label">Tag</span>
                            <span class="info-value">
                                @If Not String.IsNullOrEmpty(Model.Tag) Then
                                    @<span class="badge bg-secondary">@Model.Tag</span>
                                Else
                                    @<span class="text-muted">-</span>
                                End If
                            </span>
                        </div>
                        <div class="info-item">
                            <span class="info-label">Khách hàng</span>
                            <span class="info-value">@Model.CustomerName</span>
                        </div>
                        <div class="info-item">
                            <span class="info-label">Kỹ thuật phụ trách</span>
                            <span class="info-value">@Model.AssignedToName</span>
                        </div>
                        <div class="info-item">
                            <span class="info-label">Tình trạng</span>
                            <span class="info-value">
                                @Select Case Model.Status
                                    Case TaskStatus.Pending
                                        @<span class="status-badge status-pending">🟡 Chưa xử lý</span>
                                    Case TaskStatus.InProgress
                                        @<span class="status-badge status-inprogress">🔵 Đang xử lý</span>
                                    Case TaskStatus.Waiting
                                        @<span class="status-badge status-waiting">🟠 Chờ phản hồi</span>
                                    Case TaskStatus.Completed
                                        @<span class="status-badge status-completed">🟢 Đã hoàn thành</span>
                                    Case TaskStatus.Paused
                                        @<span class="status-badge status-paused">🔴 Quá hạn</span>
                                End Select
                            </span>
                        </div>
                        <div class="info-item">
                            <span class="info-label">Phần mềm</span>
                            <span class="info-value">
                                @If Not String.IsNullOrEmpty(Model.SoftwareName) Then
                                    @<span class="badge bg-info">@Model.SoftwareName</span>
                                Else
                                    @<span class="text-muted">-</span>
                                End If
                            </span>
                        </div>
                        <div class="info-item">
                            <span class="info-label">Nền tảng hỗ trợ</span>
                            <span class="info-value">
                                @Select Case Model.SupportPlatform
                                    Case SupportPlatform.Zalo
                                        @<span>Zalo</span>
                                    Case SupportPlatform.MemberSupport
                                        @<span>Member Support</span>
                                    Case SupportPlatform.CustomerContactSale
                                        @<span>Khách liên hệ Sale</span>
                                    Case Else
                                        @<span class="text-muted">-</span>
                                End Select
                            </span>
                        </div>
                        <div class="info-item">
                            <span class="info-label">Sale quản lý</span>
                            <span class="info-value">
                                @If Not String.IsNullOrEmpty(Model.SaleManagerName) Then
                                    @Model.SaleManagerName
                                Else
                                    @<span class="text-muted">-</span>
                                End If
                            </span>
                        </div>
                        <div class="info-item">
                            <span class="info-label">Ngày nhận file</span>
                            <span class="info-value">
                                @If Model.FileReceivedDate.HasValue Then
                                    @Model.FileReceivedDate.Value.ToString("dd/MM/yyyy")
                                Else
                                    @<span class="text-muted">-</span>
                                End If
                            </span>
                        </div>
                        <div class="info-item">
                            <span class="info-label">Ngày tạo</span>
                            <span class="info-value">@Model.CreatedDate.ToString("dd/MM/yyyy HH:mm")</span>
                        </div>
                        @If Model.ExpectedCompletionDate.HasValue Then
                            @<div class="info-item">
                                <span class="info-label">Ngày dự kiến</span>
                                <span class="info-value">@Model.ExpectedCompletionDate.Value.ToString("dd/MM/yyyy")</span>
                            </div>
                        End If
                        @If Model.CompletedDate.HasValue Then
                            @<div class="info-item">
                                <span class="info-label">Ngày hoàn thành</span>
                                <span class="info-value">@Model.CompletedDate.Value.ToString("dd/MM/yyyy HH:mm")</span>
                            </div>
                        End If
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>

<!-- Quill Rich Text Editor -->
<link href="https://cdn.quilljs.com/1.3.6/quill.snow.css" rel="stylesheet">
<script src="https://cdn.quilljs.com/1.3.6/quill.js"></script>

<!-- Modal Trả lời câu hỏi -->
<div class="modal fade" id="replyModal" tabindex="-1" aria-labelledby="replyModalLabel" aria-hidden="true">
    <div class="modal-dialog modal-lg">
        <div class="modal-content">
            <div class="modal-header bg-primary text-white">
                <h5 class="modal-title" id="replyModalLabel">
                    <i class="fas fa-reply me-2"></i>Trả lời câu hỏi
                </h5>
                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            @Using Html.BeginForm("ReplyToCustomer", "Tasks", FormMethod.Post, New With {.id = "replyForm", .enctype = "multipart/form-data"})
                @Html.AntiForgeryToken()
                @Html.Hidden("id", Model.Id)
                @<div class="modal-body">
                    @If String.IsNullOrEmpty(Model.ResponseToCustomer) Then
                        @<div class="alert alert-info">
                            <i class="fas fa-info-circle me-2"></i>
                            <strong>Lưu ý:</strong> Sau khi trả lời, công việc sẽ tự động chuyển sang trạng thái "Đã hoàn thành".
                        </div>
                    Else
                        @<div class="alert alert-warning">
                            <i class="fas fa-exclamation-triangle me-2"></i>
                            <strong>Lưu ý:</strong> Đã có phản hồi trước đó. Bạn có thể cập nhật phản hồi này. Công việc sẽ vẫn ở trạng thái "Đã hoàn thành".
                        </div>
                    End If
                    <div class="mb-3">
                        <label for="responseToCustomer" class="form-label">
                            <i class="fas fa-comment me-2"></i>Nội dung trả lời khách hàng: <span class="text-danger">*</span>
                        </label>
                        @Html.TextAreaFor(Function(m) m.ResponseToCustomer, New With {.class = "form-control", .id = "responseToCustomer-textarea", .style = "display:none;", .required = "required"})
                        <div id="responseToCustomer-editor" style="min-height: 200px; background: white; border: 1px solid #d1d5db; border-radius: 6px;"></div>
                        <small class="form-text text-muted">Vui lòng nhập nội dung trả lời chi tiết và rõ ràng.</small>
                    </div>
                    
                    @* File đính kèm *@
                    <div class="mb-3">
                        <div class="row">
                            <div class="col-md-6 mb-3">
                                <label class="form-label">
                                    <i class="fas fa-image me-2"></i>Hình ảnh đính kèm
                                </label>
                                <div class="upload-zone" id="replyImageUploadZone">
                                    <input type="file" name="uploadedImages" id="replyUploadedImages" class="d-none" multiple accept="image/*" />
                                    <div class="upload-area" onclick="document.getElementById('replyUploadedImages').click()">
                                        <i class="fas fa-cloud-upload-alt fa-2x mb-2"></i>
                                        <p class="mb-1">Kéo thả ảnh vào đây hoặc click để chọn</p>
                                        <p class="small text-muted mb-0">Hoặc dán ảnh từ clipboard (Ctrl+V)</p>
                                    </div>
                                    <div class="upload-preview" id="replyImagePreview"></div>
                                </div>
                                <small class="form-text">Có thể chọn nhiều hình ảnh, kéo thả, hoặc copy/paste</small>
                            </div>
                            <div class="col-md-6 mb-3">
                                <label class="form-label">
                                    <i class="fas fa-file me-2"></i>File đính kèm
                                </label>
                                <div class="upload-zone" id="replyFileUploadZone">
                                    <input type="file" name="uploadedFiles" id="replyUploadedFiles" class="d-none" multiple />
                                    <div class="upload-area" onclick="document.getElementById('replyUploadedFiles').click()">
                                        <i class="fas fa-cloud-upload-alt fa-2x mb-2"></i>
                                        <p class="mb-1">Kéo thả file vào đây hoặc click để chọn</p>
                                        <p class="small text-muted mb-0">Có thể chọn nhiều file</p>
                                    </div>
                                    <div class="upload-preview" id="replyFilePreview"></div>
                                </div>
                                <small class="form-text">Có thể chọn nhiều file hoặc kéo thả</small>
                            </div>
                        </div>
                    </div>
                </div>
                @<div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">
                        <i class="fas fa-times me-2"></i>Hủy
                    </button>
                    <button type="submit" class="btn btn-primary">
                        <i class="fas fa-paper-plane me-2"></i>Gửi trả lời
                    </button>
                </div>
            End Using
        </div>
    </div>
</div>

<style>
    /* Quill Editor Styles for Reply Modal */
    #responseToCustomer-editor {
        border: 1px solid #d1d5db;
        border-radius: 6px;
    }
    
    #responseToCustomer-editor .ql-container {
        font-size: 0.875rem;
        min-height: 200px;
    }
    
    #responseToCustomer-editor .ql-editor {
        min-height: 200px;
    }
    
    #responseToCustomer-editor .ql-toolbar {
        border-top-left-radius: 6px;
        border-top-right-radius: 6px;
        border-bottom: 1px solid #d1d5db;
        background: #f8f9fa;
    }
    
    #responseToCustomer-editor .ql-toolbar button:hover,
    #responseToCustomer-editor .ql-toolbar button.ql-active {
        color: var(--primary-color);
    }
    
    /* Upload Zone Styles for Reply Modal */
    .upload-zone {
        position: relative;
    }
    
    .upload-area {
        border: 2px dashed #d1d5db;
        border-radius: 8px;
        padding: 1.5rem;
        text-align: center;
        background: #f9fafb;
        cursor: pointer;
        transition: all 0.3s ease;
    }
    
    .upload-area:hover {
        border-color: var(--primary-color);
        background: #f0fdf4;
    }
    
    .upload-area.drag-over {
        border-color: var(--primary-color);
        background: #dcfce7;
        transform: scale(1.02);
    }
    
    .upload-preview {
        margin-top: 1rem;
        display: flex;
        flex-wrap: wrap;
        gap: 0.5rem;
    }
    
    .preview-item {
        position: relative;
        border: 1px solid #e5e7eb;
        border-radius: 6px;
        overflow: hidden;
        background: white;
    }
    
    .preview-item img {
        width: 80px;
        height: 80px;
        object-fit: cover;
        display: block;
    }
    
    .preview-item .file-icon {
        width: 80px;
        height: 80px;
        display: flex;
        align-items: center;
        justify-content: center;
        background: #f3f4f6;
        color: #6b7280;
        font-size: 1.5rem;
    }
    
    .preview-item .file-name {
        padding: 0.25rem 0.5rem;
        font-size: 0.7rem;
        color: #374151;
        max-width: 80px;
        overflow: hidden;
        text-overflow: ellipsis;
        white-space: nowrap;
    }
    
    .preview-item .remove-btn {
        position: absolute;
        top: 0;
        right: 0;
        background: rgba(239, 68, 68, 0.9);
        color: white;
        border: none;
        width: 24px;
        height: 24px;
        border-radius: 0 6px 0 6px;
        cursor: pointer;
        display: flex;
        align-items: center;
        justify-content: center;
        font-size: 0.7rem;
    }
    
    .preview-item .remove-btn:hover {
        background: #dc2626;
    }
</style>

<script>
    document.addEventListener('DOMContentLoaded', function() {
        // Khởi tạo Quill editor cho reply form
        var replyQuill = new Quill('#responseToCustomer-editor', {
            theme: 'snow',
            modules: {
                toolbar: [
                    [{ 'header': [1, 2, 3, false] }],
                    ['bold', 'italic', 'underline', 'strike'],
                    [{ 'color': [] }, { 'background': [] }],
                    [{ 'list': 'ordered'}, { 'list': 'bullet' }],
                    [{ 'align': [] }],
                    ['link', 'image'],
                    ['clean']
                ]
            },
            placeholder: 'Nhập nội dung trả lời khách hàng...'
        });
        
        // Lấy textarea ẩn
        var replyTextarea = document.getElementById('responseToCustomer-textarea');
        
        // Sync nội dung từ Quill vào textarea khi thay đổi
        replyQuill.on('text-change', function() {
            var html = replyQuill.root.innerHTML;
            if (html === '<p><br></p>' || html === '<p></p>') {
                replyTextarea.value = '';
            } else {
                replyTextarea.value = html;
            }
        });
        
        // Load nội dung hiện có vào editor (nếu có)
        if (replyTextarea && replyTextarea.value) {
            replyQuill.root.innerHTML = replyTextarea.value;
        }
        
        // Upload Zone Functionality for Reply Form
        const replyImageInput = document.getElementById('replyUploadedImages');
        const replyFileInput = document.getElementById('replyUploadedFiles');
        const replyImageZone = document.getElementById('replyImageUploadZone');
        const replyFileZone = document.getElementById('replyFileUploadZone');
        const replyImagePreview = document.getElementById('replyImagePreview');
        const replyFilePreview = document.getElementById('replyFilePreview');
        
        let replyImageFiles = [];
        let replyFileFiles = [];
        
        // Setup Image Upload Zone
        if (replyImageZone && replyImageInput) {
            setupReplyUploadZone(replyImageZone, replyImageInput, replyImagePreview, replyImageFiles, true);
        }
        
        // Setup File Upload Zone
        if (replyFileZone && replyFileInput) {
            setupReplyUploadZone(replyFileZone, replyFileInput, replyFilePreview, replyFileFiles, false);
        }
        
        // Paste images from clipboard
        document.addEventListener('paste', function(e) {
            if (e.target.closest('#replyModal')) {
                const items = e.clipboardData.items;
                for (let i = 0; i < items.length; i++) {
                    if (items[i].type.indexOf('image') !== -1) {
                        const blob = items[i].getAsFile();
                        const file = new File([blob], 'pasted-image-' + Date.now() + '.png', { type: blob.type });
                        addFileToReplyInput(file, replyImageInput, replyImagePreview, replyImageFiles, true);
                    }
                }
            }
        });
        
        function setupReplyUploadZone(zone, input, preview, filesArray, isImage) {
            const uploadArea = zone.querySelector('.upload-area');
            
            input.addEventListener('change', function(e) {
                handleReplyFiles(e.target.files, input, preview, filesArray, isImage);
            });
            
            uploadArea.addEventListener('dragover', function(e) {
                e.preventDefault();
                uploadArea.classList.add('drag-over');
            });
            
            uploadArea.addEventListener('dragleave', function(e) {
                e.preventDefault();
                uploadArea.classList.remove('drag-over');
            });
            
            uploadArea.addEventListener('drop', function(e) {
                e.preventDefault();
                uploadArea.classList.remove('drag-over');
                handleReplyFiles(e.dataTransfer.files, input, preview, filesArray, isImage);
            });
        }
        
        function handleReplyFiles(files, input, preview, filesArray, isImage) {
            for (let i = 0; i < files.length; i++) {
                const file = files[i];
                if (isImage && file.type.startsWith('image/')) {
                    addFileToReplyInput(file, input, preview, filesArray, true);
                } else if (!isImage) {
                    addFileToReplyInput(file, input, preview, filesArray, false);
                }
            }
            updateReplyFileInput(input, filesArray);
        }
        
        function addFileToReplyInput(file, input, preview, filesArray, isImage) {
            filesArray.push(file);
            
            const previewItem = document.createElement('div');
            previewItem.className = 'preview-item';
            
            if (isImage) {
                const img = document.createElement('img');
                img.src = URL.createObjectURL(file);
                previewItem.appendChild(img);
            } else {
                const fileIcon = document.createElement('div');
                fileIcon.className = 'file-icon';
                fileIcon.innerHTML = '<i class="fas fa-file"></i>';
                previewItem.appendChild(fileIcon);
                
                const fileName = document.createElement('div');
                fileName.className = 'file-name';
                fileName.textContent = file.name;
                previewItem.appendChild(fileName);
            }
            
            const removeBtn = document.createElement('button');
            removeBtn.className = 'remove-btn';
            removeBtn.innerHTML = '<i class="fas fa-times"></i>';
            removeBtn.onclick = function() {
                const index = filesArray.indexOf(file);
                if (index > -1) {
                    filesArray.splice(index, 1);
                    updateReplyFileInput(input, filesArray);
                    if (isImage) {
                        const imgElement = previewItem.querySelector('img');
                        if (imgElement) {
                            URL.revokeObjectURL(imgElement.src);
                        }
                    }
                    previewItem.remove();
                }
            };
            previewItem.appendChild(removeBtn);
            preview.appendChild(previewItem);
        }
        
        function updateReplyFileInput(input, filesArray) {
            const dataTransfer = new DataTransfer();
            filesArray.forEach(file => dataTransfer.items.add(file));
            input.files = dataTransfer.files;
        }
        
        // Form submission
        const replyForm = document.getElementById('replyForm');
        if (replyForm) {
            replyForm.addEventListener('submit', function(e) {
                e.preventDefault();
                
                // Validate Quill editor content
                var content = replyQuill.root.innerHTML;
                if (content === '<p><br></p>' || content === '<p></p>' || content.trim() === '') {
                    alert('Vui lòng nhập nội dung trả lời khách hàng!');
                    replyQuill.focus();
                    return false;
                }
                
                // Update hidden textarea with Quill content before creating FormData
                replyTextarea.value = content;
                
                const submitBtn = replyForm.querySelector('button[type="submit"]');
                const originalText = submitBtn.innerHTML;
                const formAction = replyForm.getAttribute('action') || '@Url.Action("ReplyToCustomer", "Tasks")';
                
                submitBtn.disabled = true;
                submitBtn.innerHTML = '<i class="fas fa-spinner fa-spin me-2"></i>Đang xử lý...';
                
                // Lấy AntiForgeryToken từ form
                const tokenInput = replyForm.querySelector('input[name="__RequestVerificationToken"]');
                if (!tokenInput) {
                    alert('Không tìm thấy token bảo mật. Vui lòng tải lại trang.');
                    submitBtn.disabled = false;
                    submitBtn.innerHTML = originalText;
                    return;
                }
                
                // Tạo FormData từ form đã được cập nhật giá trị textarea
                const formData = new FormData(replyForm);
                
                // Lưu ý: Không cần gọi formData.set('responseToCustomer', content) nữa 
                // vì textarea đã có giá trị mới nhất và sẽ được FormData tự động lấy.
                // MVC sẽ tự động bind ResponseToCustomer vào parameter responseToCustomer.
                
                fetch(formAction, {
                    method: 'POST',
                    body: formData
                })
                .then(response => {
                    if (!response.ok) {
                        return response.text().then(text => {
                            throw new Error('HTTP ' + response.status + ': ' + text.substring(0, 100));
                        });
                    }
                    return response.json();
                })
                .then(data => {
                    if (data.success) {
                        alert('Trả lời đã được gửi thành công! Công việc đã chuyển sang trạng thái "Đã hoàn thành".');
                        window.location.reload();
                    } else {
                        alert('Có lỗi xảy ra: ' + (data.message || 'Vui lòng thử lại.'));
                        submitBtn.disabled = false;
                        submitBtn.innerHTML = originalText;
                    }
                })
                .catch(error => {
                    console.error('Error:', error);
                    alert('Có lỗi xảy ra khi gửi trả lời. Vui lòng thử lại.');
                    submitBtn.disabled = false;
                    submitBtn.innerHTML = originalText;
                });
            });
        }
    });
    
    // Image Lightbox
    document.addEventListener('DOMContentLoaded', function() {
        const images = document.querySelectorAll('.image-zoom');
        const lightbox = document.createElement('div');
        lightbox.className = 'image-lightbox';
        lightbox.innerHTML = `
            <span class="image-lightbox-close">&times;</span>
            <div class="image-lightbox-content">
                <img src="" alt="Image" />
            </div>
        `;
        document.body.appendChild(lightbox);
        
        const lightboxImg = lightbox.querySelector('img');
        const closeBtn = lightbox.querySelector('.image-lightbox-close');
        
        images.forEach(function(img) {
            img.addEventListener('click', function(e) {
                e.preventDefault();
                e.stopPropagation();
                const imageSrc = this.getAttribute('data-image-src') || (this.querySelector('img') ? this.querySelector('img').src : this.src);
                if (imageSrc) {
                    lightboxImg.src = imageSrc;
                    lightbox.style.display = 'flex';
                    document.body.style.overflow = 'hidden';
                }
            });
        });
        
        closeBtn.addEventListener('click', function(e) {
            e.preventDefault();
            e.stopPropagation();
            lightbox.style.display = 'none';
            document.body.style.overflow = '';
        });
        
        lightbox.addEventListener('click', function(e) {
            if (e.target === lightbox || e.target === lightboxImg) {
                lightbox.style.display = 'none';
                document.body.style.overflow = '';
            }
        });
        
        document.addEventListener('keydown', function(e) {
            if (e.key === 'Escape' && lightbox.style.display === 'flex') {
                lightbox.style.display = 'none';
                document.body.style.overflow = '';
            }
        });
    });
</script>
