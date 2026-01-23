@ModelType Question
@Code
    ViewData("Title") = "Chi tiết yêu cầu hỗ trợ"
End Code

<div class="container-fluid py-3">
    <div class="row g-3">
        <div class="col-lg-8">
            <div class="card shadow-sm mb-3">
                <div class="card-header-custom">
                    <h5 class="mb-0" style="font-size: 0.95rem; font-weight: 600; color: #1f2937;">
                        <i class="fas fa-headset me-2" style="color: var(--primary-color);"></i>Chi tiết yêu cầu hỗ trợ
                    </h5>
                </div>
                <div class="card-body p-3">
                    <div class="row g-2">
                        <div class="col-md-6">
                            <label class="form-label small text-muted mb-1" style="font-weight: 600;">Kỹ thuật viên:</label>
                            <p class="mb-0 fw-semibold">@If Not String.IsNullOrEmpty(Model.TechnicianName) Then @Model.TechnicianName Else @<span class="text-muted">-</span> End If</p>
                        </div>
                        <div class="col-md-6">
                            <label class="form-label small text-muted mb-1" style="font-weight: 600;">Ngày gửi yêu cầu:</label>
                            <p class="mb-0 small">@Model.CreatedDate.ToString("dd/MM/yyyy HH:mm")</p>
                        </div>
                        <div class="col-md-6">
                            <label class="form-label small text-muted mb-1" style="font-weight: 600;">Trạng thái:</label>
                            <p class="mb-0">
                                @If Model.Status = QuestionStatus.Answered Then
                                    @<span class="badge bg-success">Đã trả lời</span>
                                Else
                                    @<span class="badge bg-warning text-dark">Chờ trả lời</span>
                                End If
                            </p>
                        </div>
                        @If Model.Status = QuestionStatus.Answered AndAlso Model.AnsweredDate.HasValue Then
                            @<div class="col-md-6">
                                <label class="form-label small text-muted mb-1" style="font-weight: 600;">Ngày trả lời:</label>
                                <p class="mb-0 small">@Model.AnsweredDate.Value.ToString("dd/MM/yyyy HH:mm")</p>
                            </div>
                        End If
                    </div>
                </div>
            </div>

            <div class="card shadow-sm mb-3">
                <div class="card-header-custom">
                    <h6 class="mb-0" style="font-size: 0.875rem; font-weight: 600; color: #1f2937;">
                        <i class="fas fa-comment-question me-2" style="color: #6366f1;"></i>Nội dung yêu cầu hỗ trợ
                    </h6>
                </div>
                <div class="card-body p-3">
                    <div class="content-box" style="font-size: 0.875rem; word-wrap: break-word; line-height: 1.6;">
                        @If Not String.IsNullOrEmpty(Model.Question) Then
                            @Html.Raw(Model.Question)
                        Else
                            @<span class="text-muted">Chưa có nội dung</span>
                        End If
                    </div>
                </div>
            </div>

            @If Model.Status = QuestionStatus.Answered AndAlso Not String.IsNullOrEmpty(Model.Answer) Then
                @<div class="card shadow-sm mb-3">
                    <div class="card-header-custom">
                        <h6 class="mb-0" style="font-size: 0.875rem; font-weight: 600; color: #1f2937;">
                            <i class="fas fa-comment-check me-2" style="color: #10b981;"></i>Phản hồi hỗ trợ
                        </h6>
                    </div>
                    <div class="card-body p-3">
                        <div class="content-box answer-box" style="font-size: 0.875rem; white-space: pre-wrap; word-wrap: break-word; line-height: 1.6;">
                            @Html.Raw(Model.Answer)
                        </div>
                    </div>
                </div>
            End If

            @If Model.Images IsNot Nothing AndAlso Model.Images.Count > 0 Then
                @<div class="card shadow-sm mb-3">
                    <div class="card-header-custom">
                        <h6 class="mb-0" style="font-size: 0.875rem; font-weight: 600; color: #1f2937;">
                            <i class="fas fa-image me-2" style="color: #8b5cf6;"></i>Hình ảnh đính kèm
                        </h6>
                    </div>
                    <div class="card-body p-3">
                        <div class="row g-2">
                            @For Each imgUrl In Model.Images
                                @<div class="col-md-3 col-sm-4 col-6">
                                    <a href="@Url.Action("Preview", "File", New With {.filePath = imgUrl})" target="_blank" class="image-lightbox">
                                        <img src="@Url.Action("Preview", "File", New With {.filePath = imgUrl})" alt="Hình ảnh" class="img-thumbnail w-100" style="height: 120px; object-fit: cover; cursor: pointer;" />
                                    </a>
                                </div>
                            Next
                        </div>
                    </div>
                </div>
            End If

            @If Model.Attachments IsNot Nothing AndAlso Model.Attachments.Count > 0 Then
                @<div class="card shadow-sm mb-3">
                    <div class="card-header-custom">
                        <h6 class="mb-0" style="font-size: 0.875rem; font-weight: 600; color: #1f2937;">
                            <i class="fas fa-file me-2" style="color: #f59e0b;"></i>File đính kèm
                        </h6>
                    </div>
                    <div class="card-body p-3">
                        <div class="d-flex flex-wrap gap-2">
                            @For Each fileUrl In Model.Attachments
                                @<a href="@Url.Action("Download", "File", New With {.filePath = fileUrl})" target="_blank" class="btn btn-outline-primary btn-sm">
                                    <i class="fas fa-download me-1"></i>@System.IO.Path.GetFileName(fileUrl.Split("?"c)(0))
                                </a>
                            Next
                        </div>
                    </div>
                </div>
            End If
        </div>

        <div class="col-lg-4">
            <div class="card shadow-sm mb-3">
                <div class="card-header-custom">
                    <h6 class="mb-0" style="font-size: 0.875rem; font-weight: 600; color: #1f2937;">
                        <i class="fas fa-bolt me-2" style="color: #64748b;"></i>Thao tác nhanh
                    </h6>
                </div>
                <div class="card-body p-3">
                    <div class="d-grid gap-2">
                        <a href="@Url.Action("Index", "Questions")" class="btn btn-secondary btn-sm">
                            <i class="fas fa-list me-2"></i>Danh sách yêu cầu
                        </a>
                        @Using Html.BeginForm("Delete", "Questions", FormMethod.Post, New With {.style = "display: inline;"})
                            @Html.AntiForgeryToken()
                            @Html.Hidden("id", Model.Id)
                            @<button type="submit" class="btn btn-danger btn-sm w-100" onclick="return confirm('Bạn có chắc chắn muốn xóa yêu cầu hỗ trợ này?');">
                                <i class="fas fa-trash me-2"></i>Xóa yêu cầu
                            </button>
                        End Using
                    </div>
                </div>
            </div>

            @Code
                Dim buttonText = If(Model.Status = QuestionStatus.Answered, "Cập nhật phản hồi", "Gửi phản hồi")
                Dim helpText = If(Model.Status = QuestionStatus.Answered, "Bạn có thể chỉnh sửa hoặc thêm nội dung vào câu trả lời hiện tại.", "Vui lòng nhập nội dung phản hồi chi tiết và rõ ràng.")
                Dim answerValue = If(Model.Status = QuestionStatus.Answered AndAlso Not String.IsNullOrEmpty(Model.Answer), Model.Answer, "")
            End Code

            <div class="card shadow-sm">
                <div class="card-header-custom">
                    <h6 class="mb-0" style="font-size: 0.875rem; font-weight: 600; color: #1f2937;">
                        <i class="fas fa-reply me-2" style="color: var(--primary-color);"></i>Phản hồi yêu cầu hỗ trợ
                        @If Model.Status = QuestionStatus.Answered Then
                            @<span class="badge badge-success ms-2">Đã phản hồi</span>
                        End If
                    </h6>
                </div>
                <div class="card-body p-3">
                    @Using Html.BeginForm("Reply", "Questions", FormMethod.Post, New With {.id = "replyForm"})
                        @Html.AntiForgeryToken()
                        @Html.Hidden("id", Model.Id)
                        @<div class="mb-2">
                            <label for="answer" class="form-label small mb-1" style="font-weight: 600;">
                                Nội dung phản hồi: <span class="text-danger">*</span>
                                @If Model.Status = QuestionStatus.Answered Then
                                    @<span class="text-muted small ms-2">(Có thể chỉnh sửa hoặc thêm nội dung)</span>
                                End If
                            </label>
                            <textarea class="form-control form-control-sm" id="answer" name="answer" rows="6" required placeholder="Nhập nội dung phản hồi hỗ trợ...">@answerValue</textarea>
                            <small class="form-text text-muted" style="font-size: 0.7rem;">
                                <i class="fas fa-info-circle me-1"></i>@helpText
                            </small>
                        </div>
                        @<div class="d-grid">
                            <button type="submit" class="btn btn-primary btn-sm">
                                <i class="fas fa-paper-plane me-2"></i>@buttonText
                            </button>
                        </div>
                    End Using
                </div>
            </div>
        </div>
    </div>
</div>

<style>
    .card {
        border: 1px solid #e5e7eb;
        border-radius: 8px;
        background: white;
    }
    
    .card-header-custom {
        padding: 0.875rem 1rem;
        background: #f9fafb;
        border-bottom: 1px solid #e5e7eb;
        border-radius: 8px 8px 0 0;
    }
    
    .content-box {
        padding: 1rem;
        background: #f9fafb;
        border-radius: 6px;
        border-left: 3px solid #6366f1;
    }
    
    .answer-box {
        border-left-color: #10b981;
    }
    
    .form-control-sm {
        font-size: 0.875rem;
        border-radius: 6px;
        border: 1px solid #d1d5db;
        padding: 0.5rem 0.75rem;
        background: white;
    }
    
    .form-control-sm:focus {
        border-color: var(--primary-color);
        box-shadow: 0 0 0 3px rgba(76, 175, 80, 0.1);
        outline: none;
    }
    
    textarea.form-control-sm {
        resize: vertical;
        min-height: 120px;
    }
    
    .badge {
        font-size: 0.7rem;
        padding: 0.3rem 0.6rem;
        font-weight: 500;
    }
    
    .badge-success {
        background-color: #d1fae5;
        color: #065f46;
    }
    
    .btn-sm {
        font-size: 0.875rem;
        padding: 0.4rem 0.75rem;
    }
    
    .card-body {
        background: white;
    }
    
    /* Lightbox styles */
    .image-lightbox {
        display: block;
        position: relative;
    }
    
    .image-lightbox img {
        transition: transform 0.2s;
    }
    
    .image-lightbox:hover img {
        transform: scale(1.05);
    }
    
    .lightbox-overlay {
        display: none;
        position: fixed;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        background: rgba(0, 0, 0, 0.9);
        z-index: 9999;
        cursor: pointer;
    }
    
    .lightbox-overlay.active {
        display: flex;
        align-items: center;
        justify-content: center;
    }
    
    .lightbox-image {
        max-width: 90%;
        max-height: 90%;
        object-fit: contain;
    }
</style>

<script>
    document.addEventListener('DOMContentLoaded', function() {
        const replyForm = document.getElementById('replyForm');
        if (replyForm) {
            replyForm.addEventListener('submit', function(e) {
                e.preventDefault();
                
                const formData = new FormData(this);
                const answer = formData.get('answer');
                
                if (!answer || answer.trim() === '') {
                    alert('Vui lòng nhập nội dung trả lời');
                    return;
                }
                
                // Show loading
                const submitBtn = this.querySelector('button[type="submit"]');
                const originalText = submitBtn.innerHTML;
                submitBtn.disabled = true;
                submitBtn.innerHTML = '<i class="fas fa-spinner fa-spin me-2"></i>Đang gửi...';
                
                fetch('@Url.Action("Reply", "Questions")', {
                    method: 'POST',
                    body: formData,
                    headers: {
                        'RequestVerificationToken': formData.get('__RequestVerificationToken')
                    }
                })
                .then(response => response.json())
                .then(data => {
                    if (data.success) {
                        alert(data.message);
                        window.location.reload();
                    } else {
                        alert(data.message || 'Có lỗi xảy ra');
                        submitBtn.disabled = false;
                        submitBtn.innerHTML = originalText;
                    }
                })
                .catch(error => {
                    console.error('Error:', error);
                    alert('Có lỗi xảy ra khi gửi câu trả lời');
                    submitBtn.disabled = false;
                    submitBtn.innerHTML = originalText;
                });
            });
        }
        
        // Lightbox for images
        const imageLinks = document.querySelectorAll('.image-lightbox');
        const overlay = document.createElement('div');
        overlay.className = 'lightbox-overlay';
        const lightboxImg = document.createElement('img');
        lightboxImg.className = 'lightbox-image';
        overlay.appendChild(lightboxImg);
        document.body.appendChild(overlay);
        
        imageLinks.forEach(link => {
            link.addEventListener('click', function(e) {
                e.preventDefault();
                lightboxImg.src = this.href;
                overlay.classList.add('active');
            });
        });
        
        overlay.addEventListener('click', function() {
            this.classList.remove('active');
        });
    });
</script>

