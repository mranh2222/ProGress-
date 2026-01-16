@ModelType Question
@Code
    ViewData("Title") = "Gửi yêu cầu hỗ trợ"
    Dim technicians As List(Of Technician) = If(ViewBag.Technicians, New List(Of Technician))
End Code

<div class="container-fluid py-4">
    <div class="row">
        <div class="col-xl-8 col-lg-9 mx-auto">
            <div class="card">
                <div class="card-header">
                    <h4 class="mb-0">
                        <i class="fas fa-headset me-2"></i>Gửi yêu cầu hỗ trợ
                    </h4>
                </div>
                <div class="card-body">
                    @If TempData("ErrorMessage") IsNot Nothing Then
                        @<div class="alert alert-danger alert-dismissible fade show" role="alert">
                            <i class="fas fa-exclamation-circle me-2"></i>@TempData("ErrorMessage")
                            <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                        </div>
                    End If

                    @If Not ViewData.ModelState.IsValid Then
                        @Code
                            Dim allErrors As New List(Of String)
                            For Each state In ViewData.ModelState.Values
                                For Each modelError In state.Errors
                                    If Not String.IsNullOrEmpty(modelError.ErrorMessage) Then
                                        allErrors.Add(modelError.ErrorMessage)
                                    End If
                                Next
                            Next
                        End Code
                        @If allErrors.Count > 0 Then
                            @<div class="alert alert-danger alert-dismissible fade show" role="alert">
                                <i class="fas fa-exclamation-triangle me-2"></i>
                                <strong>Vui lòng kiểm tra lại các thông tin sau:</strong>
                                <ul class="mb-0 mt-2">
                                    @For Each errorMsg In allErrors
                                        @<li>@errorMsg</li>
                                    Next
                                </ul>
                                <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                            </div>
                        End If
                    End If

                    @Using Html.BeginForm("Create", "Questions", FormMethod.Post, New With {.id = "createQuestionForm", .enctype = "multipart/form-data"})
                        @Html.AntiForgeryToken()
                        
                        @<div class="mb-3">
                            <label class="form-label required-field">
                                <i class="fas fa-user me-2"></i>Kỹ thuật viên <span class="text-danger">*</span>
                            </label>
                            <select name="technicianId" id="technicianId" class="form-select" required>
                                <option value="">-- Chọn kỹ thuật viên --</option>
                                @For Each tech In technicians
                                    @<option value="@tech.Id">@tech.Name</option>
                                Next
                            </select>
                            @Html.ValidationMessage("TechnicianId", "", New With {.class = "text-danger"})
                        </div>

                        @<div class="mb-3">
                            <label class="form-label required-field">
                                <i class="fas fa-comment-dots me-2"></i>Nội dung yêu cầu hỗ trợ <span class="text-danger">*</span>
                            </label>
                            @Html.TextAreaFor(Function(m) m.Question, New With {.class = "form-control", .id = "question-textarea", .style = "display:none;", .required = "required"})
                            <div id="question-editor" style="min-height: 200px; background: white; border: 1px solid #d1d5db; border-radius: 8px;"></div>
                            @Html.ValidationMessageFor(Function(m) m.Question, "", New With {.class = "text-danger"})
                            <small class="form-text text-muted">
                                <i class="fas fa-info-circle me-1"></i>Vui lòng mô tả vấn đề một cách chi tiết và rõ ràng để được hỗ trợ tốt nhất.
                            </small>
                        </div>

                        @<div class="mb-3">
                            <label class="form-label">
                                <i class="fas fa-image me-2"></i>Hình ảnh đính kèm
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
                            <small class="form-text text-muted">
                                <i class="fas fa-info-circle me-1"></i>Có thể chọn nhiều hình ảnh, kéo thả, hoặc copy/paste (JPG, PNG, GIF)
                            </small>
                        </div>

                        @<div class="mb-3">
                            <label class="form-label">
                                <i class="fas fa-file me-2"></i>File đính kèm
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
                            <small class="form-text text-muted">
                                <i class="fas fa-info-circle me-1"></i>Có thể chọn nhiều file hoặc kéo thả (PDF, DOC, DOCX, XLS, XLSX, ZIP, RAR...)
                            </small>
                        </div>

                        @<div class="d-flex justify-content-between align-items-center">
                            <a href="@Url.Action("Index", "Questions")" class="btn btn-secondary">
                                <i class="fas fa-arrow-left me-2"></i>Quay lại
                            </a>
                            <button type="submit" class="btn btn-primary" id="submitBtn">
                                <i class="fas fa-paper-plane me-2"></i>Gửi yêu cầu
                            </button>
                        </div>
                    End Using
                </div>
            </div>
        </div>
    </div>
</div>

<style>
    .required-field {
        font-weight: 600;
        color: var(--dark-color);
    }
    
    .form-control, .form-select {
        font-size: 0.9rem;
        border-radius: 8px;
        border: 1px solid #d1d5db;
        padding: 0.65rem 0.85rem;
    }
    
    .form-control:focus, .form-select:focus {
        border-color: var(--primary-color);
        box-shadow: 0 0 0 4px rgba(37, 99, 235, 0.15);
        outline: none;
    }
    
    textarea.form-control {
        resize: vertical;
        min-height: 150px;
    }
    
    .card {
        border: none;
        box-shadow: 0 4px 12px rgba(0, 0, 0, 0.08);
        border-radius: 12px;
    }
    
    .card-header {
        background: linear-gradient(135deg, var(--primary-color) 0%, var(--primary-darker) 100%);
        color: white;
        border-radius: 12px 12px 0 0 !important;
        padding: 1.25rem 1.5rem;
    }
    
    .card-body {
        padding: 2rem;
    }
</style>

<!-- Quill Rich Text Editor -->
<link href="https://cdn.quilljs.com/1.3.6/quill.snow.css" rel="stylesheet">
<script src="https://cdn.quilljs.com/1.3.6/quill.js"></script>

<style>
    #question-editor {
        border: 1px solid #d1d5db;
        border-radius: 8px;
    }
    
    #question-editor .ql-container {
        font-size: 0.9rem;
        min-height: 200px;
    }
    
    #question-editor .ql-editor {
        min-height: 200px;
    }
    
    #question-editor .ql-toolbar {
        border-top-left-radius: 8px;
        border-top-right-radius: 8px;
        border-bottom: 1px solid #d1d5db;
        background: #f8f9fa;
    }
    
    #question-editor .ql-stroke {
        stroke: #374151;
    }
    
    #question-editor .ql-fill {
        fill: #374151;
    }
    
    #question-editor .ql-picker-label {
        color: #374151;
    }
    
    #question-editor .ql-toolbar button:hover,
    #question-editor .ql-toolbar button.ql-active {
        color: var(--primary-color);
    }
    
    #question-editor .ql-toolbar button:hover .ql-stroke,
    #question-editor .ql-toolbar button.ql-active .ql-stroke {
        stroke: var(--primary-color);
    }
    
    #question-editor .ql-toolbar button:hover .ql-fill,
    #question-editor .ql-toolbar button.ql-active .ql-fill {
        fill: var(--primary-color);
    }
</style>

<script>
    document.addEventListener('DOMContentLoaded', function() {
        // Khởi tạo Quill editor
        var quill = new Quill('#question-editor', {
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
            placeholder: 'Mô tả chi tiết vấn đề cần hỗ trợ...'
        });
        
        // Lấy textarea ẩn
        var textarea = document.getElementById('question-textarea');
        
        // Sync nội dung từ Quill vào textarea khi thay đổi
        quill.on('text-change', function() {
            var html = quill.root.innerHTML;
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
        if (imageZone && imageInput) {
            setupUploadZone(imageZone, imageInput, imagePreview, imageFiles, true);
        }
        
        // File Upload Zone
        if (fileZone && fileInput) {
            setupUploadZone(fileZone, fileInput, filePreview, fileFiles, false);
        }
        
        // Paste images from clipboard
        document.addEventListener('paste', function(e) {
            const items = e.clipboardData.items;
            for (let i = 0; i < items.length; i++) {
                if (items[i].type.indexOf('image') !== -1) {
                    const blob = items[i].getAsFile();
                    const file = new File([blob], 'pasted-image-' + Date.now() + '.png', { type: blob.type });
                    if (imageInput && imagePreview) {
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
        
        // Validate và submit form
        const form = document.getElementById('createQuestionForm');
        if (form) {
            form.addEventListener('submit', function(e) {
                const technicianId = document.getElementById('technicianId').value;
                const content = quill.root.innerHTML;
                
                // Kiểm tra nếu chỉ có thẻ p rỗng
                if (content === '<p><br></p>' || content === '<p></p>' || content.trim() === '') {
                    e.preventDefault();
                    alert('Vui lòng nhập nội dung yêu cầu hỗ trợ!');
                    quill.focus();
                    return false;
                }
                
                if (!technicianId || technicianId.trim() === '') {
                    e.preventDefault();
                    alert('Vui lòng chọn kỹ thuật viên');
                    return false;
                }
                
                // Cập nhật textarea trước khi submit
                textarea.value = content;
                
                // Show loading
                const submitBtn = document.getElementById('submitBtn');
                if (submitBtn) {
                    submitBtn.disabled = true;
                    submitBtn.innerHTML = '<i class="fas fa-spinner fa-spin me-2"></i>Đang gửi...';
                }
            });
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

