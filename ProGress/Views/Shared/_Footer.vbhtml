<footer class="main-footer">
    <div class="footer-content">
        <div class="footer-logo-section">
            <img src="@Url.Content("~/Content/Images/logo.png")" alt="KetcauSoft Logo" class="footer-logo" />
        </div>
        <div class="footer-text-section">
            <p class="footer-title">KetcauSoft - Hệ thống quản lý công việc kỹ thuật</p>
            <p class="footer-copyright">&copy; @DateTime.Now.Year - Bản quyền thuộc về KetcauSoft</p>
        </div>
    </div>
</footer>

<style>
    .main-footer {
        margin-left: 70px;
        background: linear-gradient(135deg, var(--dark-color) 0%, #0f172a 100%);
        color: white;
        padding: 1.5rem 0;
        margin-top: 0;
        border-top: 3px solid var(--primary-color);
        position: relative;
        z-index: 100;
    }
    
    .footer-content {
        display: flex;
        align-items: center;
        justify-content: flex-start;
        gap: 0;
        max-width: 100%;
        margin: 0;
        padding: 0 1.5rem;
    }
    
    .footer-logo-section {
        flex-shrink: 0;
        margin-right: 0.3rem;
    }
    
    .footer-logo {
        height: 28px;
        width: auto;
        object-fit: contain;
        display: block;
    }
    
    .footer-text-section {
        flex-grow: 1;
        text-align: left;
    }
    
    .footer-title {
        font-size: 0.9rem;
        font-weight: 600;
        margin: 0 0 0.25rem 0;
    }
    
    .footer-copyright {
        font-size: 0.85rem;
        opacity: 0.8;
        margin: 0;
    }
    
    @@media (max-width: 768px) {
        .main-footer {
            margin-left: 70px;
        }
        
        .footer-content {
            flex-direction: column;
            gap: 0.5rem;
        }
        
        .footer-logo {
            height: 25px;
        }
        
        .footer-title {
            font-size: 0.8rem;
        }
        
        .footer-copyright {
            font-size: 0.75rem;
        }
    }
</style>

