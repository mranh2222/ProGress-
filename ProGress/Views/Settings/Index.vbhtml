@Code
    ViewData("Title") = "Cài đặt"
End Code

<div class="container-fluid py-3">
    <div class="d-flex justify-content-between align-items-center mb-4">
        <h4 class="mb-0" style="font-weight: 600; color: #1f2937;">
            <i class="fas fa-cog me-2" style="color: var(--primary-color);"></i>Cài đặt hệ thống
        </h4>
    </div>

    <div class="row g-3">
        <!-- Cấu hình Firebase -->
        <div class="col-lg-8">
            <div class="card shadow-sm mb-3">
                <div class="card-header-custom">
                    <h6 class="mb-0" style="font-size: 0.875rem; font-weight: 600; color: #1f2937;">
                        <i class="fas fa-database me-2" style="color: #f59e0b;"></i>Cấu hình Firebase
                    </h6>
                </div>
                <div class="card-body p-3">
                    <div class="mb-3">
                        <label class="form-label small mb-1" style="font-weight: 600; color: #374151;">
                            Firebase Realtime Database URL
                        </label>
                        <div class="input-group">
                            <input type="text" class="form-control form-control-sm" value="@ViewBag.FirebaseUrl" readonly style="background: #f9fafb; font-family: monospace; font-size: 0.8rem;">
                            <button class="btn btn-outline-secondary btn-sm" type="button" onclick="copyToClipboard('@ViewBag.FirebaseUrl')" title="Sao chép">
                                <i class="fas fa-copy"></i>
                            </button>
                        </div>
                        <small class="form-text text-muted" style="font-size: 0.7rem;">
                            <i class="fas fa-info-circle me-1"></i>URL của Firebase Realtime Database
                        </small>
                    </div>
                    
                    <div class="mb-0">
                        <label class="form-label small mb-1" style="font-weight: 600; color: #374151;">
                            Firebase Storage Bucket
                        </label>
                        <div class="input-group">
                            <input type="text" class="form-control form-control-sm" value="@ViewBag.FirebaseStorageBucket" readonly style="background: #f9fafb; font-family: monospace; font-size: 0.8rem;">
                            <button class="btn btn-outline-secondary btn-sm" type="button" onclick="copyToClipboard('@ViewBag.FirebaseStorageBucket')" title="Sao chép">
                                <i class="fas fa-copy"></i>
                            </button>
                        </div>
                        <small class="form-text text-muted" style="font-size: 0.7rem;">
                            <i class="fas fa-info-circle me-1"></i>Tên bucket của Firebase Storage
                        </small>
                    </div>
                </div>
            </div>

            <!-- Thông tin hệ thống -->
            <div class="card shadow-sm mb-3">
                <div class="card-header-custom">
                    <h6 class="mb-0" style="font-size: 0.875rem; font-weight: 600; color: #1f2937;">
                        <i class="fas fa-info-circle me-2" style="color: #6366f1;"></i>Thông tin hệ thống
                    </h6>
                </div>
                <div class="card-body p-3">
                    <div class="row g-3">
                        <div class="col-md-6">
                            <label class="form-label small mb-1" style="font-weight: 600; color: #374151;">Tên ứng dụng</label>
                            <p class="mb-0 small">ProGress - Hệ thống quản lý công việc</p>
                        </div>
                        <div class="col-md-6">
                            <label class="form-label small mb-1" style="font-weight: 600; color: #374151;">Phiên bản</label>
                            <p class="mb-0 small">1.0.0</p>
                        </div>
                        <div class="col-md-6">
                            <label class="form-label small mb-1" style="font-weight: 600; color: #374151;">Framework</label>
                            <p class="mb-0 small">ASP.NET MVC 5.2.9</p>
                        </div>
                        <div class="col-md-6">
                            <label class="form-label small mb-1" style="font-weight: 600; color: #374151;">Database</label>
                            <p class="mb-0 small">Firebase Realtime Database</p>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Hướng dẫn -->
        <div class="col-lg-4">
            <div class="card shadow-sm mb-3">
                <div class="card-header-custom">
                    <h6 class="mb-0" style="font-size: 0.875rem; font-weight: 600; color: #1f2937;">
                        <i class="fas fa-book me-2" style="color: #10b981;"></i>Hướng dẫn
                    </h6>
                </div>
                <div class="card-body p-3">
                    <div class="mb-3">
                        <h6 class="small mb-2" style="font-weight: 600; color: #374151;">
                            <i class="fas fa-cog me-1"></i>Cấu hình Firebase
                        </h6>
                        <p class="small text-muted mb-0">
                            Để thay đổi cấu hình Firebase, vui lòng chỉnh sửa file <code style="font-size: 0.7rem;">Web.config</code> trong thư mục gốc của dự án.
                        </p>
                    </div>
                    <div class="mb-0">
                        <h6 class="small mb-2" style="font-weight: 600; color: #374151;">
                            <i class="fas fa-shield-alt me-1"></i>Bảo mật
                        </h6>
                        <p class="small text-muted mb-0">
                            Không chia sẻ thông tin cấu hình Firebase với người không có quyền truy cập.
                        </p>
                    </div>
                </div>
            </div>

            <div class="card shadow-sm">
                <div class="card-header-custom">
                    <h6 class="mb-0" style="font-size: 0.875rem; font-weight: 600; color: #1f2937;">
                        <i class="fas fa-link me-2" style="color: #8b5cf6;"></i>Liên kết hữu ích
                    </h6>
                </div>
                <div class="card-body p-3">
                    <div class="d-grid gap-2">
                        <a href="https://firebase.google.com/docs/database" target="_blank" class="btn btn-outline-primary btn-sm">
                            <i class="fas fa-external-link-alt me-2"></i>Firebase Documentation
                        </a>
                        <a href="https://console.firebase.google.com" target="_blank" class="btn btn-outline-secondary btn-sm">
                            <i class="fas fa-external-link-alt me-2"></i>Firebase Console
                        </a>
                    </div>
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
    
    .card-body {
        background: white;
    }
    
    .form-control-sm {
        font-size: 0.875rem;
        border-radius: 6px;
        border: 1px solid #d1d5db;
    }
    
    .form-control-sm:focus {
        border-color: var(--primary-color);
        box-shadow: 0 0 0 3px rgba(76, 175, 80, 0.1);
        outline: none;
    }
    
    code {
        background: #f3f4f6;
        padding: 0.2rem 0.4rem;
        border-radius: 4px;
        font-size: 0.85em;
    }
    
    .btn-sm {
        font-size: 0.875rem;
        padding: 0.4rem 0.75rem;
    }
</style>

<script>
    function copyToClipboard(text) {
        navigator.clipboard.writeText(text).then(function() {
            // Hiển thị thông báo
            const btn = event.target.closest('button');
            const originalHTML = btn.innerHTML;
            btn.innerHTML = '<i class="fas fa-check"></i>';
            btn.classList.remove('btn-outline-secondary');
            btn.classList.add('btn-success');
            
            setTimeout(function() {
                btn.innerHTML = originalHTML;
                btn.classList.remove('btn-success');
                btn.classList.add('btn-outline-secondary');
            }, 2000);
        }).catch(function(err) {
            alert('Không thể sao chép: ' + err);
        });
    }
</script>









