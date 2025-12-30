@ModelType Task
@Code
    ViewData("Title") = "Chi tiết công việc"
End Code

<div class="container-fluid">
    <div class="row mb-4">
        <div class="col-md-8">
            <h1><i class="fas fa-info-circle me-2"></i>Chi tiết công việc</h1>
        </div>
        <div class="col-md-4 text-end">
            <a href="@Url.Action("Edit", "Tasks", New With {.id = Model.Id})" class="btn btn-warning">
                <i class="fas fa-edit me-2"></i>Chỉnh sửa
            </a>
            <a href="@Url.Action("Index", "Tasks")" class="btn btn-secondary">
                <i class="fas fa-arrow-left me-2"></i>Quay lại
            </a>
        </div>
    </div>

    <div class="row">
        <div class="col-lg-8">
            <div class="card mb-3">
                <div class="card-header">
                    <h5 class="mb-0">Thông tin cơ bản</h5>
                </div>
                <div class="card-body">
                    <dl class="row">
                        <dt class="col-sm-3">Tag:</dt>
                        <dd class="col-sm-9">
                            @If Not String.IsNullOrEmpty(Model.Tag) Then
                                @<span class="badge bg-secondary">@Model.Tag</span>
                            Else
                                @<span class="text-muted">-</span>
                            End If
                        </dd>

                        <dt class="col-sm-3">Ngày nhận file:</dt>
                        <dd class="col-sm-9">
                            @If Model.FileReceivedDate.HasValue Then
                                @Model.FileReceivedDate.Value.ToString("dd/MM/yyyy")
                            Else
                                @<span class="text-muted">-</span>
                            End If
                        </dd>

                        <dt class="col-sm-3">Nền tảng hỗ trợ:</dt>
                        <dd class="col-sm-9">
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
                        </dd>

                        <dt class="col-sm-3">Sale quản lý:</dt>
                        <dd class="col-sm-9">
                            @If Not String.IsNullOrEmpty(Model.SaleManagerName) Then
                                @Model.SaleManagerName
                            Else
                                @<span class="text-muted">-</span>
                            End If
                        </dd>

                        <dt class="col-sm-3">Khách hàng:</dt>
                        <dd class="col-sm-9">@Model.CustomerName</dd>

                        <dt class="col-sm-3">Phần mềm sử dụng:</dt>
                        <dd class="col-sm-9">
                            @If Not String.IsNullOrEmpty(Model.SoftwareName) Then
                                @<span class="badge bg-info">@Model.SoftwareName</span>
                            Else
                                @<span class="text-muted">-</span>
                            End If
                        </dd>

                        <dt class="col-sm-3">Kỹ thuật phụ trách:</dt>
                        <dd class="col-sm-9">@Model.AssignedToName</dd>

                        <dt class="col-sm-3">Tình trạng:</dt>
                        <dd class="col-sm-9">
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
                                    @<span class="status-badge status-paused">🔴 Tạm dừng</span>
                            End Select
                        </dd>

                        <dt class="col-sm-3">Ngày dự kiến hoàn thành:</dt>
                        <dd class="col-sm-9">
                            @If Model.ExpectedCompletionDate.HasValue Then
                                @Model.ExpectedCompletionDate.Value.ToString("dd/MM/yyyy")
                            Else
                                @<span class="text-muted">-</span>
                            End If
                        </dd>

                        <dt class="col-sm-3">Ngày thực tế hoàn thành:</dt>
                        <dd class="col-sm-9">
                            @If Model.CompletedDate.HasValue Then
                                @Model.CompletedDate.Value.ToString("dd/MM/yyyy HH:mm")
                            Else
                                @<span class="text-muted">-</span>
                            End If
                        </dd>

                        <dt class="col-sm-3">Ngày tạo:</dt>
                        <dd class="col-sm-9">@Model.CreatedDate.ToString("dd/MM/yyyy HH:mm")</dd>

                        @If Model.UpdatedDate.HasValue Then
                            @<dt class="col-sm-3">Ngày cập nhật:</dt>
                            @<dd class="col-sm-9">@Model.UpdatedDate.Value.ToString("dd/MM/yyyy HH:mm")</dd>
                        End If
                    </dl>
                </div>
            </div>

            @Code
                Dim hasImages = (Model.Images IsNot Nothing AndAlso Model.Images.Any())
                Dim hasAttachments = (Model.Attachments IsNot Nothing AndAlso Model.Attachments.Any())
            End Code
            
            @If hasImages OrElse hasAttachments Then
                @<div class="row mb-3">
                    @If hasImages Then
                        @<div class="col-md-6">
                            <div class="card h-100">
                                <div class="card-header">
                                    <h5 class="mb-0">Hình ảnh đính kèm</h5>
                                </div>
                                <div class="card-body">
                                    <div class="row">
                                        @For Each img In Model.Images
                                            @<div class="col-md-6 mb-3">
                                                <img src="@img" class="img-fluid rounded" style="max-height: 200px; width: 100%; object-fit: cover;" />
                                            </div>
                                        Next
                                    </div>
                                </div>
                            </div>
                        </div>
                    End If
                    
                    @If hasAttachments Then
                        @<div class="col-md-6">
                            <div class="card h-100">
                                <div class="card-header">
                                    <h5 class="mb-0">File đính kèm</h5>
                                </div>
                                <div class="card-body">
                                    <ul class="list-group">
                                        @For Each att In Model.Attachments
                                            @<li class="list-group-item">
                                                <a href="@att" target="_blank" class="text-decoration-none">
                                                    <i class="fas fa-file me-2"></i>@System.IO.Path.GetFileName(att)
                                                </a>
                                            </li>
                                        Next
                                    </ul>
                                </div>
                            </div>
                        </div>
                    End If
                </div>
            End If

            @If Model.History IsNot Nothing AndAlso Model.History.Any() Then
                @<div class="card mb-3">
                    <div class="card-header">
                        <h5 class="mb-0">Lịch sử cập nhật</h5>
                    </div>
                    <div class="card-body">
                        <div class="timeline">
                            @For Each history In Model.History.OrderByDescending(Function(h) h.ChangedDate).ToList()
                                @<div class="mb-3 pb-3 border-bottom">
                                    <div class="d-flex justify-content-between">
                                        <div>
                                            <strong>@history.Action</strong>
                                            @If Not String.IsNullOrEmpty(history.Description) Then
                                                @<p class="mb-1">@history.Description</p>
                                            End If
                                            @If Not String.IsNullOrEmpty(history.OldValue) OrElse Not String.IsNullOrEmpty(history.NewValue) Then
                                                @<small class="text-muted">
                                                    @If Not String.IsNullOrEmpty(history.OldValue) Then
                                                        @<span>Từ: @history.OldValue</span>
                                                    End If
                                                    @If Not String.IsNullOrEmpty(history.NewValue) Then
                                                        @<span> → @history.NewValue</span>
                                                    End If
                                                </small>
                                            End If
                                        </div>
                                        <div class="text-end">
                                            <small class="text-muted">@history.ChangedByName</small><br/>
                                            <small class="text-muted">@history.ChangedDate.ToString("dd/MM/yyyy HH:mm")</small>
                                        </div>
                                    </div>
                                </div>
                            Next
                        </div>
                    </div>
                </div>
            End If
        </div>

        <div class="col-lg-4">
            <div class="card mb-3">
                <div class="card-header">
                    <h5 class="mb-0">Thao tác nhanh</h5>
                </div>
                <div class="card-body">
                    <div class="d-grid gap-2">
                        <a href="@Url.Action("Edit", "Tasks", New With {.id = Model.Id})" class="btn btn-warning">
                            <i class="fas fa-edit me-2"></i>Chỉnh sửa
                        </a>
                        <a href="@Url.Action("Delete", "Tasks", New With {.id = Model.Id})" class="btn btn-danger" onclick="return confirm('Bạn có chắc chắn muốn xóa công việc này?');">
                            <i class="fas fa-trash me-2"></i>Xóa
                        </a>
                        <a href="@Url.Action("Index", "Tasks")" class="btn btn-secondary">
                            <i class="fas fa-list me-2"></i>Danh sách
                        </a>
                        <a href="@Url.Action("Index", "Dashboard")" class="btn btn-info">
                            <i class="fas fa-home me-2"></i>Dashboard
                        </a>
                    </div>
                </div>
            </div>

            <div class="card">
                <div class="card-header bg-primary text-white d-flex justify-content-between align-items-center">
                    <h5 class="mb-0"><i class="fas fa-comment-dots me-2"></i>Nội dung hỗ trợ</h5>
                    @If String.IsNullOrEmpty(Model.ResponseToCustomer) Then
                        @<button type="button" class="btn btn-light btn-sm" data-bs-toggle="modal" data-bs-target="#replyModal">
                            <i class="fas fa-reply me-1"></i>Trả lời câu hỏi
                        </button>
                    End If
                </div>
                <div class="card-body">
                    <div class="mb-4">
                        <h6 class="text-primary mb-3"><i class="fas fa-question-circle me-2"></i>Câu hỏi / Mô tả lỗi:</h6>
                        <div class="p-3 bg-light rounded border-start border-primary border-4">
                            @If Not String.IsNullOrEmpty(Model.Description) Then
                                @<p class="mb-0" style="white-space: pre-wrap; word-wrap: break-word;">@Model.Description</p>
                            Else
                                @<p class="text-muted mb-0">Chưa có mô tả</p>
                            End If
                        </div>
                    </div>

                    @If Not String.IsNullOrEmpty(Model.Solution) Then
                        @<div class="mb-4">
                            <h6 class="text-success mb-3"><i class="fas fa-wrench me-2"></i>Giải pháp:</h6>
                            <div class="p-3 bg-light rounded border-start border-success border-4">
                                <p class="mb-0" style="white-space: pre-wrap; word-wrap: break-word;">@Model.Solution</p>
                            </div>
                        </div>
                    End If

                    @If Not String.IsNullOrEmpty(Model.ResponseToCustomer) Then
                        @<div>
                            <h6 class="text-info mb-3"><i class="fas fa-reply me-2"></i>Phản hồi khách hàng:</h6>
                            <div class="p-3 bg-light rounded border-start border-info border-4">
                                <p class="mb-0" style="white-space: pre-wrap; word-wrap: break-word;">@Model.ResponseToCustomer</p>
                            </div>
                        </div>
                    End If

                    @If String.IsNullOrEmpty(Model.Solution) AndAlso String.IsNullOrEmpty(Model.ResponseToCustomer) Then
                        @<div class="text-center text-muted py-4">
                            <i class="fas fa-inbox fa-2x mb-2"></i>
                            <p class="mb-0">Chưa có giải pháp hoặc phản hồi</p>
                        </div>
                    End If
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
                    <div class="alert alert-info">
                        <i class="fas fa-info-circle me-2"></i>
                        <strong>Lưu ý:</strong> Sau khi trả lời, công việc sẽ tự động chuyển sang trạng thái "Đã hoàn thành".
                    </div>
                    <div class="mb-3">
                        <label for="responseToCustomer" class="form-label">
                            <i class="fas fa-comment me-2"></i>Nội dung trả lời khách hàng: <span class="text-danger">*</span>
                        </label>
                        <textarea class="form-control" id="responseToCustomer" name="responseToCustomer" rows="8" required placeholder="Nhập nội dung trả lời khách hàng..."></textarea>
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
                const formData = new FormData(replyForm);
                const submitBtn = replyForm.querySelector('button[type="submit"]');
                const originalText = submitBtn.innerHTML;
                const formAction = replyForm.getAttribute('action') || '@Url.Action("ReplyToCustomer", "Tasks")';
                
                submitBtn.disabled = true;
                submitBtn.innerHTML = '<i class="fas fa-spinner fa-spin me-2"></i>Đang xử lý...';
                
                // Lấy AntiForgeryToken
                const tokenInput = document.querySelector('input[name="__RequestVerificationToken"]');
                if (!tokenInput) {
                    alert('Không tìm thấy token bảo mật. Vui lòng tải lại trang.');
                    submitBtn.disabled = false;
                    submitBtn.innerHTML = originalText;
                    return;
                }
                
                formData.append('__RequestVerificationToken', tokenInput.value);
                
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
</script>
