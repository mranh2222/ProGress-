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
    }
    
    .image-lightbox-content {
        position: relative;
        margin: auto;
        padding: 20px;
        width: 90%;
        max-width: 1200px;
        top: 50%;
        transform: translateY(-50%);
    }
    
    .image-lightbox img {
        width: 100%;
        height: auto;
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

    <div class="row">
        <div class="col-lg-8">
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

            @Code
                Dim hasImages = (Model.Images IsNot Nothing AndAlso Model.Images.Any())
                Dim hasAttachments = (Model.Attachments IsNot Nothing AndAlso Model.Attachments.Any())
            End Code
            
            @If hasImages OrElse hasAttachments Then
                @<div class="card">
                    <div class="card-header">File & Hình ảnh đính kèm</div>
                    <div class="card-body">
                        <div class="row g-3">
                            @If hasImages Then
                                @<div class="col-md-6">
                                    <h6 class="mb-2" style="font-size: 0.8rem; font-weight: 600;">Hình ảnh</h6>
                                    <div class="row g-2">
                                        @For Each img In Model.Images
                                            @<div class="col-6">
                                                <img src="@img" class="img-fluid rounded image-zoom" style="max-height: 150px; width: 100%; object-fit: cover; cursor: pointer;" data-image-src="@img" />
                                            </div>
                                        Next
                                    </div>
                                </div>
                            End If
                            
                            @If hasAttachments Then
                                @<div class="col-md-6">
                                    <h6 class="mb-2" style="font-size: 0.8rem; font-weight: 600;">File đính kèm</h6>
                                    <div class="d-flex flex-column gap-1">
                                        @For Each att In Model.Attachments
                                            @<a href="@att" target="_blank" class="text-decoration-none d-flex align-items-center" style="font-size: 0.8rem; padding: 0.4rem; background: #f9fafb; border-radius: 4px;">
                                                <i class="fas fa-file me-2 text-primary"></i>
                                                <span class="text-truncate">@System.IO.Path.GetFileName(att)</span>
                                            </a>
                                        Next
                                    </div>
                                </div>
                            End If
                        </div>
                    </div>
                </div>
            End If

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

        <div class="col-lg-4">
            <div class="card">
                <div class="card-header d-flex justify-content-between align-items-center">
                    <span>Nội dung hỗ trợ</span>
                    <button type="button" class="btn btn-primary btn-sm" data-bs-toggle="modal" data-bs-target="#replyModal">
                        <i class="fas fa-reply me-1"></i>Trả lời
                    </button>
                </div>
                <div class="card-body">
                    <div class="content-section" style="border-left-color: #3b82f6;">
                        <h6><i class="fas fa-question-circle me-1"></i>Câu hỏi / Mô tả lỗi</h6>
                        @If Not String.IsNullOrEmpty(Model.Description) Then
                            @<div style="word-wrap: break-word;">@Html.Raw(Model.Description)</div>
                        Else
                            @<p class="text-muted mb-0">Chưa có mô tả</p>
                        End If
                    </div>

                    @If Not String.IsNullOrEmpty(Model.Solution) Then
                        @<div class="content-section" style="border-left-color: #10b981;">
                            <h6><i class="fas fa-wrench me-1"></i>Giải pháp</h6>
                            <p style="white-space: pre-wrap; word-wrap: break-word; margin: 0;">@Model.Solution</p>
                        </div>
                    End If

                    @If Not String.IsNullOrEmpty(Model.ResponseToCustomer) Then
                        @<div class="content-section" style="border-left-color: #3b82f6;">
                            <h6><i class="fas fa-reply me-1"></i>Phản hồi khách hàng</h6>
                            <p style="white-space: pre-wrap; word-wrap: break-word; margin: 0;">@Model.ResponseToCustomer</p>
                        </div>
                    End If

                    @If String.IsNullOrEmpty(Model.Solution) AndAlso String.IsNullOrEmpty(Model.ResponseToCustomer) Then
                        @<div class="text-center text-muted py-3">
                            <i class="fas fa-inbox fa-lg mb-2"></i>
                            <p class="mb-0" style="font-size: 0.8rem;">Chưa có giải pháp hoặc phản hồi</p>
                        </div>
                    End If
                </div>
            </div>
            
            <div class="card">
                <div class="card-header">Thao tác</div>
                <div class="card-body">
                    <div class="d-grid gap-2">
                        <a href="@Url.Action("Delete", "Tasks", New With {.id = Model.Id})" class="btn btn-danger btn-sm" onclick="return confirm('Bạn có chắc chắn muốn xóa công việc này?');">
                            <i class="fas fa-trash me-1"></i>Xóa
                        </a>
                        <a href="@Url.Action("Index", "Tasks")" class="btn btn-secondary btn-sm">
                            <i class="fas fa-list me-1"></i>Danh sách
                        </a>
                        <a href="@Url.Action("Index", "Dashboard")" class="btn btn-info btn-sm">
                            <i class="fas fa-home me-1"></i>Dashboard
                        </a>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>

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
            @Using Html.BeginForm("ReplyToCustomer", "Tasks", FormMethod.Post, New With {.id = "replyForm"})
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
                        <textarea class="form-control" id="responseToCustomer" name="responseToCustomer" rows="8" required placeholder="Nhập nội dung trả lời khách hàng...">@Model.ResponseToCustomer</textarea>
                        <small class="form-text text-muted">Vui lòng nhập nội dung trả lời chi tiết và rõ ràng.</small>
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

<script>
    document.addEventListener('DOMContentLoaded', function() {
        const replyForm = document.getElementById('replyForm');
        if (replyForm) {
            replyForm.addEventListener('submit', function(e) {
                e.preventDefault();
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
                
                // Lấy các giá trị từ form
                const id = replyForm.querySelector('input[name="id"]').value;
                const responseToCustomer = replyForm.querySelector('textarea[name="responseToCustomer"]').value;
                
                // Tạo FormData với token
                const formData = new FormData();
                formData.append('__RequestVerificationToken', tokenInput.value);
                formData.append('id', id);
                formData.append('responseToCustomer', responseToCustomer);
                
                fetch(formAction, {
                    method: 'POST',
                    body: formData,
                    headers: {
                        'RequestVerificationToken': tokenInput.value
                    }
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
            img.addEventListener('click', function() {
                lightboxImg.src = this.getAttribute('data-image-src') || this.src;
                lightbox.style.display = 'block';
                document.body.style.overflow = 'hidden';
            });
        });
        
        closeBtn.addEventListener('click', function() {
            lightbox.style.display = 'none';
            document.body.style.overflow = 'auto';
        });
        
        lightbox.addEventListener('click', function(e) {
            if (e.target === lightbox) {
                lightbox.style.display = 'none';
                document.body.style.overflow = 'auto';
            }
        });
        
        document.addEventListener('keydown', function(e) {
            if (e.key === 'Escape' && lightbox.style.display === 'block') {
                lightbox.style.display = 'none';
                document.body.style.overflow = 'auto';
            }
        });
    });
</script>
