<!DOCTYPE html>
<html lang="vi">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>@ViewBag.Title - Ketcau Soft</title>
    @Styles.Render("~/Content/css")
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" />
    <link rel="preconnect" href="https://fonts.googleapis.com">
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
    <link href="https://fonts.googleapis.com/css2?family=Inter:wght@300;400;500;600;700&display=swap" rel="stylesheet">
    <style>
        :root {
            --primary-color: #4CAF50;
            --primary-dark: #388E3C;
            --primary-darker: #388E3C;
            --primary-light: #C8E6C9;
            --secondary-color: #64748b;
            --success-color: #4CAF50;
            --warning-color: #FFCA00;
            --danger-color: #B30208;
            --info-color: #0D6EFD;
            --info-light: #A8CBFE;
            --info-dark: #094BAC;
            --dark-color: #1e293b;
            --light-color: #f0f9ff;
        }

        body {
            font-family: 'Inter', 'Segoe UI', 'Arial', 'Helvetica', Tahoma, Geneva, Verdana, sans-serif;
            background: linear-gradient(135deg, #f8fafc 0%, #f1f5f9 100%);
            min-height: 100vh;
            margin: 0;
            padding: 0;
        }
        
        * {
            font-family: 'Inter', 'Segoe UI', 'Arial', 'Helvetica', Tahoma, Geneva, Verdana, sans-serif;
        }
        
        .sidebar {
            width: 70px;
            height: 100vh;
            background: linear-gradient(180deg, var(--primary-color) 0%, var(--primary-darker) 100%);
            box-shadow: 2px 0 12px rgba(76, 175, 80, 0.25);
            position: fixed;
            left: 0;
            top: 0;
            z-index: 1000;
            display: flex;
            flex-direction: column;
            overflow-y: auto;
            overflow-x: visible !important;
        }
        
        .sidebar-nav {
            flex: 1;
            padding: 1rem 0;
            overflow: visible;
            position: relative;
        }
        
        .sidebar-nav .nav-item {
            position: relative;
            overflow: visible;
        }
        
        .sidebar::-webkit-scrollbar {
            width: 6px;
        }
        
        .sidebar::-webkit-scrollbar-track {
            background: rgba(255, 255, 255, 0.1);
        }
        
        .sidebar::-webkit-scrollbar-thumb {
            background: rgba(255, 255, 255, 0.3);
            border-radius: 3px;
        }
        
        .sidebar::-webkit-scrollbar-thumb:hover {
            background: rgba(255, 255, 255, 0.5);
        }
        
        .sidebar-header {
            padding: 1rem 0.5rem;
            border-bottom: 1px solid rgba(255, 255, 255, 0.1);
            text-align: center;
        }
        
        .sidebar-brand {
            font-weight: 700;
            font-size: 1.1rem;
            color: white !important;
            text-shadow: 0 2px 4px rgba(0,0,0,0.2);
            display: flex;
            align-items: center;
            justify-content: center;
            gap: 0.5rem;
            text-decoration: none;
            flex-direction: column;
        }
        
        .sidebar-brand:hover {
            color: white !important;
        }
        
        .sidebar-brand span {
            display: none;
        }
        
        .sidebar-subtitle {
            display: none;
        }
        
        .company-logo {
            height: 35px;
            width: auto;
            object-fit: contain;
            filter: drop-shadow(0 2px 4px rgba(0,0,0,0.2));
        }
        
        .sidebar-nav {
            flex: 1;
            padding: 1rem 0;
            overflow: visible;
            position: relative;
        }
        
        .sidebar-nav .nav-link {
            color: rgba(255,255,255,0.9) !important;
            font-weight: 500;
            padding: 0.75rem !important;
            margin: 0.25rem 0.5rem;
            border-radius: 8px;
            transition: all 0.3s;
            display: flex;
            align-items: center;
            justify-content: center;
            gap: 0.75rem;
            position: relative;
        }
        
        .sidebar-nav .nav-link i {
            width: 24px;
            text-align: center;
            font-size: 1.2rem;
        }
        
        .sidebar-nav .nav-link span {
            display: none;
        }
        
        .sidebar-nav .nav-link:hover {
            color: white !important;
            background-color: rgba(255,255,255,0.15);
        }
        
        .sidebar-nav .nav-link.active {
            background-color: rgba(255,255,255,0.2);
            color: white !important;
            font-weight: 600;
        }
        
        /* Tooltip sẽ được xử lý bằng JavaScript */
        .sidebar-nav .nav-link {
            position: relative;
        }
        
        .sidebar-nav .nav-item {
            position: relative;
            overflow: visible !important;
        }
        
        /* Đảm bảo tooltip không bị che bởi các element khác */
        .sidebar-nav .nav-link:hover {
            z-index: 10003;
        }
        
        /* Side Panel cho Create Menu */
        .create-panel {
            position: fixed;
            left: 70px;
            top: 0;
            width: 280px;
            height: 100vh;
            background: linear-gradient(180deg, var(--primary-color) 0%, var(--primary-darker) 100%);
            box-shadow: 4px 0 20px rgba(0, 0, 0, 0.3);
            z-index: 9999;
            transform: translateX(-100%);
            transition: transform 0.3s ease;
            overflow-y: auto;
            overflow-x: hidden;
            visibility: hidden;
            opacity: 0;
        }
        
        .create-panel.show {
            transform: translateX(0);
            visibility: visible;
            opacity: 1;
        }
        
        .create-panel-header {
            padding: 1.5rem 1.5rem 1rem;
            border-bottom: 1px solid rgba(255, 255, 255, 0.1);
            display: flex;
            align-items: center;
            justify-content: space-between;
        }
        
        .create-panel-header h3 {
            color: white;
            font-size: 1.2rem;
            font-weight: 600;
            margin: 0;
        }
        
        .create-panel-close {
            background: none;
            border: none;
            color: white;
            font-size: 1.5rem;
            cursor: pointer;
            padding: 0;
            width: 30px;
            height: 30px;
            display: flex;
            align-items: center;
            justify-content: center;
            border-radius: 50%;
            transition: background-color 0.2s;
        }
        
        .create-panel-close:hover {
            background-color: rgba(255, 255, 255, 0.2);
        }
        
        .create-panel-body {
            padding: 1rem;
        }
        
        .create-panel-item {
            display: block;
            width: 100%;
            padding: 1rem 1.5rem;
            margin-bottom: 0.75rem;
            background-color: rgba(255, 255, 255, 0.1);
            border: 1px solid rgba(255, 255, 255, 0.2);
            border-radius: 8px;
            color: white;
            text-decoration: none;
            transition: all 0.3s;
            display: flex;
            align-items: center;
            gap: 1rem;
        }
        
        .create-panel-item:hover {
            background-color: rgba(255, 255, 255, 0.2);
            transform: translateX(5px);
            color: white;
            text-decoration: none;
        }
        
        .create-panel-item i {
            font-size: 1.5rem;
            width: 30px;
            text-align: center;
        }
        
        .create-panel-item-content {
            flex: 1;
        }
        
        .create-panel-item-title {
            font-weight: 600;
            font-size: 1rem;
            margin-bottom: 0.25rem;
        }
        
        .create-panel-item-desc {
            font-size: 0.85rem;
            opacity: 0.8;
        }
        
        /* Overlay khi panel mở */
        .create-panel-overlay {
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background-color: rgba(0, 0, 0, 0.3);
            z-index: 9998;
            opacity: 0;
            visibility: hidden;
            transition: opacity 0.3s ease, visibility 0.3s ease;
        }
        
        .create-panel-overlay.show {
            opacity: 1;
            visibility: visible;
        }
        
        .main-content {
            margin-left: 70px;
            min-height: 100vh;
            padding: 2rem;
            width: calc(100% - 70px);
        }
        
        @@media (max-width: 768px) {
            .sidebar {
                width: 70px;
            }
            
            .sidebar-header {
                padding: 1rem 0.5rem;
            }
            
            .sidebar-brand span:not(.company-logo) {
                display: none;
            }
            
            .sidebar-header div {
                display: none;
            }
            
            .sidebar-nav .nav-link span {
                display: none;
            }
            
            .main-content {
                margin-left: 70px;
                width: calc(100% - 70px);
                padding: 1rem;
            }
            
            .create-panel {
                width: 100%;
                left: 0;
            }
        }

        .card {
            border: none;
            border-radius: 16px;
            box-shadow: 0 4px 12px rgba(76, 175, 80, 0.1);
            transition: all 0.3s;
            margin-bottom: 1.5rem;
            background: white;
            overflow: hidden;
        }

        .card:hover {
            box-shadow: 0 8px 24px rgba(76, 175, 80, 0.2);
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
            box-shadow: 0 4px 12px rgba(76, 175, 80, 0.3);
        }

        .btn-primary:hover {
            transform: translateY(-2px);
            box-shadow: 0 6px 20px rgba(76, 175, 80, 0.4);
            background: linear-gradient(135deg, var(--primary-dark) 0%, var(--primary-darker) 100%);
        }

        .btn-success {
            background: linear-gradient(135deg, var(--primary-color) 0%, var(--primary-darker) 100%);
            border: none;
            border-radius: 10px;
            padding: 0.7rem 1.8rem;
            font-weight: 600;
            transition: all 0.3s;
            box-shadow: 0 4px 12px rgba(76, 175, 80, 0.3);
        }

        .btn-success:hover {
            transform: translateY(-2px);
            box-shadow: 0 6px 20px rgba(76, 175, 80, 0.4);
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
            box-shadow: 0 4px 12px rgba(76, 175, 80, 0.1);
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
            color: #1e293b !important;
            font-weight: 500 !important;
            text-decoration: none !important;
            font-size: 0.85rem !important;
        }
        
        .kanban-card h6 * {
            color: #1e293b !important;
            font-weight: 500 !important;
            text-decoration: none !important;
            background: transparent !important;
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
            box-shadow: 0 4px 12px rgba(76, 175, 80, 0.1);
            transition: all 0.3s;
            border-top: 4px solid var(--primary-color);
        }

        .stat-card:hover {
            transform: translateY(-8px);
            box-shadow: 0 12px 28px rgba(37, 99, 235, 0.2);
        }

        .stat-number {
            font-size: 3rem;
            font-weight: 700;
            margin: 0.5rem 0;
            background: linear-gradient(135deg, var(--primary-color) 0%, var(--primary-darker) 100%);
            -webkit-background-clip: text;
            -webkit-text-fill-color: transparent;
            background-clip: text;
            color: transparent;
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
            box-shadow: 0 0 0 4px rgba(76, 175, 80, 0.15);
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
            background-color: #4CAF50;
        }

        .table-scroll-container table thead th {
            background-color: #4CAF50;
            color: white;
            font-weight: 600;
            border-bottom: 2px solid #388E3C;
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
            margin-top: 0;
            box-shadow: 0 -4px 12px rgba(76, 175, 80, 0.2);
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
    <!-- Sidebar -->
    <aside class="sidebar">
        <div class="sidebar-header">
            <a class="sidebar-brand" href="@Url.Action("Index", "Dashboard")">
                <img src="@Url.Content("~/Content/Images/logo.png")" alt="KetcauSoft Logo" class="company-logo">
                <span>KetcauSoft</span>
            </a>
            <div class="sidebar-subtitle">Task Manager</div>
        </div>
        
        <nav class="sidebar-nav">
            <ul class="navbar-nav flex-column">
                <li class="nav-item">
                    @Code
                        Dim isDashboardActive = If(ViewContext.RouteData.Values("controller").ToString() = "Dashboard", "active", "")
                    End Code
                    <a class="nav-link @isDashboardActive" href="@Url.Action("Index", "Dashboard")" data-tooltip="Dashboard">
                        <i class="fas fa-home"></i>
                        <span>Dashboard</span>
                    </a>
                </li>
                <li class="nav-item">
                    @Code
                        Dim isTasksActive = If(ViewContext.RouteData.Values("controller").ToString() = "Tasks", "active", "")
                    End Code
                    <a class="nav-link @isTasksActive" href="@Url.Action("Index", "Tasks")" data-tooltip="Danh sách công việc">
                        <i class="fas fa-list"></i>
                        <span>Danh sách công việc</span>
                    </a>
                </li>
                <li class="nav-item">
                    @Code
                        Dim isQuestionsActive = If(ViewContext.RouteData.Values("controller").ToString() = "Questions", "active", "")
                    End Code
                    <a class="nav-link @isQuestionsActive" href="@Url.Action("Index", "Questions")" data-tooltip="Câu trả lời đã lưu">
                        <i class="fas fa-bookmark"></i>
                        <span>Câu trả lời đã lưu</span>
                    </a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" href="#" id="openCreatePanel" data-tooltip="Tạo mới">
                        <i class="fas fa-plus-circle"></i>
                        <span>Tạo mới</span>
                    </a>
                </li>
                <li class="nav-item">
                    @Code
                        Dim isSettingsActive = If(ViewContext.RouteData.Values("controller").ToString() = "Settings", "active", "")
                    End Code
                    <a class="nav-link @isSettingsActive" href="@Url.Action("Index", "Settings")" data-tooltip="Cài đặt">
                        <i class="fas fa-cog"></i>
                        <span>Cài đặt</span>
                    </a>
                </li>
            </ul>
        </nav>
    </aside>

    <!-- Create Panel -->
    <div class="create-panel-overlay" id="createPanelOverlay"></div>
    <div class="create-panel" id="createPanel">
        <div class="create-panel-header">
            <h3><i class="fas fa-plus-circle me-2"></i>Tạo mới</h3>
            <button class="create-panel-close" id="closeCreatePanel" aria-label="Đóng">
                <i class="fas fa-times"></i>
            </button>
        </div>
        <div class="create-panel-body">
            <a href="@Url.Action("Create", "Tasks")" class="create-panel-item">
                <i class="fas fa-tasks"></i>
                <div class="create-panel-item-content">
                    <div class="create-panel-item-title">Tạo công việc mới</div>
                    <div class="create-panel-item-desc">Thêm một công việc mới vào hệ thống</div>
                </div>
            </a>
            <a href="@Url.Action("Create", "Technicians")" class="create-panel-item">
                <i class="fas fa-user-plus"></i>
                <div class="create-panel-item-content">
                    <div class="create-panel-item-title">Thêm kỹ thuật viên</div>
                    <div class="create-panel-item-desc">Thêm thành viên kỹ thuật mới</div>
                </div>
            </a>
        </div>
    </div>

    <!-- Main Content -->
    <div class="main-content">
        @RenderBody()
    </div>

    @Html.Partial("_Footer")

    @Scripts.Render("~/bundles/jquery")
    @Scripts.Render("~/bundles/bootstrap")
    <script>
        // Khởi tạo Create Panel và Tooltip
        (function() {
            function initCreatePanel() {
                var openBtn = document.getElementById('openCreatePanel');
                var closeBtn = document.getElementById('closeCreatePanel');
                var panel = document.getElementById('createPanel');
                var overlay = document.getElementById('createPanelOverlay');
                
                if (!panel || !overlay) {
                    return;
                }
                
                // Đảm bảo panel ẩn mặc định
                panel.classList.remove('show');
                overlay.classList.remove('show');
                
                function openPanel() {
                    panel.classList.add('show');
                    overlay.classList.add('show');
                    document.body.style.overflow = 'hidden';
                }
                
                function closePanel() {
                    panel.classList.remove('show');
                    overlay.classList.remove('show');
                    document.body.style.overflow = '';
                }
                
                if (openBtn) {
                    openBtn.addEventListener('click', function(e) {
                        e.preventDefault();
                        e.stopPropagation();
                        openPanel();
                    });
                }
                
                if (closeBtn) {
                    closeBtn.addEventListener('click', function(e) {
                        e.preventDefault();
                        e.stopPropagation();
                        closePanel();
                    });
                }
                
                if (overlay) {
                    overlay.addEventListener('click', function(e) {
                        e.preventDefault();
                        closePanel();
                    });
                }
                
                // Đóng panel khi nhấn ESC
                document.addEventListener('keydown', function(e) {
                    if (e.key === 'Escape' && panel.classList.contains('show')) {
                        closePanel();
                    }
                });
            }
            
            function initTooltips() {
                var navLinks = document.querySelectorAll('.sidebar-nav .nav-link[data-tooltip]');
                var tooltipEl = null;
                
                navLinks.forEach(function(link) {
                    var tooltip = link.getAttribute('data-tooltip');
                    if (!tooltip) return;
                    
                    link.addEventListener('mouseenter', function(e) {
                        var rect = link.getBoundingClientRect();
                        
                        if (!tooltipEl) {
                            tooltipEl = document.createElement('div');
                            tooltipEl.className = 'custom-tooltip';
                            document.body.appendChild(tooltipEl);
                        }
                        
                        tooltipEl.textContent = tooltip;
                        tooltipEl.style.display = 'block';
                        
                        // Tính toán vị trí
                        var left = rect.right + 15;
                        var top = rect.top + (rect.height / 2);
                        
                        tooltipEl.style.left = left + 'px';
                        tooltipEl.style.top = top + 'px';
                        tooltipEl.style.transform = 'translateY(-50%)';
                        tooltipEl.style.opacity = '1';
                        tooltipEl.style.visibility = 'visible';
                    });
                    
                    link.addEventListener('mouseleave', function() {
                        if (tooltipEl) {
                            tooltipEl.style.opacity = '0';
                            tooltipEl.style.visibility = 'hidden';
                        }
                    });
                });
            }
            
            // Khởi tạo khi DOM ready
            if (document.readyState === 'loading') {
                document.addEventListener('DOMContentLoaded', function() {
                    initCreatePanel();
                    initTooltips();
                });
            } else {
                initCreatePanel();
                initTooltips();
            }
        })();
    </script>
    
    <style>
        .custom-tooltip {
            position: fixed;
            padding: 0.5rem 0.75rem;
            background-color: rgba(0, 0, 0, 0.95);
            color: white;
            border-radius: 6px;
            font-size: 0.875rem;
            white-space: nowrap;
            opacity: 0;
            visibility: hidden;
            pointer-events: none;
            transition: opacity 0.15s ease, visibility 0.15s ease;
            z-index: 10001;
            box-shadow: 0 4px 12px rgba(0, 0, 0, 0.4);
            font-weight: 500;
        }
        
        .custom-tooltip::before {
            content: '';
            position: absolute;
            left: -6px;
            top: 50%;
            transform: translateY(-50%);
            border: 6px solid transparent;
            border-right-color: rgba(0, 0, 0, 0.95);
        }
    </style>
    @RenderSection("scripts", required:=False)
</body>
</html>