<!DOCTYPE html>
<html lang="vi">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>@ViewBag.Title - Ketcau Soft</title>
    @Styles.Render("~/Content/css")
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" />
    <style>
        :root {
            --primary-color: #22c55e;
            --primary-dark: #16a34a;
            --primary-darker: #15803d;
            --secondary-color: #64748b;
            --success-color: #22c55e;
            --warning-color: #f59e0b;
            --danger-color: #ef4444;
            --info-color: #3b82f6;
            --dark-color: #1e293b;
            --light-color: #f0fdf4;
        }

        body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background: linear-gradient(135deg, #f0fdf4 0%, #dcfce7 100%);
            min-height: 100vh;
        }

        .navbar {
            background: linear-gradient(135deg, var(--primary-color) 0%, var(--primary-darker) 100%) !important;
            box-shadow: 0 4px 12px rgba(34, 197, 94, 0.3);
            padding: 1rem 0;
            border-bottom: 3px solid var(--primary-darker);
        }

        .navbar-brand {
            font-weight: 700;
            font-size: 1.6rem;
            color: white !important;
            text-shadow: 0 2px 4px rgba(0,0,0,0.2);
            display: flex;
            align-items: center;
            gap: 0.5rem;
        }

        .navbar-brand i {
            font-size: 1.8rem;
        }

        .company-logo {
            height: 40px;
            width: auto;
            margin-right: 10px;
            object-fit: contain;
            filter: drop-shadow(0 2px 4px rgba(0,0,0,0.2));
        }

        .navbar-nav .nav-link {
            color: rgba(255,255,255,0.95) !important;
            font-weight: 500;
            margin: 0 0.5rem;
            padding: 0.5rem 1rem !important;
            border-radius: 8px;
            transition: all 0.3s;
        }

        .navbar-nav .nav-link:hover {
            color: white !important;
            background-color: rgba(255,255,255,0.15);
            transform: translateY(-2px);
        }

        .main-content {
            min-height: calc(100vh - 200px);
            padding: 2rem 0;
        }

        .card {
            border: none;
            border-radius: 16px;
            box-shadow: 0 4px 12px rgba(34, 197, 94, 0.1);
            transition: all 0.3s;
            margin-bottom: 1.5rem;
            background: white;
            overflow: hidden;
        }

        .card:hover {
            box-shadow: 0 8px 24px rgba(34, 197, 94, 0.2);
            transform: translateY(-4px);
        }

        .card-header {
            background: linear-gradient(135deg, var(--primary-color) 0%, var(--primary-darker) 100%);
            color: white;
            border-radius: 0 !important;
            padding: 1.25rem 1.5rem;
            font-weight: 600;
            font-size: 1.1rem;
        }

        .btn-primary {
            background: linear-gradient(135deg, var(--primary-color) 0%, var(--primary-darker) 100%);
            border: none;
            border-radius: 10px;
            padding: 0.7rem 1.8rem;
            font-weight: 600;
            transition: all 0.3s;
            box-shadow: 0 4px 12px rgba(34, 197, 94, 0.3);
        }

        .btn-primary:hover {
            transform: translateY(-2px);
            box-shadow: 0 6px 20px rgba(34, 197, 94, 0.4);
            background: linear-gradient(135deg, var(--primary-dark) 0%, var(--primary-darker) 100%);
        }

        .btn-success {
            background: linear-gradient(135deg, var(--primary-color) 0%, var(--primary-darker) 100%);
            border: none;
            border-radius: 10px;
            padding: 0.7rem 1.8rem;
            font-weight: 600;
            transition: all 0.3s;
            box-shadow: 0 4px 12px rgba(34, 197, 94, 0.3);
        }

        .btn-success:hover {
            transform: translateY(-2px);
            box-shadow: 0 6px 20px rgba(34, 197, 94, 0.4);
        }

        .btn-info {
            background: linear-gradient(135deg, #3b82f6 0%, #2563eb 100%);
            border: none;
            border-radius: 10px;
            padding: 0.5rem 1.2rem;
            font-weight: 500;
            transition: all 0.3s;
        }

        .btn-warning {
            background: linear-gradient(135deg, #f59e0b 0%, #d97706 100%);
            border: none;
            border-radius: 10px;
            padding: 0.5rem 1.2rem;
            font-weight: 500;
            transition: all 0.3s;
        }

        .btn-danger {
            background: linear-gradient(135deg, #ef4444 0%, #dc2626 100%);
            border: none;
            border-radius: 10px;
            padding: 0.5rem 1.2rem;
            font-weight: 500;
            transition: all 0.3s;
        }

        .status-badge {
            padding: 0.5rem 1rem;
            border-radius: 25px;
            font-size: 0.85rem;
            font-weight: 600;
            display: inline-block;
            box-shadow: 0 2px 6px rgba(0,0,0,0.1);
        }

        .status-pending {
            background: linear-gradient(135deg, #fef3c7 0%, #fde68a 100%);
            color: #92400e;
        }

        .status-inprogress {
            background: linear-gradient(135deg, #dbeafe 0%, #bfdbfe 100%);
            color: #1e40af;
        }

        .status-waiting {
            background: linear-gradient(135deg, #fed7aa 0%, #fdba74 100%);
            color: #9a3412;
        }

        .status-completed {
            background: linear-gradient(135deg, #d1fae5 0%, #a7f3d0 100%);
            color: #065f46;
            border: 2px solid var(--primary-color);
        }

        .status-paused {
            background: linear-gradient(135deg, #fee2e2 0%, #fecaca 100%);
            color: #991b1b;
        }

        .kanban-column {
            height: 500px;
            background: white;
            border-radius: 16px;
            padding: 0;
            margin-bottom: 1.5rem;
            box-shadow: 0 4px 12px rgba(34, 197, 94, 0.1);
            border-top: 4px solid var(--primary-color);
            overflow: hidden;
            word-wrap: break-word;
            overflow-wrap: break-word;
            width: 100%;
            box-sizing: border-box;
            display: flex;
            flex-direction: column;
        }

        .kanban-column h5 {
            padding: 1.5rem 1.5rem 1rem 1.5rem;
            margin: 0;
            border-bottom: 1px solid #e5e7eb;
            flex-shrink: 0;
        }

        .kanban-column-content {
            flex: 1;
            overflow-y: auto;
            overflow-x: hidden;
            padding: 1rem 1.5rem 1.5rem 1.5rem;
        }

        .kanban-column-content::-webkit-scrollbar {
            width: 6px;
        }

        .kanban-column-content::-webkit-scrollbar-track {
            background: #f1f1f1;
            border-radius: 10px;
        }

        .kanban-column-content::-webkit-scrollbar-thumb {
            background: var(--primary-color);
            border-radius: 10px;
        }

        .kanban-column-content::-webkit-scrollbar-thumb:hover {
            background: var(--primary-darker);
        }

        .kanban-card {
            background: white;
            border-left: 5px solid var(--primary-color);
            border-radius: 12px;
            padding: 1.2rem;
            margin-bottom: 1rem;
            cursor: pointer;
            transition: all 0.3s;
            box-shadow: 0 2px 8px rgba(0,0,0,0.08);
            word-wrap: break-word;
            overflow-wrap: break-word;
            max-width: 100%;
            overflow: hidden;
            width: 100%;
            box-sizing: border-box;
        }

        .kanban-card * {
            word-wrap: break-word;
            overflow-wrap: break-word;
            max-width: 100%;
        }

        .kanban-card h6 {
            word-wrap: break-word;
            overflow-wrap: break-word;
            max-width: 100%;
            line-height: 1.4;
            margin-bottom: 0.5rem;
            overflow: hidden;
        }

        .kanban-card p {
            word-wrap: break-word;
            overflow-wrap: break-word;
            max-width: 100%;
            line-height: 1.4;
            overflow: hidden;
        }

        .kanban-card small {
            word-wrap: break-word;
            overflow-wrap: break-word;
            max-width: 100%;
            display: inline-block;
            white-space: nowrap;
            overflow: hidden;
            text-overflow: ellipsis;
        }

        .kanban-card .d-flex {
            flex-wrap: wrap;
            gap: 0.5rem;
        }

        .kanban-card .d-flex small {
            flex: 0 1 auto;
            min-width: 0;
        }

        .kanban-card .status-dropdown {
            font-size: 0.75rem;
            padding: 0.25rem 0.5rem;
            border: 1px solid #ddd;
            border-radius: 6px;
            background: white;
            cursor: pointer;
            z-index: 10;
            position: relative;
        }

        .kanban-card .status-dropdown:hover {
            border-color: var(--primary-color);
        }

        .kanban-card .status-dropdown:focus {
            outline: none;
            border-color: var(--primary-color);
            box-shadow: 0 0 0 2px rgba(34, 197, 94, 0.2);
        }

        .kanban-card:hover {
            box-shadow: 0 6px 20px rgba(34, 197, 94, 0.2);
            transform: translateX(8px);
            border-left-color: var(--primary-darker);
        }

        .stat-card {
            background: white;
            border-radius: 16px;
            padding: 2rem;
            text-align: center;
            box-shadow: 0 4px 12px rgba(34, 197, 94, 0.1);
            transition: all 0.3s;
            border-top: 4px solid var(--primary-color);
        }

        .stat-card:hover {
            transform: translateY(-8px);
            box-shadow: 0 12px 28px rgba(34, 197, 94, 0.2);
        }

        .stat-number {
            font-size: 3rem;
            font-weight: 700;
            margin: 0.5rem 0;
            background: linear-gradient(135deg, var(--primary-color) 0%, var(--primary-darker) 100%);
            -webkit-background-clip: text;
            -webkit-text-fill-color: transparent;
            background-clip: text;
        }

        .stat-label {
            color: var(--secondary-color);
            font-size: 0.9rem;
            text-transform: uppercase;
            letter-spacing: 1px;
            font-weight: 600;
        }

        footer {
            background: linear-gradient(135deg, var(--dark-color) 0%, #0f172a 100%);
            color: white;
            padding: 2.5rem 0;
            margin-top: 3rem;
            border-top: 3px solid var(--primary-color);
        }

        .form-control, .form-select {
            border-radius: 10px;
            border: 2px solid #e2e8f0;
            padding: 0.75rem 1rem;
            transition: all 0.3s;
        }

        .form-control:focus, .form-select:focus {
            border-color: var(--primary-color);
            box-shadow: 0 0 0 4px rgba(34, 197, 94, 0.15);
            outline: none;
        }

        .table {
            border-radius: 12px;
            overflow: hidden;
            box-shadow: 0 4px 12px rgba(0,0,0,0.05);
        }

        .table thead {
            background: linear-gradient(135deg, var(--primary-color) 0%, var(--primary-darker) 100%);
            color: white;
        }

        .table thead th {
            border: none;
            padding: 1rem;
            font-weight: 600;
            text-transform: uppercase;
            font-size: 0.85rem;
            letter-spacing: 0.5px;
        }

        .table tbody tr {
            transition: all 0.2s;
            border-bottom: 1px solid #f1f5f9;
        }

        .table tbody tr:hover {
            background-color: #f0fdf4;
            transform: scale(1.01);
        }

        .table tbody td {
            padding: 1rem;
            vertical-align: middle;
        }

        h1 {
            color: var(--primary-darker);
            font-weight: 700;
            margin-bottom: 1.5rem;
        }

        h1 i {
            color: var(--primary-color);
        }

        .ketcau-brand {
            font-size: 0.9rem;
            opacity: 0.9;
            font-weight: 500;
        }

        /* Table scroll container */
        .table-scroll-container {
            max-height: 600px;
            overflow-y: auto;
            overflow-x: auto;
            position: relative;
        }

        /* Sticky header khi cuộn */
        .table-scroll-container table thead {
            position: sticky;
            top: 0;
            z-index: 10;
            background-color: #22c55e;
        }

        .table-scroll-container table thead th {
            background-color: #22c55e;
            color: white;
            font-weight: 600;
            border-bottom: 2px solid #16a34a;
            padding: 1rem;
            white-space: nowrap;
        }

        /* Custom scrollbar cho table */
        .table-scroll-container::-webkit-scrollbar {
            width: 8px;
            height: 8px;
        }

        .table-scroll-container::-webkit-scrollbar-track {
            background: #f1f5f9;
            border-radius: 10px;
        }

        .table-scroll-container::-webkit-scrollbar-thumb {
            background: linear-gradient(180deg, var(--primary-color), var(--primary-dark));
            border-radius: 10px;
        }

        .table-scroll-container::-webkit-scrollbar-thumb:hover {
            background: linear-gradient(180deg, var(--primary-dark), var(--primary-darker));
        }

        /* Firefox scrollbar */
        .table-scroll-container {
            scrollbar-width: thin;
            scrollbar-color: var(--primary-color) #f1f5f9;
        }

        /* Footer styles */
        footer {
            background: linear-gradient(135deg, var(--primary-color) 0%, var(--primary-darker) 100%);
            color: white;
            padding: 2rem 0;
            margin-top: 3rem;
            box-shadow: 0 -4px 12px rgba(34, 197, 94, 0.2);
            border-top: 3px solid var(--primary-darker);
        }

        footer img {
            filter: drop-shadow(0 2px 4px rgba(0,0,0,0.2));
        }

        footer p {
            margin: 0;
        }
    </style>
    @RenderSection("styles", required:=False)
</head>
<body>
    <nav class="navbar navbar-expand-lg navbar-dark">
        <div class="container-fluid">
            <a class="navbar-brand" href="@Url.Action("Index", "Dashboard")">
                <img src="@Url.Content("~/Content/Images/logo.png")" alt="Ketcau Soft Logo" class="company-logo">
                <span>Ketcau Soft</span>
                <span class="ketcau-brand" style="font-size: 0.7rem; opacity: 0.8; margin-left: 0.5rem;">Task Manager</span>
            </a>
            <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
                <span class="navbar-toggler-icon"></span>
            </button>
            <div class="collapse navbar-collapse" id="navbarNav">
                <ul class="navbar-nav ms-auto">
                    <li class="nav-item">
                        <a class="nav-link" href="@Url.Action("Index", "Dashboard")">
                            <i class="fas fa-home me-1"></i>Dashboard
                        </a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="@Url.Action("Index", "Tasks")">
                            <i class="fas fa-list me-1"></i>Danh sách công việc
                        </a>
                    </li>
                    <li class="nav-item dropdown">
                        <a class="nav-link dropdown-toggle" href="#" id="createDropdown" role="button" data-bs-toggle="dropdown" data-toggle="dropdown" aria-expanded="false" aria-haspopup="true">
                            <i class="fas fa-plus-circle me-1"></i>Tạo mới
                        </a>
                        <ul class="dropdown-menu dropdown-menu-end" aria-labelledby="createDropdown">
                            <li>
                                <a class="dropdown-item" href="@Url.Action("Create", "Tasks")">
                                    <i class="fas fa-tasks me-2"></i>Tạo công việc mới
                                </a>
                            </li>
                            <li><hr class="dropdown-divider"></li>
                            <li>
                                <a class="dropdown-item" href="@Url.Action("Create", "Technicians")">
                                    <i class="fas fa-user-plus me-2"></i>Thêm kỹ thuật viên
                                </a>
                            </li>
                        </ul>
                    </li>
                </ul>
            </div>
        </div>
    </nav>

    <div class="container-fluid main-content">
        @RenderBody()
    </div>

    <footer>
        <div class="container-fluid text-center">
            <p class="mb-2" style="font-size: 1.1rem; font-weight: 600; display: flex; align-items: center; justify-content: center; gap: 0.5rem;">
                <img src="@Url.Content("~/Content/Images/logo.png")" alt="Ketcau Soft Logo" style="height: 30px; width: auto; object-fit: contain;">
                Ketcau Soft - Hệ thống quản lý công việc kỹ thuật
            </p>
            <p class="mb-0" style="opacity: 0.8;">&copy; @DateTime.Now.Year - Bản quyền thuộc về Ketcau Soft</p>
        </div>
    </footer>

    @Scripts.Render("~/bundles/jquery")
    @Scripts.Render("~/bundles/bootstrap")
    <script>
        // Khởi tạo Bootstrap dropdown
        (function() {
            function initDropdowns() {
                var dropdowns = document.querySelectorAll('.dropdown-toggle');
                
                // Bootstrap 5
                if (typeof bootstrap !== 'undefined' && bootstrap.Dropdown) {
                    dropdowns.forEach(function(dropdownEl) {
                        try {
                            if (!bootstrap.Dropdown.getInstance(dropdownEl)) {
                                new bootstrap.Dropdown(dropdownEl);
                            }
                        } catch (e) {
                            console.log('Bootstrap 5 dropdown error:', e);
                        }
                    });
                }
                // Bootstrap 4 fallback (jQuery)
                else if (typeof jQuery !== 'undefined' && jQuery.fn.dropdown) {
                    jQuery('.dropdown-toggle').dropdown();
                }
                // Fallback: Toggle bằng JavaScript thuần
                else {
                    dropdowns.forEach(function(dropdownEl) {
                        dropdownEl.addEventListener('click', function(e) {
                            e.preventDefault();
                            var menu = this.nextElementSibling;
                            if (menu && menu.classList.contains('dropdown-menu')) {
                                menu.classList.toggle('show');
                                this.setAttribute('aria-expanded', menu.classList.contains('show'));
                            }
                        });
                    });
                    
                    // Đóng dropdown khi click outside
                    document.addEventListener('click', function(e) {
                        if (!e.target.closest('.dropdown')) {
                            document.querySelectorAll('.dropdown-menu.show').forEach(function(menu) {
                                menu.classList.remove('show');
                            });
                            document.querySelectorAll('.dropdown-toggle[aria-expanded="true"]').forEach(function(toggle) {
                                toggle.setAttribute('aria-expanded', 'false');
                            });
                        }
                    });
                }
            }
            
            // Khởi tạo khi DOM ready
            if (document.readyState === 'loading') {
                document.addEventListener('DOMContentLoaded', function() {
                    setTimeout(initDropdowns, 100);
                });
            } else {
                setTimeout(initDropdowns, 100);
            }
        })();
    </script>
    @RenderSection("scripts", required:=False)
</body>
</html>