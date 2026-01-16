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
        border-radius: 12px;
        box-shadow: 0 4px 20px rgba(34, 197, 94, 0.1);
        padding: 1.5rem;
        margin-top: 0.5rem;
        max-width: 900px;
        margin-left: auto;
        margin-right: auto;
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

    .form-section-title {
        color: var(--primary-darker);
        font-weight: 600;
        font-size: 0.85rem;
        margin-bottom: 0.75rem;
        display: flex;
        align-items: center;
        gap: 8px;
        padding-bottom: 0.5rem;
        border-bottom: 1px solid #e5e7eb;
    }

    .form-section-title i {
        font-size: 0.9rem;
        color: var(--primary-color);
    }

    .form-label {
        font-weight: 500;
        color: #374151;
        margin-bottom: 0.4rem;
        font-size: 0.8rem;
        display: flex;
        align-items: center;
        gap: 4px;
    }

    .form-label i {
        color: var(--primary-color);
        font-size: 0.75rem;
    }

    .form-control,
    .form-select {
        border: 1px solid #d1d5db;
        border-radius: 6px;
        padding: 0.4rem 0.65rem;
        font-size: 0.8rem;
        transition: all 0.2s ease;
        background: white;
    }

    .form-control:focus,
    .form-select:focus {
        border-color: var(--primary-color);
        box-shadow: 0 0 0 3px rgba(34, 197, 94, 0.1);
        outline: none;
    }

    textarea.form-control {
        min-height: 80px;
        resize: vertical;
    }

    input[type="file"].form-control {
        padding: 0.4rem;
        cursor: pointer;
        font-size: 0.8rem;
    }

    .form-text {
        font-size: 0.7rem;
        color: #6b7280;
        margin-top: 0.25rem;
    }

    .btn {
        border-radius: 6px;
        padding: 0.4rem 1rem;
        font-weight: 500;
        font-size: 0.8rem;
        transition: all 0.2s ease;
        border: none;
    }

    .btn-primary {
        background: var(--primary-color);
        box-shadow: 0 2px 8px rgba(34, 197, 94, 0.2);
    }

    .btn-primary:hover {
        background: var(--primary-dark);
        box-shadow: 0 3px 12px rgba(34, 197, 94, 0.3);
    }

    .btn-secondary {
        background: #6b7280;
    }

    .btn-secondary:hover {
        background: #4b5563;
    }

    .action-buttons {
        background: #f8f9fa;
        border-radius: 8px;
        padding: 1rem;
        margin-top: 1rem;
        border-top: 2px solid var(--primary-color);
    }

    .required-field::after {
        content: " *";
        color: #ef4444;
        font-weight: 600;
    }

    .mb-3 {
        margin-bottom: 0.75rem !important;
    }

    .mb-4 {
        margin-bottom: 1rem !important;
    }

    @@media (max-width: 768px) {
        .form-wrapper {
            padding: 1rem;
        }

        .form-section {
            padding: 0.75rem;
        }
    }
</style>

<div class="container-fluid">
    <div class="row mb-3">
        <div class="col-12 text-center">
            <h1><i class="fas fa-plus-circle me-2"></i>Tạo công việc mới</h1>
        </div>
    </div>

    <div class="row">
        <div class="col-12">
            <div class="form-wrapper">
                @Using Html.BeginForm("Create", "Tasks", FormMethod.Post, New With {.enctype = "multipart/form-data"})
                    @Html.AntiForgeryToken()
                    
                    @<div>
                    @* Thông tin cơ bản - Gộp với Nền tảng & Sale *@
                    <div class="form-section">
                        <div class="form-section-title">
                            <i class="fas fa-info-circle"></i>
                            <span>Thông tin cơ bản</span>
                        </div>
                        <div class="row">
                            <div class="col-md-3 mb-3">
                                <label class="form-label">
                                    <i class="fas fa-tag"></i>Tag
                                </label>
                                @Html.TextBoxFor(Function(m) m.Tag, New With {.class = "form-control", .placeholder = "Tag công việc"})
                            </div>
                            <div class="col-md-3 mb-3">
                                <label class="form-label">
                                    <i class="fas fa-calendar-check"></i>Ngày nhận file
                                </label>
                                <input type="date" name="FileReceivedDate" class="form-control" />
                            </div>
                            <div class="col-md-3 mb-3">
                                <label class="form-label required-field">
                                    <i class="fas fa-headset"></i>Nền tảng hỗ trợ
                                </label>
                                <select name="SupportPlatform" class="form-select" required>
                                    <option value="0">📱 Zalo</option>
                                    <option value="1">💬 Member Support</option>
                                    <option value="2">👥 Khách liên hệ Sale</option>
                                </select>
                            </div>
                            <div class="col-md-3 mb-3">
                                <label class="form-label required-field">
                                    <i class="fas fa-user-tie"></i>Sale quản lý
                                </label>
                                <select name="SaleManagerId" class="form-select" required>
                                    <option value="">-- Chọn Sale --</option>
                                    @For Each sale In saleManagers
                                        @<option value="@sale.Id">@sale.Name</option>
                                    Next
                                </select>
                            </div>
                        </div>
                    </div>

                    @* Thông tin khách hàng & Phần mềm *@
                    <div class="form-section">
                        <div class="form-section-title">
                            <i class="fas fa-user-tie"></i>
                            <span>Khách hàng & Phần mềm</span>
                        </div>
                        <div class="row">
                            <div class="col-md-6 mb-3">
                                <label class="form-label required-field">
                                    <i class="fas fa-user-tie"></i>Khách hàng
                                </label>
                                @Html.TextBoxFor(Function(m) m.CustomerName, New With {.class = "form-control", .required = "required", .placeholder = "Tên khách hàng"})
                                @Html.ValidationMessageFor(Function(m) m.CustomerName, "", New With {.class = "text-danger"})
                            </div>
                            <div class="col-md-6 mb-3">
                                <label class="form-label required-field">
                                    <i class="fas fa-laptop-code"></i>Phần mềm
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
                        <div class="mb-3">
                            <label class="form-label required-field">
                                <i class="fas fa-file-alt"></i>Mô tả lỗi / nội dung hỗ trợ
                            </label>
                            @Html.TextAreaFor(Function(m) m.Description, New With {.class = "form-control", .id = "description-textarea", .style = "display:none;", .required = "required"})
                            <div id="description-editor" style="min-height: 200px; background: white; border: 1px solid #d1d5db; border-radius: 6px;"></div>
                            @Html.ValidationMessageFor(Function(m) m.Description, "", New With {.class = "text-danger"})
                        </div>
                    </div>

                    @* Phân công & Trạng thái & Thời gian - Gộp lại *@
                    <div class="form-section">
                        <div class="form-section-title">
                            <i class="fas fa-user-cog"></i>
                            <span>Phân công & Thời gian</span>
                        </div>
                        <div class="row">
                            <div class="col-md-4 mb-3">
                                <label class="form-label required-field">
                                    <i class="fas fa-user-cog"></i>Kỹ thuật phụ trách
                                </label>
                                <select name="AssignedTo" class="form-select" required>
                                    <option value="">-- Chọn kỹ thuật --</option>
                                    @For Each tech In technicians
                                        @<option value="@tech.Id">@tech.Name</option>
                                    Next
                                </select>
                            </div>
                            <div class="col-md-4 mb-3">
                                <label class="form-label required-field">
                                    <i class="fas fa-info-circle"></i>Tình trạng
                                </label>
                                <select name="Status" class="form-select" required>
                                    <option value="0">🟡 Chưa xử lý</option>
                                    <option value="1">🔵 Đang xử lý</option>
                                    <option value="2">🟠 Chờ phản hồi</option>
                                    <option value="3">🟢 Đã hoàn thành</option>
                                    <option value="4">🔴 Quá hạn</option>
                                </select>
                            </div>
                            <div class="col-md-4 mb-3">
                                <label class="form-label">
                                    <i class="fas fa-calendar-alt"></i>Ngày dự kiến hoàn thành
                                </label>
                                <input type="date" name="ExpectedCompletionDate" class="form-control" />
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
                                    <i class="fas fa-image"></i>Hình ảnh
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
                                <small class="form-text">Có thể chọn nhiều hình ảnh, kéo thả, hoặc copy/paste</small>
                            </div>
                            <div class="col-md-6 mb-3">
                                <label class="form-label">
                                    <i class="fas fa-file"></i>File đính kèm
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
                                <small class="form-text">Có thể chọn nhiều file hoặc kéo thả</small>
                            </div>
                        </div>
                    </div>

                    @* Nút hành động *@
                    <div class="action-buttons d-flex justify-content-between align-items-center">
                        <a href="@Url.Action("Index", "Tasks")" class="btn btn-secondary">
                            <i class="fas fa-arrow-left me-2"></i>Quay lại
                        </a>
                        <button type="submit" class="btn btn-primary">
                            <i class="fas fa-save me-2"></i>Lưu công việc
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
    #description-editor {
        border: 1px solid #d1d5db;
        border-radius: 6px;
    }
    
    #description-editor .ql-container {
        font-size: 0.875rem;
        min-height: 200px;
    }
    
    #description-editor .ql-editor {
        min-height: 200px;
    }
    
    #description-editor .ql-toolbar {
        border-top-left-radius: 6px;
        border-top-right-radius: 6px;
        border-bottom: 1px solid #d1d5db;
        background: #f8f9fa;
    }
    
    #description-editor .ql-stroke {
        stroke: #374151;
    }
    
    #description-editor .ql-fill {
        fill: #374151;
    }
    
    #description-editor .ql-picker-label {
        color: #374151;
    }
    
    #description-editor .ql-toolbar button:hover,
    #description-editor .ql-toolbar button.ql-active {
        color: var(--primary-color);
    }
    
    #description-editor .ql-toolbar button:hover .ql-stroke,
    #description-editor .ql-toolbar button.ql-active .ql-stroke {
        stroke: var(--primary-color);
    }
    
    #description-editor .ql-toolbar button:hover .ql-fill,
    #description-editor .ql-toolbar button.ql-active .ql-fill {
        fill: var(--primary-color);
    }
    
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

<script>
    document.addEventListener('DOMContentLoaded', function() {
        // Khởi tạo Quill editor
        var quill = new Quill('#description-editor', {
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
        var textarea = document.getElementById('description-textarea');
        
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
        
        // Sync nội dung từ textarea vào Quill khi load (nếu có giá trị cũ)
        if (textarea.value) {
            quill.root.innerHTML = textarea.value;
        }
        
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
        
        // Upload Zone Functionality
        const imageInput = document.getElementById('uploadedImages');
        const fileInput = document.getElementById('uploadedFiles');
        const imageZone = document.getElementById('imageUploadZone');
        const fileZone = document.getElementById('fileUploadZone');
        const imagePreview = document.getElementById('imagePreview');
        const filePreview = document.getElementById('filePreview');
        
        let imageFiles = [];
        let fileFiles = [];
        
        // Image Upload Zone
        setupUploadZone(imageZone, imageInput, imagePreview, imageFiles, true);
        
        // File Upload Zone
        setupUploadZone(fileZone, fileInput, filePreview, fileFiles, false);
        
        // Paste images from clipboard
        document.addEventListener('paste', function(e) {
            const items = e.clipboardData.items;
            for (let i = 0; i < items.length; i++) {
                if (items[i].type.indexOf('image') !== -1) {
                    const blob = items[i].getAsFile();
                    const file = new File([blob], 'pasted-image-' + Date.now() + '.png', { type: blob.type });
                    addFileToInput(file, imageInput, imagePreview, imageFiles, true);
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