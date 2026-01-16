@ModelType Task
@Code
    ViewData("Title") = "Chỉnh sửa công việc"
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
    
    ' Tính toán trước để tránh conflict với @Using
    Dim hasImages As Boolean = False
    Dim hasAttachments As Boolean = False
    If Model IsNot Nothing Then
        hasImages = (Model.Images IsNot Nothing AndAlso Model.Images.Any())
        hasAttachments = (Model.Attachments IsNot Nothing AndAlso Model.Attachments.Any())
    End If
End Code

<style>
    body {
        background: #f8fafc;
        min-height: 100vh;
    }

    .form-wrapper {
        background: white;
        border-radius: 6px;
        box-shadow: 0 1px 2px rgba(0,0,0,0.05);
        padding: 0.875rem;
        margin-top: 0.5rem;
    }

    h1 {
        color: var(--primary-darker);
        font-weight: 600;
        font-size: 1rem;
        margin-bottom: 0;
    }

    .form-section {
        background: #f9fafb;
        border-radius: 4px;
        padding: 0.5rem;
        margin-bottom: 0.5rem;
        border: 1px solid #e5e7eb;
    }

    .form-section-title {
        color: var(--primary-darker);
        font-weight: 600;
        font-size: 0.75rem;
        margin-bottom: 0.4rem;
        display: flex;
        align-items: center;
        gap: 4px;
        padding-bottom: 0.3rem;
        border-bottom: 1px solid #e5e7eb;
    }

    .form-section-title i {
        font-size: 0.85rem;
        color: var(--primary-color);
        background: #eff6ff;
        padding: 4px;
        border-radius: 4px;
        width: 22px;
        height: 22px;
        display: flex;
        align-items: center;
        justify-content: center;
    }

    .form-label {
        font-weight: 500;
        color: #374151;
        margin-bottom: 0.25rem;
        font-size: 0.7rem;
        display: flex;
        align-items: center;
        gap: 3px;
    }

    .form-label i {
        color: var(--primary-color);
        font-size: 0.65rem;
    }

    .form-control,
    .form-select {
        border: 1px solid #d1d5db;
        border-radius: 4px;
        padding: 0.3rem 0.5rem;
        font-size: 0.75rem;
        transition: all 0.2s ease;
        background: white;
    }

    .form-control:focus,
    .form-select:focus {
        border-color: var(--primary-color);
        box-shadow: 0 0 0 3px rgba(37, 99, 235, 0.1);
        outline: none;
    }

    .form-control:hover,
    .form-select:hover {
        border-color: #cbd5e1;
    }

    textarea.form-control {
        min-height: 60px;
        resize: vertical;
        font-size: 0.8rem;
    }

    input[type="file"].form-control {
        padding: 0.35rem;
        cursor: pointer;
        font-size: 0.75rem;
    }

    input[type="file"].form-control:hover {
        background: #f8f9fa;
    }

    .form-text {
        font-size: 0.7rem;
        color: #6b7280;
        margin-top: 0.2rem;
    }

    .form-text i {
        color: var(--primary-color);
    }

    .btn {
        border-radius: 4px;
        padding: 0.35rem 0.75rem;
        font-weight: 500;
        font-size: 0.75rem;
        transition: all 0.2s ease;
        border: none;
    }

    .btn-primary {
        background: var(--primary-color);
    }

    .btn-primary:hover {
        background: var(--primary-dark);
        transform: translateY(-1px);
    }

    .btn-secondary {
        background: #6b7280;
    }

    .btn-secondary:hover {
        background: #4b5563;
        transform: translateY(-1px);
    }

    .action-buttons {
        background: #f9fafb;
        border-radius: 4px;
        padding: 0.5rem;
        margin-top: 0.5rem;
        border-top: 1px solid #e5e7eb;
    }
    
    .mb-3 {
        margin-bottom: 0.5rem !important;
    }
    
    .mb-2 {
        margin-bottom: 0.4rem !important;
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

<div class="container-fluid">
    <div class="row mb-2">
        <div class="col-12">
            <div class="d-flex justify-content-between align-items-center">
                <h1 class="mb-0"><i class="fas fa-edit me-1"></i>Chỉnh sửa công việc</h1>
                <a href="@Url.Action("Index", "Tasks")" class="btn btn-secondary btn-sm">
                    <i class="fas fa-arrow-left me-1"></i>Quay lại
                </a>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-xl-8 col-lg-9 mx-auto">
            <div class="form-wrapper">
            @Using Html.BeginForm("Edit", "Tasks", FormMethod.Post, New With {.enctype = "multipart/form-data"})
                @Html.AntiForgeryToken()
                @Html.HiddenFor(Function(m) m.Id)
                @Html.HiddenFor(Function(m) m.CreatedDate)
                
                @<div>
                    @* Thông tin cơ bản *@
                    <div class="form-section">
                        <div class="form-section-title">
                            <i class="fas fa-info-circle"></i>
                            <span>Thông tin cơ bản</span>
                        </div>
                        <div class="row g-2">
                            <div class="col-md-3 mb-2">
                                @Html.LabelFor(Function(m) m.Tag, "Tag", New With {.class = "form-label"})
                                @Html.TextBoxFor(Function(m) m.Tag, New With {.class = "form-control", .placeholder = "Tag"})
                            </div>
                            <div class="col-md-3 mb-2">
                                <label class="form-label">Ngày nhận file</label>
                                @Code
                                    Dim fileReceivedDateStr = If(Model.FileReceivedDate.HasValue, Model.FileReceivedDate.Value.ToString("yyyy-MM-dd"), "")
                                End Code
                                <input type="date" name="FileReceivedDate" class="form-control" value="@fileReceivedDateStr" />
                            </div>
                            <div class="col-md-3 mb-2">
                                <label class="form-label">Nền tảng hỗ trợ *</label>
                                @Code
                                    Dim selZalo = If(Model.SupportPlatform = SupportPlatform.Zalo, "selected", "")
                                    Dim selMember = If(Model.SupportPlatform = SupportPlatform.MemberSupport, "selected", "")
                                    Dim selSale = If(Model.SupportPlatform = SupportPlatform.CustomerContactSale, "selected", "")
                                End Code
                                <select name="SupportPlatform" class="form-select" required>
                                    <option value="0" @selZalo>Zalo</option>
                                    <option value="1" @selMember>Member Support</option>
                                    <option value="2" @selSale>Khách liên hệ Sale</option>
                                </select>
                            </div>
                            <div class="col-md-3 mb-2">
                                <label class="form-label">Sale quản lý *</label>
                                <select name="SaleManagerId" class="form-select" required>
                                    <option value="">-- Chọn Sale --</option>
                                    @For Each sale In saleManagers
                                        @Code
                                            Dim isSelected = If(Model.SaleManagerId = sale.Id, "selected", "")
                                        End Code
                                        @<option value="@sale.Id" @isSelected>@sale.Name</option>
                                    Next
                                </select>
                            </div>
                            <div class="col-md-6 mb-2">
                                @Html.LabelFor(Function(m) m.CustomerName, "Khách hàng *", New With {.class = "form-label"})
                                @Html.TextBoxFor(Function(m) m.CustomerName, New With {.class = "form-control", .required = "required"})
                                @Html.ValidationMessageFor(Function(m) m.CustomerName, "", New With {.class = "text-danger"})
                            </div>
                            <div class="col-md-6 mb-2">
                                <label class="form-label">Phần mềm sử dụng *</label>
                                <select name="SoftwareId" class="form-select" id="softwareSelect" required>
                                    <option value="">-- Chọn phần mềm --</option>
                                    @For Each soft In software
                                        @Code
                                            Dim isSelected = If(Model.SoftwareId = soft.Id, "selected", "")
                                        End Code
                                        @<option value="@soft.Id" @isSelected>@soft.Name</option>
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
                        <div class="mb-2">
                            @Html.LabelFor(Function(m) m.Description, "Mô tả lỗi / nội dung hỗ trợ *", New With {.class = "form-label"})
                            @Html.TextAreaFor(Function(m) m.Description, New With {.class = "form-control", .id = "description-textarea-edit", .style = "display:none;", .required = "required"})
                            <div id="description-editor-edit" style="min-height: 120px; background: white; border: 1px solid #d1d5db; border-radius: 4px;"></div>
                            @Html.ValidationMessageFor(Function(m) m.Description, "", New With {.class = "text-danger", .style = "font-size: 0.7rem;"})
                        </div>
                    </div>

                    @* Phân công & Trạng thái & Thời gian *@
                    <div class="form-section">
                        <div class="form-section-title">
                            <i class="fas fa-user-cog"></i>
                            <span>Phân công, Trạng thái & Thời gian</span>
                        </div>
                        <div class="row g-2">
                            <div class="col-md-4 mb-2">
                                <label class="form-label">Kỹ thuật phụ trách *</label>
                                <select name="AssignedTo" class="form-select" required>
                                    <option value="">-- Chọn kỹ thuật --</option>
                                    @For Each tech In technicians
                                        @Code
                                            Dim isSelected = If(Model.AssignedTo = tech.Id, "selected", "")
                                        End Code
                                        @<option value="@tech.Id" @isSelected>@tech.Name</option>
                                    Next
                                </select>
                            </div>
                            <div class="col-md-4 mb-2">
                                <label class="form-label">Tình trạng *</label>
                                @Code
                                    Dim sel0 = If(Model.Status = TaskStatus.Pending, "selected", "")
                                    Dim sel1 = If(Model.Status = TaskStatus.InProgress, "selected", "")
                                    Dim sel2 = If(Model.Status = TaskStatus.Waiting, "selected", "")
                                    Dim sel3 = If(Model.Status = TaskStatus.Completed, "selected", "")
                                    Dim sel4 = If(Model.Status = TaskStatus.Paused, "selected", "")
                                End Code
                                <select name="Status" class="form-select" required>
                                    <option value="0" @sel0>🟡 Chưa xử lý</option>
                                    <option value="1" @sel1>🔵 Đang xử lý</option>
                                    <option value="2" @sel2>🟠 Chờ phản hồi</option>
                                    <option value="3" @sel3>🟢 Đã hoàn thành</option>
                                    <option value="4" @sel4>🔴 Quá hạn</option>
                                </select>
                            </div>
                            <div class="col-md-4 mb-2">
                                <label class="form-label">Ngày dự kiến</label>
                                @Code
                                    Dim expectedDateStr = If(Model.ExpectedCompletionDate.HasValue, Model.ExpectedCompletionDate.Value.ToString("yyyy-MM-dd"), "")
                                End Code
                                <input type="date" name="ExpectedCompletionDate" class="form-control" value="@expectedDateStr" />
                            </div>
                            <div class="col-md-4 mb-2">
                                <label class="form-label">Ngày hoàn thành</label>
                                @Code
                                    Dim completedDateStr = If(Model.CompletedDate.HasValue, Model.CompletedDate.Value.ToString("yyyy-MM-dd"), "")
                                End Code
                                <input type="date" name="CompletedDate" class="form-control" value="@completedDateStr" />
                            </div>
                        </div>
                    </div>

                    @* Giải pháp & Phản hồi *@
                    <div class="form-section">
                        <div class="form-section-title">
                            <i class="fas fa-lightbulb"></i>
                            <span>Giải pháp & Phản hồi</span>
                        </div>
                        <div class="row g-2">
                            <div class="col-md-6 mb-2">
                                @Html.LabelFor(Function(m) m.Solution, "Phương án khắc phục", New With {.class = "form-label"})
                                @Html.TextAreaFor(Function(m) m.Solution, New With {.class = "form-control", .rows = "3", .placeholder = "Mô tả nguyên nhân và hướng xử lý..."})
                            </div>
                            <div class="col-md-6 mb-2">
                                @Html.LabelFor(Function(m) m.ResponseToCustomer, "Nội dung trả lời khách hàng", New With {.class = "form-label"})
                                @Html.TextAreaFor(Function(m) m.ResponseToCustomer, New With {.class = "form-control", .rows = "3", .placeholder = "Nội dung trả lời cho khách hàng..."})
                            </div>
                        </div>
                    </div>

                    @* File đính kèm *@
                    <div class="form-section">
                        <div class="form-section-title">
                            <i class="fas fa-paperclip"></i>
                            <span>File đính kèm</span>
                        </div>
                        
                        @If hasImages Then
                            @<div class="mb-2">
                                <label class="form-label" style="font-size: 0.7rem;">Hình ảnh hiện tại</label>
                                <div class="row g-1">
                                    @For Each img In Model.Images
                                        @<div class="col-3">
                                            <img src="@img" class="img-thumbnail image-zoom" style="max-height: 60px; width: 100%; object-fit: cover; padding: 2px; cursor: pointer;" data-image-src="@img" />
                                        </div>
                                    Next
                                </div>
                            </div>
                        End If
                        
                        <div class="row g-2">
                            <div class="col-md-6 mb-2">
                                <label class="form-label">
                                    <i class="fas fa-image me-1"></i>@If hasImages Then @<span>Thêm hình ảnh</span> Else @<span>Hình ảnh</span> End If
                                </label>
                                <div class="upload-zone" id="imageUploadZone">
                                    <input type="file" name="uploadedImages" id="uploadedImages" class="d-none" multiple accept="image/*" />
                                    <div class="upload-area" onclick="document.getElementById('uploadedImages').click()">
                                        <i class="fas fa-cloud-upload-alt fa-2x mb-2"></i>
                                        <p class="mb-1">Kéo thả ảnh vào đây hoặc click để chọn</p>
                                        <p class="small text-muted mb-0">Hoặc dán ảnh từ clipboard (Ctrl+V)</p>
                                    </div>
                                    <div class="upload-preview" id="imagePreview"></div>
                                </div>
                            </div>
                            <div class="col-md-6 mb-2">
                                <label class="form-label">
                                    <i class="fas fa-file me-1"></i>@If hasAttachments Then @<span>Thêm file</span> Else @<span>File</span> End If
                                </label>
                                <div class="upload-zone" id="fileUploadZone">
                                    <input type="file" name="uploadedFiles" id="uploadedFiles" class="d-none" multiple />
                                    <div class="upload-area" onclick="document.getElementById('uploadedFiles').click()">
                                        <i class="fas fa-cloud-upload-alt fa-2x mb-2"></i>
                                        <p class="mb-1">Kéo thả file vào đây hoặc click để chọn</p>
                                        <p class="small text-muted mb-0">Có thể chọn nhiều file</p>
                                    </div>
                                    <div class="upload-preview" id="filePreview"></div>
                                </div>
                            </div>
                        </div>
                        
                        @If hasAttachments Then
                            @<div class="mb-2">
                                <label class="form-label" style="font-size: 0.7rem;">File hiện tại</label>
                                <div class="d-flex flex-wrap gap-1">
                                    @For Each att In Model.Attachments
                                        @<a href="@att" target="_blank" class="badge bg-secondary text-decoration-none" style="font-size: 0.7rem;">
                                            <i class="fas fa-file me-1"></i>@System.IO.Path.GetFileName(att)
                                        </a>
                                    Next
                                </div>
                            </div>
                        End If
                    </div>

                    @* Nút hành động *@
                    <div class="action-buttons d-flex justify-content-end align-items-center gap-2">
                        <button type="submit" class="btn btn-primary">
                            <i class="fas fa-save me-1"></i>Cập nhật
                        </button>
                    </div>
                </div>
            End Using
            </div>
        </div>
    </div>
</div>

<!-- Quill Rich Text Editor -->
<link href="https://cdn.quilljs.com/1.3.6/quill.snow.css" rel="stylesheet">
<script src="https://cdn.quilljs.com/1.3.6/quill.js"></script>

<style>
    #description-editor-edit {
        border: 1px solid #d1d5db;
        border-radius: 6px;
    }
    
    #description-editor-edit .ql-container {
        font-size: 0.75rem;
        min-height: 120px;
    }
    
    #description-editor-edit .ql-editor {
        min-height: 120px;
        padding: 0.4rem;
    }
    
    #description-editor-edit .ql-toolbar {
        padding: 0.3rem;
    }
    
    #description-editor-edit .ql-toolbar .ql-formats {
        margin-right: 0.5rem;
    }
    
    #description-editor-edit .ql-toolbar {
        border-top-left-radius: 6px;
        border-top-right-radius: 6px;
        border-bottom: 1px solid #d1d5db;
        background: #f8f9fa;
    }
    
    #description-editor-edit .ql-stroke {
        stroke: #374151;
    }
    
    #description-editor-edit .ql-fill {
        fill: #374151;
    }
    
    #description-editor-edit .ql-picker-label {
        color: #374151;
    }
    
    #description-editor-edit .ql-toolbar button:hover,
    #description-editor-edit .ql-toolbar button.ql-active {
        color: var(--primary-color);
    }
    
    #description-editor-edit .ql-toolbar button:hover .ql-stroke,
    #description-editor-edit .ql-toolbar button.ql-active .ql-stroke {
        stroke: var(--primary-color);
    }
    
    #description-editor-edit .ql-toolbar button:hover .ql-fill,
    #description-editor-edit .ql-toolbar button.ql-active .ql-fill {
        fill: var(--primary-color);
    }
</style>

<script>
    document.addEventListener('DOMContentLoaded', function() {
        // Khởi tạo Quill editor
        var quill = new Quill('#description-editor-edit', {
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
            placeholder: 'Nhập mô tả chi tiết về lỗi hoặc nội dung cần hỗ trợ...'
        });
        
        // Lấy textarea ẩn
        var textarea = document.getElementById('description-textarea-edit');
        
        // Load nội dung hiện có vào editor
        if (textarea && textarea.value) {
            quill.root.innerHTML = textarea.value;
        }
        
        // Sync nội dung từ Quill vào textarea khi thay đổi
        quill.on('text-change', function() {
            var html = quill.root.innerHTML;
            // Nếu chỉ có <p><br></p> thì coi như rỗng
            if (html === '<p><br></p>' || html === '<p></p>') {
                textarea.value = '';
            } else {
                textarea.value = html;
            }
        });
        
        // Validate trước khi submit
        var form = document.querySelector('form');
        if (form) {
            form.addEventListener('submit', function(e) {
                var content = quill.root.innerHTML;
                // Kiểm tra nếu chỉ có thẻ p rỗng
                if (content === '<p><br></p>' || content === '<p></p>' || content.trim() === '') {
                    e.preventDefault();
                    alert('Vui lòng nhập mô tả lỗi / nội dung hỗ trợ!');
                    quill.focus();
                    return false;
                }
                // Cập nhật textarea trước khi submit
                textarea.value = content;
            });
        }
    });
    
    // Image Lightbox
    document.addEventListener('DOMContentLoaded', function() {
        const images = document.querySelectorAll('.image-zoom');
        if (images.length > 0) {
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
        }
        
        // Upload Zone Functionality
        const imageInput = document.getElementById('uploadedImages');
        const fileInput = document.getElementById('uploadedFiles');
        const imageZone = document.getElementById('imageUploadZone');
        const fileZone = document.getElementById('fileUploadZone');
        const imagePreview = document.getElementById('imagePreview');
        const filePreview = document.getElementById('filePreview');
        
        let imageFiles = [];
        let fileFiles = [];
        
        if (imageInput && imageZone) {
            setupUploadZone(imageZone, imageInput, imagePreview, imageFiles, true);
        }
        
        if (fileInput && fileZone) {
            setupUploadZone(fileZone, fileInput, filePreview, fileFiles, false);
        }
        
        // Paste images from clipboard
        document.addEventListener('paste', function(e) {
            const items = e.clipboardData.items;
            for (let i = 0; i < items.length; i++) {
                if (items[i].type.indexOf('image') !== -1) {
                    const blob = items[i].getAsFile();
                    const file = new File([blob], 'pasted-image-' + Date.now() + '.png', { type: blob.type });
                    if (imageInput && imagePreview && imageFiles) {
                        addFileToInput(file, imageInput, imagePreview, imageFiles, true);
                    }
                }
            }
        });
        
        function setupUploadZone(zone, input, preview, filesArray, isImage) {
            const uploadArea = zone.querySelector('.upload-area');
            
            // Click to select
            input.addEventListener('change', function(e) {
                handleFiles(e.target.files, input, preview, filesArray, isImage);
            });
            
            // Drag and drop
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
                handleFiles(e.dataTransfer.files, input, preview, filesArray, isImage);
            });
        }
        
        function handleFiles(files, input, preview, filesArray, isImage) {
            for (let i = 0; i < files.length; i++) {
                const file = files[i];
                if (isImage && file.type.startsWith('image/')) {
                    addFileToInput(file, input, preview, filesArray, true);
                } else if (!isImage) {
                    addFileToInput(file, input, preview, filesArray, false);
                }
            }
            updateFileInput(input, filesArray);
        }
        
        function addFileToInput(file, input, preview, filesArray, isImage) {
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
                    updateFileInput(input, filesArray);
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
        
        function updateFileInput(input, filesArray) {
            const dataTransfer = new DataTransfer();
            filesArray.forEach(file => dataTransfer.items.add(file));
            input.files = dataTransfer.files;
        }
    });
</script>

<style>
    /* Upload Zone Styles */
    .upload-zone {
        position: relative;
    }
    
    .upload-area {
        border: 2px dashed #d1d5db;
        border-radius: 8px;
        padding: 2rem;
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
        width: 100px;
        height: 100px;
        object-fit: cover;
        display: block;
    }
    
    .preview-item .file-icon {
        width: 100px;
        height: 100px;
        display: flex;
        align-items: center;
        justify-content: center;
        background: #f3f4f6;
        color: #6b7280;
        font-size: 2rem;
    }
    
    .preview-item .file-name {
        padding: 0.25rem 0.5rem;
        font-size: 0.7rem;
        color: #374151;
        max-width: 100px;
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