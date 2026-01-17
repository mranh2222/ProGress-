@ModelType IEnumerable(Of Task)
@Code
    ViewData("Title") = "Dashboard - Ketcau Soft"
    Dim tasksByStatus = TryCast(ViewBag.TasksByStatus, Dictionary(Of TaskStatus, List(Of Task)))
    
    ' Tính toán count cho mỗi status
    Dim pendingCount As Integer = 0
    Dim inProgressCount As Integer = 0
    Dim waitingCount As Integer = 0
    Dim completedCount As Integer = 0
    Dim pausedCount As Integer = 0
    
    If tasksByStatus IsNot Nothing Then
        If tasksByStatus.ContainsKey(TaskStatus.Pending) Then pendingCount = tasksByStatus(TaskStatus.Pending).Count
        If tasksByStatus.ContainsKey(TaskStatus.InProgress) Then inProgressCount = tasksByStatus(TaskStatus.InProgress).Count
        If tasksByStatus.ContainsKey(TaskStatus.Waiting) Then waitingCount = tasksByStatus(TaskStatus.Waiting).Count
        If tasksByStatus.ContainsKey(TaskStatus.Completed) Then completedCount = tasksByStatus(TaskStatus.Completed).Count
        If tasksByStatus.ContainsKey(TaskStatus.Paused) Then pausedCount = tasksByStatus(TaskStatus.Paused).Count
    End If
    
    Dim totalTasks = ViewBag.TotalTasks
    
    ' Thống kê theo kỹ thuật viên (dùng chung cho cả bảng và biểu đồ)
    Dim technicianStats = TryCast(ViewBag.TechnicianStats, List(Of Object))
    Dim techLabels As New List(Of String)
    Dim techData As New List(Of Integer)
    
    If technicianStats IsNot Nothing AndAlso technicianStats.Count > 0 Then
        For Each techStat In technicianStats
            ' Decode HTML entities để hiển thị đúng tiếng Việt
            Dim decodedName = System.Web.HttpUtility.HtmlDecode(techStat.Name)
            techLabels.Add(decodedName)
            techData.Add(techStat.Total)
        Next
    End If
End Code

@section styles
    <style>
        .dashboard-header {
            background: linear-gradient(135deg, #4CAF50 0%, #388E3C 100%);
            color: white;
            padding: 1rem 1.5rem;
            border-radius: 8px;
            margin-bottom: 1rem;
            box-shadow: 0 2px 8px rgba(76, 175, 80, 0.2);
        }

        .stat-card {
            background: white;
            border-radius: 8px;
            padding: 0.75rem;
            box-shadow: 0 1px 5px rgba(0, 0, 0, 0.08);
            transition: all 0.2s ease;
            border-left: 3px solid;
            height: 100%;
            display: flex;
            flex-direction: column;
        }

        .stat-card:hover {
            transform: translateY(-2px);
            box-shadow: 0 3px 10px rgba(0, 0, 0, 0.12);
        }
        
        .stat-card-header {
            display: flex;
            align-items: center;
            gap: 0.5rem;
            margin-bottom: 0.5rem;
        }

        .stat-card.total {
            border-left-color: #4CAF50;
            background: linear-gradient(135deg, #C8E6C9 0%, #ffffff 100%);
        }

        .stat-card.pending {
            border-left-color: #FFCA00;
            background: linear-gradient(135deg, #fff9e6 0%, #ffffff 100%);
        }

        .stat-card.inprogress {
            border-left-color: #3b82f6;
            background: linear-gradient(135deg, #dbeafe 0%, #ffffff 100%);
        }

        .stat-card.waiting {
            border-left-color: #FF8C00;
            background: linear-gradient(135deg, #ffe6cc 0%, #ffffff 100%);
        }

        .stat-card.completed {
            border-left-color: #4CAF50;
            background: linear-gradient(135deg, #C8E6C9 0%, #ffffff 100%);
        }

        .stat-card.paused {
            border-left-color: #B30208;
            background: linear-gradient(135deg, #ffe6e6 0%, #ffffff 100%);
        }

        .stat-icon {
            width: 32px;
            height: 32px;
            border-radius: 6px;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 1rem;
            flex-shrink: 0;
        }

        .stat-icon.total { background: linear-gradient(135deg, #22c55e, #16a34a); color: white; }
        .stat-icon.pending { background: linear-gradient(135deg, #fbbf24, #f59e0b); color: white; }
        .stat-icon.inprogress { background: linear-gradient(135deg, #3b82f6, #2563eb); color: white; }
        .stat-icon.waiting { background: linear-gradient(135deg, #f59e0b, #d97706); color: white; }
        .stat-icon.completed { background: linear-gradient(135deg, #10b981, #059669); color: white; }
        .stat-icon.paused { background: linear-gradient(135deg, #ef4444, #dc2626); color: white; }

        .stat-label {
            font-size: 0.75rem;
            color: #64748b;
            font-weight: 500;
            text-transform: uppercase;
            letter-spacing: 0.3px;
            margin: 0;
            flex-grow: 1;
        }

        .stat-number {
            font-size: 1.5rem;
            font-weight: 700;
            margin: 0;
            line-height: 1;
            margin-top: auto;
        }

        .stat-number.total { color: #22c55e; }
        .stat-number.pending { color: #fbbf24; }
        .stat-number.inprogress { color: #3b82f6; }
        .stat-number.waiting { color: #f59e0b; }
        .stat-number.completed { color: #10b981; }
        .stat-number.paused { color: #ef4444; }

        .chart-card {
            background: white;
            border-radius: 8px;
            padding: 1rem;
            box-shadow: 0 1px 5px rgba(0, 0, 0, 0.08);
            margin-bottom: 1rem;
            height: 100%;
        }

        .chart-card h5 {
            color: var(--primary-darker);
            font-weight: 600;
            font-size: 0.95rem;
            margin-bottom: 1rem;
            padding-bottom: 0.75rem;
            border-bottom: 1px solid #f1f5f9;
        }

        .technician-card {
            background: white;
            border-radius: 8px;
            padding: 1rem;
            box-shadow: 0 1px 5px rgba(0, 0, 0, 0.08);
            transition: all 0.2s ease;
            border: 1px solid #e5e7eb;
            height: 100%;
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
        }
        
        .technician-card h6 {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif !important;
        }

        .technician-card:hover {
            transform: translateY(-2px);
            box-shadow: 0 3px 10px rgba(34, 197, 94, 0.15);
            border-color: var(--primary-color);
        }

        .technician-avatar {
            width: 40px;
            height: 40px;
            border-radius: 50%;
            background: linear-gradient(135deg, var(--primary-color), var(--primary-dark));
            color: white;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 1.2rem;
        }

        .technician-stats {
            margin-top: 1rem;
        }

        .stat-item {
            display: flex;
            justify-content: space-between;
            align-items: center;
            padding: 0.5rem 0;
            border-bottom: 1px solid #f1f5f9;
        }

        .stat-item:last-child {
            border-bottom: none;
        }

        .stat-item .stat-label {
            font-size: 0.9rem;
            color: #64748b;
        }

        .stat-item .stat-value {
            font-size: 1.5rem;
            font-weight: 700;
        }

        .task-item-card {
            background: white;
            border-radius: 8px;
            padding: 0.875rem;
            margin-bottom: 0.75rem;
            box-shadow: 0 1px 3px rgba(0, 0, 0, 0.08);
            border-left: 3px solid;
            border: 1px solid #e5e7eb;
            border-left-width: 3px;
            transition: all 0.2s ease;
        }

        .task-item-card:hover {
            box-shadow: 0 2px 8px rgba(0, 0, 0, 0.12);
            transform: translateY(-1px);
            border-color: #cbd5e1;
        }

        .task-item-card.pending { border-left-color: #f59e0b; }
        .task-item-card.inprogress { border-left-color: #0d9488; }
        .task-item-card.waiting { border-left-color: #ea580c; }
        .task-item-card.completed { border-left-color: #22c55e; }
        .task-item-card.paused { border-left-color: #be123c; }
        
        .task-item-card h6 {
            font-size: 0.875rem;
            font-weight: 600;
            color: #1f2937;
            margin-bottom: 0.5rem;
            line-height: 1.4;
            display: -webkit-box;
            -webkit-line-clamp: 2;
            -webkit-box-orient: vertical;
            overflow: hidden;
        }
        
        .task-item-card small {
            font-size: 0.75rem;
            color: #6b7280;
            line-height: 1.5;
        }
        
        .task-item-card .badge {
            font-size: 0.7rem;
            padding: 0.25rem 0.5rem;
            font-weight: 500;
        }
        
        .task-item-card .btn-group {
            gap: 0.25rem;
        }
        
        .task-item-card .btn-sm {
            padding: 0.25rem 0.5rem;
            font-size: 0.75rem;
        }
        
        .task-item-card .form-select-sm {
            font-size: 0.75rem;
            padding: 0.25rem 1.75rem 0.25rem 0.5rem;
        }

        .kanban-section {
            margin-top: 1rem;
        }

        .kanban-section h3 {
            color: var(--primary-darker);
            font-weight: 600;
            font-size: 1.2rem;
            margin-bottom: 1rem;
            padding-bottom: 0.75rem;
            border-bottom: 2px solid var(--primary-color);
        }
        
        /* Đảm bảo 5 cột Kanban rộng bằng phần statistics cards */
        @@media (min-width: 992px) {
            .kanban-section .row > div[class*="col-lg-2"] {
                flex: 0 0 20% !important;
                max-width: 20% !important;
            }
        }
        
        /* Override kanban styles để làm gọn hơn */
        .kanban-column {
            height: 450px !important;
            border-radius: 8px !important;
            box-shadow: 0 1px 5px rgba(0, 0, 0, 0.08) !important;
            border-top: 3px solid var(--primary-color) !important;
        }
        
        .kanban-column h5 {
            padding: 0.75rem 1rem !important;
            font-size: 0.9rem !important;
            font-weight: 600 !important;
        }
        
        .kanban-column-content {
            padding: 0.75rem 1rem !important;
        }
        
        .kanban-card {
            border-radius: 6px !important;
            padding: 0.75rem !important;
            margin-bottom: 0.75rem !important;
            box-shadow: 0 1px 4px rgba(0,0,0,0.06) !important;
            border-left: 3px solid var(--primary-color) !important;
            position: relative !important;
        }
        
        .kanban-card h6 {
            font-size: 0.85rem !important;
            margin-bottom: 0.4rem !important;
            padding-right: 130px !important;
        }
        
        .kanban-card p {
            font-size: 0.8rem !important;
            margin-bottom: 0.4rem !important;
        }
        
        .kanban-card small {
            font-size: 0.75rem !important;
        }
        
        .kanban-card .status-dropdown {
            font-size: 0.7rem !important;
            padding: 0.2rem 0.4rem !important;
            position: absolute !important;
            top: 0.75rem !important;
            right: 0.75rem !important;
            z-index: 10 !important;
            min-width: 120px !important;
        }
        
        .kanban-card .d-flex.justify-content-between.align-items-start {
            position: relative !important;
        }
        
        .kanban-card:hover {
            transform: translateX(3px) !important;
            box-shadow: 0 2px 8px rgba(34, 197, 94, 0.15) !important;
        }
    </style>
End Section

<div class="container-fluid">
    <!-- Dashboard Header -->
    <div class="dashboard-header">
        <div class="row align-items-center">
            <div class="col-md-12">
                <h1 class="mb-1" style="font-size: 1.5rem; font-weight: 600; color: white;">
                    <i class="fas fa-chart-line me-2"></i>Dashboard
                </h1>
                <p class="mb-0" style="opacity: 0.9; font-size: 0.875rem;">
                    Tổng quan hệ thống quản lý công việc
                </p>
            </div>
        </div>
    </div>

    <!-- Thống kê tổng quan -->
    <div class="row mb-3">
        <div class="col-lg-2 col-md-4 col-sm-6 mb-3">
            <div class="stat-card total">
                <div class="stat-card-header">
                    <div class="stat-icon total">
                        <i class="fas fa-tasks"></i>
                    </div>
                    <div class="stat-label">Tổng công việc</div>
                </div>
                <div class="stat-number total">@totalTasks</div>
            </div>
        </div>
        <div class="col-lg-2 col-md-4 col-sm-6 mb-3">
            <div class="stat-card pending">
                <div class="stat-card-header">
                    <div class="stat-icon pending">
                        <i class="fas fa-clock"></i>
                    </div>
                    <div class="stat-label">Chưa xử lý</div>
                </div>
                <div class="stat-number pending">@ViewBag.PendingTasks</div>
            </div>
        </div>
        <div class="col-lg-2 col-md-4 col-sm-6 mb-3">
            <div class="stat-card inprogress">
                <div class="stat-card-header">
                    <div class="stat-icon inprogress">
                        <i class="fas fa-spinner"></i>
                    </div>
                    <div class="stat-label">Đang xử lý</div>
                </div>
                <div class="stat-number inprogress">@ViewBag.InProgressTasks</div>
            </div>
        </div>
        <div class="col-lg-2 col-md-4 col-sm-6 mb-3">
            <div class="stat-card waiting">
                <div class="stat-card-header">
                    <div class="stat-icon waiting">
                        <i class="fas fa-hourglass-half"></i>
                    </div>
                    <div class="stat-label">Chờ phản hồi</div>
                </div>
                <div class="stat-number waiting">@ViewBag.WaitingTasks</div>
            </div>
        </div>
        <div class="col-lg-2 col-md-4 col-sm-6 mb-3">
            <div class="stat-card completed">
                <div class="stat-card-header">
                    <div class="stat-icon completed">
                        <i class="fas fa-check-circle"></i>
                    </div>
                    <div class="stat-label">Đã hoàn thành</div>
                </div>
                <div class="stat-number completed">@ViewBag.CompletedTasks</div>
            </div>
        </div>
        <div class="col-lg-2 col-md-4 col-sm-6 mb-3">
            <div class="stat-card paused">
                <div class="stat-card-header">
                    <div class="stat-icon paused">
                        <i class="fas fa-pause-circle"></i>
                    </div>
                    <div class="stat-label">Quá hạn</div>
                </div>
                <div class="stat-number paused">@ViewBag.PausedTasks</div>
            </div>
        </div>
    </div>

    <!-- Biểu đồ -->
    <div class="row mb-3">
        <div class="col-lg-6 mb-3">
            <div class="chart-card">
                <h5><i class="fas fa-chart-pie me-2"></i>Phân bố công việc theo trạng thái</h5>
                <canvas id="statusPieChart" style="max-height: 250px;"></canvas>
            </div>
        </div>
        <div class="col-lg-6 mb-3">
            <div class="chart-card">
                <h5><i class="fas fa-chart-bar me-2"></i>Thống kê công việc theo trạng thái</h5>
                <canvas id="statusBarChart" style="max-height: 250px;"></canvas>
            </div>
        </div>
    </div>

    <!-- Thống kê theo kỹ thuật viên và nền tảng -->
    <div class="row mb-3">
        <div class="col-lg-8 mb-3">
            <div class="chart-card">
                <h5><i class="fas fa-users me-2"></i>Thống kê công việc theo kỹ thuật viên</h5>
                <canvas id="technicianChart" style="max-height: 280px;"></canvas>
            </div>
        </div>
        <div class="col-lg-4 mb-3">
            <div class="chart-card">
                <h5><i class="fas fa-globe me-2"></i>Thống kê theo nền tảng hỗ trợ</h5>
                <canvas id="platformChart" style="max-height: 280px;"></canvas>
            </div>
        </div>
    </div>

    <!-- Thống kê công việc của từng thành viên -->
    <div class="row mb-3">
        <div class="col-12">
            <div class="chart-card">
                <h5><i class="fas fa-user-tie me-2"></i>Thống kê công việc của từng thành viên</h5>
                <div class="row">
                    @Code
                        If technicianStats IsNot Nothing Then
                            For Each techStat In technicianStats
                                WriteLiteral("<div class=""col-lg-3 col-md-4 col-sm-6 mb-3"">")
                                WriteLiteral("<div class=""technician-card"" onclick=""showTechnicianTasks('" & techStat.Id & "', '" & System.Web.HttpUtility.JavaScriptStringEncode(System.Web.HttpUtility.HtmlDecode(techStat.Name)) & "')"" style=""cursor: pointer;"">")
                                WriteLiteral("<div class=""d-flex align-items-center mb-3"">")
                                WriteLiteral("<div class=""technician-avatar"">")
                                WriteLiteral("<i class=""fas fa-user""></i>")
                                WriteLiteral("</div>")
                                WriteLiteral("<div class=""ms-3 flex-grow-1"">")
                                WriteLiteral("<h6 class=""mb-0"" style=""font-weight: 600; color: var(--primary-darker); font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;"">" & System.Web.HttpUtility.HtmlDecode(techStat.Name) & "</h6>")
                                WriteLiteral("<small class=""text-muted"">Kỹ thuật viên</small>")
                                WriteLiteral("</div>")
                                WriteLiteral("</div>")
                                WriteLiteral("<div class=""technician-stats"">")
                                WriteLiteral("<div class=""stat-item"">")
                                WriteLiteral("<span class=""stat-label"">Tổng công việc</span>")
                                WriteLiteral("<span class=""stat-value text-primary"">" & techStat.Total & "</span>")
                                WriteLiteral("</div>")
                                WriteLiteral("<div class=""row g-2 mt-2"">")
                                WriteLiteral("<div class=""col-6""><span class=""badge bg-warning"">🟡 " & techStat.Pending & "</span></div>")
                                WriteLiteral("<div class=""col-6""><span class=""badge bg-info"">🔵 " & techStat.InProgress & "</span></div>")
                                WriteLiteral("<div class=""col-6""><span class=""badge"" style=""background-color: #f59e0b;"">🟠 " & techStat.Waiting & "</span></div>")
                                WriteLiteral("<div class=""col-6""><span class=""badge bg-success"">🟢 " & techStat.Completed & "</span></div>")
                                WriteLiteral("</div>")
                                WriteLiteral("</div>")
                                WriteLiteral("</div>")
                                WriteLiteral("</div>")
                            Next
                        End If
                    End Code
                </div>
            </div>
        </div>
    </div>

    <!-- Kanban Board -->
    <div class="kanban-section">
        <h3><i class="fas fa-columns me-2"></i>Kanban Board</h3>
        
        <div class="row justify-content-center">
            <!-- Chưa xử lý -->
            <div class="col-lg-2 col-md-4 col-sm-6 mb-3" style="flex: 0 0 20%; max-width: 20%;">
                <div class="kanban-column">
                    <h5>
                        <span class="status-badge status-pending">🟡 Chưa xử lý</span>
                        <span class="badge bg-secondary">@pendingCount</span>
                    </h5>
                    <div class="kanban-column-content">
                    @Code
                        If tasksByStatus IsNot Nothing AndAlso tasksByStatus.ContainsKey(TaskStatus.Pending) Then
                            For Each task In tasksByStatus(TaskStatus.Pending)
                                Dim desc = If(task.Description, "")
                                Dim shortDesc = ""
                                If desc IsNot Nothing AndAlso desc.Length > 0 Then
                                    ' Strip HTML tags để tránh style ảnh hưởng các task khác
                                    Dim plainText = System.Text.RegularExpressions.Regex.Replace(desc, "<.*?>", "")
                                    If plainText.Length > 50 Then
                                        shortDesc = plainText.Substring(0, 50) & "..."
                                    Else
                                        shortDesc = plainText
                                    End If
                                End If
                                
                                WriteLiteral("<div class=""kanban-card"" data-task-id=""" & task.Id & """ onclick=""if(!event.target.closest('.status-dropdown') && !event.target.closest('.btn-save-task')) { location.href='" & Url.Action("Details", "Tasks", New With {.id = task.Id}) & "'; }"">")
                                WriteLiteral("<div class=""d-flex justify-content-between align-items-start mb-2"">")
                                WriteLiteral("<h6 class=""mb-0 flex-grow-1"" style=""color: inherit; font-weight: inherit; text-decoration: none;"">")
                                If Not String.IsNullOrEmpty(shortDesc) Then
                                    WriteLiteral(System.Web.HttpUtility.HtmlEncode(shortDesc))
                                Else
                                    WriteLiteral("<span class=""text-muted"">Không có mô tả</span>")
                                End If
                                WriteLiteral("</h6>")
                                WriteLiteral("<div class=""d-flex gap-1"">")
                                WriteLiteral("<button type=""button"" class=""btn btn-sm btn-outline-primary btn-save-task"" data-id=""" & task.Id & """ data-saved=""" & task.IsSaved.ToString().ToLower() & """ title=""" & (If(task.IsSaved, "Bỏ lưu", "Lưu câu trả lời")) & """ onclick=""event.stopPropagation();"">")
                                WriteLiteral("<i class=""fas fa-bookmark " & (If(task.IsSaved, "text-danger", "")) & """></i>")
                                WriteLiteral("</button>")
                                WriteLiteral("<select class=""form-select form-select-sm status-dropdown"" style=""width: auto; min-width: 120px;"" onclick=""event.stopPropagation();"" onchange=""updateStatus('" & task.Id & "', this.value)"">")
                                WriteLiteral("<option value=""0""" & If(task.Status = TaskStatus.Pending, " selected", "") & ">🟡 Chưa xử lý</option>")
                                WriteLiteral("<option value=""1""" & If(task.Status = TaskStatus.InProgress, " selected", "") & ">🔵 Đang xử lý</option>")
                                WriteLiteral("<option value=""2""" & If(task.Status = TaskStatus.Waiting, " selected", "") & ">🟠 Chờ phản hồi</option>")
                                WriteLiteral("<option value=""3""" & If(task.Status = TaskStatus.Completed, " selected", "") & ">🟢 Đã hoàn thành</option>")
                                WriteLiteral("<option value=""4""" & If(task.Status = TaskStatus.Paused, " selected", "") & ">🔴 Quá hạn</option>")
                                WriteLiteral("</select>")
                                WriteLiteral("</div>")
                                WriteLiteral("</div>")
                                WriteLiteral("<p class=""text-muted small mb-2"">")
                                WriteLiteral("<i class=""fas fa-user-tie me-1""></i>" & task.CustomerName)
                                If Not String.IsNullOrEmpty(task.SoftwareName) Then
                                    WriteLiteral("<br/><i class=""fas fa-laptop-code me-1""></i>" & task.SoftwareName)
                                End If
                                WriteLiteral("</p>")
                                WriteLiteral("<div class=""d-flex justify-content-between align-items-center"">")
                                WriteLiteral("<small class=""text-muted""><i class=""fas fa-user me-1""></i>" & task.AssignedToName & "</small>")
                                WriteLiteral("<small class=""text-muted""><i class=""fas fa-calendar me-1""></i>" & task.CreatedDate.ToString("dd/MM/yyyy") & "</small>")
                                WriteLiteral("</div></div>")
                            Next
                        End If
                    End Code
                    </div>
                </div>
            </div>

            <!-- Đang xử lý -->
            <div class="col-lg-2 col-md-4 col-sm-6 mb-3" style="flex: 0 0 20%; max-width: 20%;">
                <div class="kanban-column">
                    <h5>
                        <span class="status-badge status-inprogress">🔵 Đang xử lý</span>
                        <span class="badge bg-secondary">@inProgressCount</span>
                    </h5>
                    <div class="kanban-column-content">
                    @Code
                        If tasksByStatus IsNot Nothing AndAlso tasksByStatus.ContainsKey(TaskStatus.InProgress) Then
                            For Each task In tasksByStatus(TaskStatus.InProgress)
                                Dim desc = If(task.Description, "")
                                Dim shortDesc = ""
                                If desc IsNot Nothing AndAlso desc.Length > 0 Then
                                    ' Strip HTML tags để tránh style ảnh hưởng các task khác
                                    Dim plainText = System.Text.RegularExpressions.Regex.Replace(desc, "<.*?>", "")
                                    If plainText.Length > 50 Then
                                        shortDesc = plainText.Substring(0, 50) & "..."
                                    Else
                                        shortDesc = plainText
                                    End If
                                End If
                                
                                WriteLiteral("<div class=""kanban-card"" data-task-id=""" & task.Id & """ onclick=""if(!event.target.closest('.status-dropdown') && !event.target.closest('.btn-save-task')) { location.href='" & Url.Action("Details", "Tasks", New With {.id = task.Id}) & "'; }"">")
                                WriteLiteral("<div class=""d-flex justify-content-between align-items-start mb-2"">")
                                WriteLiteral("<h6 class=""mb-0 flex-grow-1"" style=""color: inherit; font-weight: inherit; text-decoration: none;"">")
                                If Not String.IsNullOrEmpty(shortDesc) Then
                                    WriteLiteral(System.Web.HttpUtility.HtmlEncode(shortDesc))
                                Else
                                    WriteLiteral("<span class=""text-muted"">Không có mô tả</span>")
                                End If
                                WriteLiteral("</h6>")
                                WriteLiteral("<div class=""d-flex gap-1"">")
                                WriteLiteral("<button type=""button"" class=""btn btn-sm btn-outline-primary btn-save-task"" data-id=""" & task.Id & """ data-saved=""" & task.IsSaved.ToString().ToLower() & """ title=""" & (If(task.IsSaved, "Bỏ lưu", "Lưu câu trả lời")) & """ onclick=""event.stopPropagation();"">")
                                WriteLiteral("<i class=""fas fa-bookmark " & (If(task.IsSaved, "text-danger", "")) & """></i>")
                                WriteLiteral("</button>")
                                WriteLiteral("<select class=""form-select form-select-sm status-dropdown"" style=""width: auto; min-width: 120px;"" onclick=""event.stopPropagation();"" onchange=""updateStatus('" & task.Id & "', this.value)"">")
                                WriteLiteral("<option value=""0"">🟡 Chưa xử lý</option>")
                                WriteLiteral("<option value=""1"" selected>🔵 Đang xử lý</option>")
                                WriteLiteral("<option value=""2"">🟠 Chờ phản hồi</option>")
                                WriteLiteral("<option value=""3"">🟢 Đã hoàn thành</option>")
                                WriteLiteral("<option value=""4"">🔴 Quá hạn</option>")
                                WriteLiteral("</select>")
                                WriteLiteral("</div>")
                                WriteLiteral("</div>")
                                WriteLiteral("<p class=""text-muted small mb-2"">")
                                WriteLiteral("<i class=""fas fa-user-tie me-1""></i>" & task.CustomerName)
                                If Not String.IsNullOrEmpty(task.SoftwareName) Then
                                    WriteLiteral("<br/><i class=""fas fa-laptop-code me-1""></i>" & task.SoftwareName)
                                End If
                                WriteLiteral("</p>")
                                WriteLiteral("<div class=""d-flex justify-content-between align-items-center"">")
                                WriteLiteral("<small class=""text-muted""><i class=""fas fa-user me-1""></i>" & task.AssignedToName & "</small>")
                                WriteLiteral("<small class=""text-muted""><i class=""fas fa-calendar me-1""></i>" & task.CreatedDate.ToString("dd/MM/yyyy") & "</small>")
                                WriteLiteral("</div></div>")
                            Next
                        End If
                    End Code
                    </div>
                </div>
            </div>

            <!-- Chờ phản hồi -->
            <div class="col-lg-2 col-md-4 col-sm-6 mb-3" style="flex: 0 0 20%; max-width: 20%;">
                <div class="kanban-column">
                    <h5>
                        <span class="status-badge status-waiting">🟠 Chờ phản hồi</span>
                        <span class="badge bg-secondary">@waitingCount</span>
                    </h5>
                    <div class="kanban-column-content">
                    @Code
                        If tasksByStatus IsNot Nothing AndAlso tasksByStatus.ContainsKey(TaskStatus.Waiting) Then
                            For Each task In tasksByStatus(TaskStatus.Waiting)
                                Dim desc = If(task.Description, "")
                                Dim shortDesc = ""
                                If desc IsNot Nothing AndAlso desc.Length > 0 Then
                                    ' Strip HTML tags để tránh style ảnh hưởng các task khác
                                    Dim plainText = System.Text.RegularExpressions.Regex.Replace(desc, "<.*?>", "")
                                    If plainText.Length > 50 Then
                                        shortDesc = plainText.Substring(0, 50) & "..."
                                    Else
                                        shortDesc = plainText
                                    End If
                                End If
                                
                                WriteLiteral("<div class=""kanban-card"" data-task-id=""" & task.Id & """ onclick=""if(!event.target.closest('.status-dropdown') && !event.target.closest('.btn-save-task')) { location.href='" & Url.Action("Details", "Tasks", New With {.id = task.Id}) & "'; }"">")
                                WriteLiteral("<div class=""d-flex justify-content-between align-items-start mb-2"">")
                                WriteLiteral("<h6 class=""mb-0 flex-grow-1"" style=""color: inherit; font-weight: inherit; text-decoration: none;"">")
                                If Not String.IsNullOrEmpty(shortDesc) Then
                                    WriteLiteral(System.Web.HttpUtility.HtmlEncode(shortDesc))
                                Else
                                    WriteLiteral("<span class=""text-muted"">Không có mô tả</span>")
                                End If
                                WriteLiteral("</h6>")
                                WriteLiteral("<div class=""d-flex gap-1"">")
                                WriteLiteral("<button type=""button"" class=""btn btn-sm btn-outline-primary btn-save-task"" data-id=""" & task.Id & """ data-saved=""" & task.IsSaved.ToString().ToLower() & """ title=""" & (If(task.IsSaved, "Bỏ lưu", "Lưu câu trả lời")) & """ onclick=""event.stopPropagation();"">")
                                WriteLiteral("<i class=""fas fa-bookmark " & (If(task.IsSaved, "text-danger", "")) & """></i>")
                                WriteLiteral("</button>")
                                WriteLiteral("<select class=""form-select form-select-sm status-dropdown"" style=""width: auto; min-width: 120px;"" onclick=""event.stopPropagation();"" onchange=""updateStatus('" & task.Id & "', this.value)"">")
                                WriteLiteral("<option value=""0"">🟡 Chưa xử lý</option>")
                                WriteLiteral("<option value=""1"">🔵 Đang xử lý</option>")
                                WriteLiteral("<option value=""2"" selected>🟠 Chờ phản hồi</option>")
                                WriteLiteral("<option value=""3"">🟢 Đã hoàn thành</option>")
                                WriteLiteral("<option value=""4"">🔴 Quá hạn</option>")
                                WriteLiteral("</select>")
                                WriteLiteral("</div>")
                                WriteLiteral("</div>")
                                WriteLiteral("<p class=""text-muted small mb-2"">")
                                WriteLiteral("<i class=""fas fa-user-tie me-1""></i>" & task.CustomerName)
                                If Not String.IsNullOrEmpty(task.SoftwareName) Then
                                    WriteLiteral("<br/><i class=""fas fa-laptop-code me-1""></i>" & task.SoftwareName)
                                End If
                                WriteLiteral("</p>")
                                WriteLiteral("<div class=""d-flex justify-content-between align-items-center"">")
                                WriteLiteral("<small class=""text-muted""><i class=""fas fa-user me-1""></i>" & task.AssignedToName & "</small>")
                                WriteLiteral("<small class=""text-muted""><i class=""fas fa-calendar me-1""></i>" & task.CreatedDate.ToString("dd/MM/yyyy") & "</small>")
                                WriteLiteral("</div></div>")
                            Next
                        End If
                    End Code
                    </div>
                </div>
            </div>

            <!-- Đã hoàn thành -->
            <div class="col-lg-2 col-md-4 col-sm-6 mb-3" style="flex: 0 0 20%; max-width: 20%;">
                <div class="kanban-column">
                    <h5>
                        <span class="status-badge status-completed">🟢 Đã hoàn thành</span>
                        <span class="badge bg-secondary">@completedCount</span>
                    </h5>
                    <div class="kanban-column-content">
                    @Code
                        If tasksByStatus IsNot Nothing AndAlso tasksByStatus.ContainsKey(TaskStatus.Completed) Then
                            For Each task In tasksByStatus(TaskStatus.Completed)
                                Dim desc = If(task.Description, "")
                                Dim shortDesc = ""
                                If desc IsNot Nothing AndAlso desc.Length > 0 Then
                                    ' Strip HTML tags để tránh style ảnh hưởng các task khác
                                    Dim plainText = System.Text.RegularExpressions.Regex.Replace(desc, "<.*?>", "")
                                    If plainText.Length > 50 Then
                                        shortDesc = plainText.Substring(0, 50) & "..."
                                    Else
                                        shortDesc = plainText
                                    End If
                                End If
                                
                                WriteLiteral("<div class=""kanban-card"" data-task-id=""" & task.Id & """ onclick=""if(!event.target.closest('.status-dropdown') && !event.target.closest('.btn-save-task')) { location.href='" & Url.Action("Details", "Tasks", New With {.id = task.Id}) & "'; }"">")
                                WriteLiteral("<div class=""d-flex justify-content-between align-items-start mb-2"">")
                                WriteLiteral("<h6 class=""mb-0 flex-grow-1"" style=""color: inherit; font-weight: inherit; text-decoration: none;"">")
                                If Not String.IsNullOrEmpty(shortDesc) Then
                                    WriteLiteral(System.Web.HttpUtility.HtmlEncode(shortDesc))
                                Else
                                    WriteLiteral("<span class=""text-muted"">Không có mô tả</span>")
                                End If
                                WriteLiteral("</h6>")
                                WriteLiteral("<div class=""d-flex gap-1"">")
                                WriteLiteral("<button type=""button"" class=""btn btn-sm btn-outline-primary btn-save-task"" data-id=""" & task.Id & """ data-saved=""" & task.IsSaved.ToString().ToLower() & """ title=""" & (If(task.IsSaved, "Bỏ lưu", "Lưu câu trả lời")) & """ onclick=""event.stopPropagation();"">")
                                WriteLiteral("<i class=""fas fa-bookmark " & (If(task.IsSaved, "text-danger", "")) & """></i>")
                                WriteLiteral("</button>")
                                WriteLiteral("<select class=""form-select form-select-sm status-dropdown"" style=""width: auto; min-width: 120px;"" onclick=""event.stopPropagation();"" onchange=""updateStatus('" & task.Id & "', this.value)"">")
                                WriteLiteral("<option value=""0"">🟡 Chưa xử lý</option>")
                                WriteLiteral("<option value=""1"">🔵 Đang xử lý</option>")
                                WriteLiteral("<option value=""2"">🟠 Chờ phản hồi</option>")
                                WriteLiteral("<option value=""3"" selected>🟢 Đã hoàn thành</option>")
                                WriteLiteral("<option value=""4"">🔴 Quá hạn</option>")
                                WriteLiteral("</select>")
                                WriteLiteral("</div>")
                                WriteLiteral("</div>")
                                WriteLiteral("<p class=""text-muted small mb-2"">")
                                WriteLiteral("<i class=""fas fa-user-tie me-1""></i>" & task.CustomerName)
                                If Not String.IsNullOrEmpty(task.SoftwareName) Then
                                    WriteLiteral("<br/><i class=""fas fa-laptop-code me-1""></i>" & task.SoftwareName)
                                End If
                                WriteLiteral("</p>")
                                WriteLiteral("<div class=""d-flex justify-content-between align-items-center"">")
                                WriteLiteral("<small class=""text-muted""><i class=""fas fa-user me-1""></i>" & task.AssignedToName & "</small>")
                                WriteLiteral("<small class=""text-muted""><i class=""fas fa-check-circle me-1""></i>")
                                If task.CompletedDate.HasValue Then
                                    WriteLiteral(task.CompletedDate.Value.ToString("dd/MM/yyyy"))
                                End If
                                WriteLiteral("</small></div></div>")
                            Next
                        End If
                    End Code
                    </div>
                </div>
            </div>

            <!-- Quá hạn -->
            <div class="col-lg-2 col-md-4 col-sm-6 mb-3" style="flex: 0 0 20%; max-width: 20%;">
                <div class="kanban-column">
                    <h5>
                        <span class="status-badge status-paused">🔴 Quá hạn</span>
                        <span class="badge bg-secondary">@pausedCount</span>
                    </h5>
                    <div class="kanban-column-content">
                    @Code
                        If tasksByStatus IsNot Nothing AndAlso tasksByStatus.ContainsKey(TaskStatus.Paused) Then
                            For Each task In tasksByStatus(TaskStatus.Paused)
                                Dim desc = If(task.Description, "")
                                Dim shortDesc = ""
                                If desc IsNot Nothing AndAlso desc.Length > 0 Then
                                    ' Strip HTML tags để tránh style ảnh hưởng các task khác
                                    Dim plainText = System.Text.RegularExpressions.Regex.Replace(desc, "<.*?>", "")
                                    If plainText.Length > 50 Then
                                        shortDesc = plainText.Substring(0, 50) & "..."
                                    Else
                                        shortDesc = plainText
                                    End If
                                End If
                                
                                WriteLiteral("<div class=""kanban-card"" data-task-id=""" & task.Id & """ onclick=""if(!event.target.closest('.status-dropdown') && !event.target.closest('.btn-save-task')) { location.href='" & Url.Action("Details", "Tasks", New With {.id = task.Id}) & "'; }"">")
                                WriteLiteral("<div class=""d-flex justify-content-between align-items-start mb-2"">")
                                WriteLiteral("<h6 class=""mb-0 flex-grow-1"" style=""color: inherit; font-weight: inherit; text-decoration: none;"">")
                                If Not String.IsNullOrEmpty(shortDesc) Then
                                    WriteLiteral(System.Web.HttpUtility.HtmlEncode(shortDesc))
                                Else
                                    WriteLiteral("<span class=""text-muted"">Không có mô tả</span>")
                                End If
                                WriteLiteral("</h6>")
                                WriteLiteral("<div class=""d-flex gap-1"">")
                                WriteLiteral("<button type=""button"" class=""btn btn-sm btn-outline-primary btn-save-task"" data-id=""" & task.Id & """ data-saved=""" & task.IsSaved.ToString().ToLower() & """ title=""" & (If(task.IsSaved, "Bỏ lưu", "Lưu câu trả lời")) & """ onclick=""event.stopPropagation();"">")
                                WriteLiteral("<i class=""fas fa-bookmark " & (If(task.IsSaved, "text-danger", "")) & """></i>")
                                WriteLiteral("</button>")
                                WriteLiteral("<select class=""form-select form-select-sm status-dropdown"" style=""width: auto; min-width: 120px;"" onclick=""event.stopPropagation();"" onchange=""updateStatus('" & task.Id & "', this.value)"">")
                                WriteLiteral("<option value=""0"">🟡 Chưa xử lý</option>")
                                WriteLiteral("<option value=""1"">🔵 Đang xử lý</option>")
                                WriteLiteral("<option value=""2"">🟠 Chờ phản hồi</option>")
                                WriteLiteral("<option value=""3"">🟢 Đã hoàn thành</option>")
                                WriteLiteral("<option value=""4"" selected>🔴 Tạm dừng</option>")
                                WriteLiteral("</select>")
                                WriteLiteral("</div>")
                                WriteLiteral("</div>")
                                WriteLiteral("<p class=""text-muted small mb-2"">")
                                WriteLiteral("<i class=""fas fa-user-tie me-1""></i>" & task.CustomerName)
                                If Not String.IsNullOrEmpty(task.SoftwareName) Then
                                    WriteLiteral("<br/><i class=""fas fa-laptop-code me-1""></i>" & task.SoftwareName)
                                End If
                                WriteLiteral("</p>")
                                WriteLiteral("<div class=""d-flex justify-content-between align-items-center"">")
                                WriteLiteral("<small class=""text-muted""><i class=""fas fa-user me-1""></i>" & task.AssignedToName & "</small>")
                                WriteLiteral("<small class=""text-muted""><i class=""fas fa-calendar me-1""></i>" & task.CreatedDate.ToString("dd/MM/yyyy") & "</small>")
                                WriteLiteral("</div></div>")
                            Next
                        End If
                    End Code
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>

@section scripts
    <script src="https://cdn.jsdelivr.net/npm/chart.js@4.4.0/dist/chart.umd.min.js"></script>
    <script>
        // Dữ liệu cho biểu đồ
        // Cấu hình font mặc định cho Chart.js để hỗ trợ tiếng Việt
        Chart.defaults.font.family = "'Segoe UI', Tahoma, Geneva, Verdana, sans-serif";
        Chart.defaults.font.size = 12;
        
        const pendingCount = @ViewBag.PendingTasks;
        const inProgressCount = @ViewBag.InProgressTasks;
        const waitingCount = @ViewBag.WaitingTasks;
        const completedCount = @ViewBag.CompletedTasks;
        const pausedCount = @ViewBag.PausedTasks;

        // Biểu đồ tròn (Pie Chart)
        const pieCtx = document.getElementById('statusPieChart').getContext('2d');
        new Chart(pieCtx, {
            type: 'pie',
            data: {
                labels: ['Chưa xử lý', 'Đang xử lý', 'Chờ phản hồi', 'Đã hoàn thành', 'Quá hạn'],
                datasets: [{
                    data: [pendingCount, inProgressCount, waitingCount, completedCount, pausedCount],
                    backgroundColor: [
                        '#f59e0b',
                        '#0d9488',
                        '#ea580c',
                        '#22c55e',
                        '#be123c'
                    ],
                    borderColor: '#ffffff',
                    borderWidth: 2
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: true,
                plugins: {
                    legend: {
                        position: 'bottom',
                        labels: {
                            padding: 15,
                            font: {
                                size: 12,
                                weight: '600'
                            }
                        }
                    },
                    tooltip: {
                        callbacks: {
                            label: function(context) {
                                const label = context.label || '';
                                const value = context.parsed || 0;
                                const total = context.dataset.data.reduce((a, b) => a + b, 0);
                                const percentage = total > 0 ? ((value / total) * 100).toFixed(1) : 0;
                                return label + ': ' + value + ' (' + percentage + '%)';
                            }
                        }
                    }
                }
            }
        });

        // Biểu đồ cột (Bar Chart)
        const barCtx = document.getElementById('statusBarChart').getContext('2d');
        new Chart(barCtx, {
            type: 'bar',
            data: {
                labels: ['Chưa xử lý', 'Đang xử lý', 'Chờ phản hồi', 'Đã hoàn thành', 'Quá hạn'],
                datasets: [{
                    label: 'Số lượng công việc',
                    data: [pendingCount, inProgressCount, waitingCount, completedCount, pausedCount],
                    backgroundColor: [
                        '#f59e0b',
                        '#0d9488',
                        '#ea580c',
                        '#22c55e',
                        '#be123c'
                    ],
                    borderRadius: 6
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: true,
                plugins: {
                    legend: {
                        display: false
                    },
                    tooltip: {
                        callbacks: {
                            label: function(context) {
                                return 'Số lượng: ' + context.parsed.y;
                            }
                        }
                    }
                },
                scales: {
                    y: {
                        beginAtZero: true,
                        ticks: {
                            stepSize: 1,
                            font: {
                                size: 11
                            }
                        },
                        grid: {
                            color: 'rgba(0, 0, 0, 0.05)'
                        }
                    },
                    x: {
                        ticks: {
                            font: {
                                size: 11,
                                weight: '600'
                            }
                        },
                        grid: {
                            display: false
                        }
                    }
                }
            }
        });

        // Biểu đồ thống kê theo kỹ thuật viên
        const technicianLabels = [
            @For i = 0 To techLabels.Count - 1
                @If i > 0 Then
                    @<text>,</text>
                End If
                @<text>'@Html.Raw(techLabels(i))'</text>
            Next
        ];
        
        const technicianData = [
            @For i = 0 To techData.Count - 1
                @If i > 0 Then
                    @<text>,</text>
                End If
                @techData(i)
            Next
        ];

        const technicianCtx = document.getElementById('technicianChart').getContext('2d');
        
        // Tạo gradient cho biểu đồ
        const gradient = technicianCtx.createLinearGradient(0, 0, 0, 400);
        gradient.addColorStop(0, 'rgba(76, 175, 80, 0.9)');
        gradient.addColorStop(1, 'rgba(56, 142, 60, 0.7)');
        
        new Chart(technicianCtx, {
            type: 'bar',
            data: {
                labels: technicianLabels,
                datasets: [{
                    label: 'Số lượng công việc',
                    data: technicianData,
                    backgroundColor: gradient,
                    borderColor: '#4CAF50',
                    borderWidth: 2,
                    borderRadius: 8
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: true,
                indexAxis: 'y',
                plugins: {
                    legend: {
                        display: false
                    },
                    tooltip: {
                        callbacks: {
                            label: function(context) {
                                return 'Số lượng: ' + context.parsed.x;
                            }
                        }
                    }
                },
                scales: {
                    x: {
                        beginAtZero: true,
                        ticks: {
                            stepSize: 1,
                            font: {
                                size: 12,
                                family: "'Segoe UI', Tahoma, Geneva, Verdana, sans-serif",
                                weight: '500'
                            },
                            color: '#374151'
                        },
                        grid: {
                            color: 'rgba(0, 0, 0, 0.05)'
                        }
                    },
                    y: {
                        ticks: {
                            font: {
                                size: 13,
                                family: "'Segoe UI', Tahoma, Geneva, Verdana, sans-serif",
                                weight: '600'
                            },
                            color: '#1f2937'
                        },
                        grid: {
                            display: false
                        }
                    }
                }
            }
        });

        // Biểu đồ thống kê theo nền tảng hỗ trợ
        const zaloCount = @ViewBag.ZaloCount;
        const memberSupportCount = @ViewBag.MemberSupportCount;
        const customerContactCount = @ViewBag.CustomerContactCount;

        const platformCtx = document.getElementById('platformChart').getContext('2d');
        new Chart(platformCtx, {
            type: 'doughnut',
            data: {
                labels: ['Zalo', 'Member Support', 'Khách liên hệ Sale'],
                datasets: [{
                    data: [zaloCount, memberSupportCount, customerContactCount],
                    backgroundColor: [
                        '#4CAF50',
                        '#0D6EFD',
                        '#FF8C00'
                    ],
                    borderColor: [
                        '#388E3C',
                        '#094BAC',
                        '#FF8C00'
                    ],
                    borderWidth: 2
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: true,
                plugins: {
                    legend: {
                        position: 'bottom',
                        labels: {
                            padding: 15,
                            font: {
                                size: 11,
                                weight: '600'
                            }
                        }
                    },
                    tooltip: {
                        callbacks: {
                            label: function(context) {
                                const label = context.label || '';
                                const value = context.parsed || 0;
                                const total = context.dataset.data.reduce((a, b) => a + b, 0);
                                const percentage = total > 0 ? ((value / total) * 100).toFixed(1) : 0;
                                return label + ': ' + value + ' (' + percentage + '%)';
                            }
                        }
                    }
                }
            }
        });

        // Function update status
        function updateStatus(taskId, status) {
            if (!taskId || status === undefined) {
                alert('Thông tin không hợp lệ');
                return;
            }
            
            const dropdown = event.target;
            const originalValue = dropdown.value;
            dropdown.disabled = true;
            
            const card = dropdown.closest('.kanban-card');
            if (card) {
                card.style.opacity = '0.6';
            }
            
            fetch('@Url.Action("UpdateStatus", "Tasks")', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                },
                body: `id=${encodeURIComponent(taskId)}&status=${encodeURIComponent(status)}`
            })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    location.reload();
                } else {
                    alert('Có lỗi xảy ra: ' + (data.message || 'Không thể cập nhật trạng thái'));
                    dropdown.value = originalValue;
                    dropdown.disabled = false;
                    if (card) {
                        card.style.opacity = '1';
                    }
                }
            })
            .catch(error => {
                console.error('Error:', error);
                alert('Có lỗi xảy ra khi cập nhật trạng thái');
                dropdown.value = originalValue;
                dropdown.disabled = false;
                if (card) {
                    card.style.opacity = '1';
                }
            });
        }

        $(document).on('click', '.btn-save-task', function(e) {
            e.stopPropagation();
            var btn = $(this);
            var id = btn.data('id');
            var currentSaved = btn.data('saved');
            var newSaved = !currentSaved;
            
            $.ajax({
                url: '@Url.Action("ToggleSaved", "Tasks")',
                type: 'POST',
                data: { id: id, isSaved: newSaved },
                success: function(response) {
                    if (response.success) {
                        btn.data('saved', newSaved);
                        var icon = btn.find('i');
                        if (newSaved) {
                            icon.addClass('text-danger');
                            btn.attr('title', 'Bỏ lưu');
                        } else {
                            icon.removeClass('text-danger');
                            btn.attr('title', 'Lưu câu trả lời');
                        }
                    } else {
                        alert('Có lỗi xảy ra khi thực hiện thao tác.');
                    }
                }
            });
        });

        // Hiển thị danh sách công việc của kỹ thuật viên
        function showTechnicianTasks(technicianId, technicianName) {
            document.getElementById('technicianModalTitle').textContent = 'Công việc của ' + technicianName;
            document.getElementById('technicianTasksList').innerHTML = '<div class="text-center py-4"><i class="fas fa-spinner fa-spin fa-2x text-primary"></i><p class="mt-2">Đang tải...</p></div>';
            
            const modal = new bootstrap.Modal(document.getElementById('technicianModal'));
            modal.show();
            
            fetch('@Url.Action("GetTasksByTechnician", "Dashboard")?technicianId=' + encodeURIComponent(technicianId))
                .then(response => response.json())
                .then(data => {
                    if (data.success && data.tasks) {
                        renderTechnicianTasks(data.tasks);
                    } else {
                        document.getElementById('technicianTasksList').innerHTML = '<div class="text-center py-4 text-muted"><i class="fas fa-inbox fa-2x mb-2"></i><p>Không có công việc nào</p></div>';
                    }
                })
                .catch(error => {
                    console.error('Error:', error);
                    document.getElementById('technicianTasksList').innerHTML = '<div class="alert alert-danger">Có lỗi xảy ra khi tải dữ liệu</div>';
                });
        }

        // Hàm escape HTML để tránh XSS
        function escapeHtml(text) {
            const div = document.createElement('div');
            div.textContent = text;
            return div.innerHTML;
        }
        
        // Render danh sách công việc
        function renderTechnicianTasks(tasks) {
            const container = document.getElementById('technicianTasksList');
            if (!tasks || tasks.length === 0) {
                container.innerHTML = '<div class="text-center py-4 text-muted"><i class="fas fa-inbox fa-2x mb-2"></i><p>Không có công việc nào</p></div>';
                return;
            }

            let html = '';
            tasks.forEach(task => {
                const statusClass = getStatusClass(task.status);
                const statusText = getStatusText(task.status);
                // Strip HTML tags để tránh style ảnh hưởng
                let desc = 'Không có mô tả';
                if (task.description) {
                    const tempDiv = document.createElement('div');
                    tempDiv.innerHTML = task.description;
                    const plainText = tempDiv.textContent || tempDiv.innerText || '';
                    desc = plainText.length > 100 ? plainText.substring(0, 100) + '...' : plainText;
                }
                
                html += `
                    <div class="task-item-card ${statusClass}" data-task-id="${task.id}">
                        <div class="d-flex justify-content-between align-items-start mb-2">
                            <div class="flex-grow-1 me-2">
                                <h6>${escapeHtml(desc)}</h6>
                                <div class="d-flex flex-wrap gap-2 mb-2" style="font-size: 0.75rem;">
                                    <span class="text-muted">
                                        <i class="fas fa-user-tie me-1"></i>${task.customerName || '-'}
                                    </span>
                                    ${task.softwareName ? `<span class="text-muted"><i class="fas fa-laptop-code me-1"></i>${task.softwareName}</span>` : ''}
                                    <span class="text-muted">
                                        <i class="fas fa-calendar me-1"></i>${formatDate(task.createdDate)}
                                    </span>
                                </div>
                            </div>
                            <span class="badge ${getStatusBadgeClass(task.status)} align-self-start">${statusText}</span>
                        </div>
                        <div class="d-flex justify-content-end align-items-center gap-2 pt-2 border-top" style="border-color: #f1f5f9;">
                            <button type="button" class="btn btn-sm btn-outline-primary btn-save-task" data-id="${task.id}" data-saved="${task.isSaved}" title="${task.isSaved ? 'Bỏ lưu' : 'Lưu câu trả lời'}">
                                <i class="fas fa-bookmark ${task.isSaved ? 'text-danger' : ''}"></i>
                            </button>
                            <select class="form-select form-select-sm status-select" style="width: auto; min-width: 140px;" onchange="updateTaskStatus('${task.id}', this.value)">
                                <option value="0" ${task.status == 0 ? 'selected' : ''}>🟡 Chưa xử lý</option>
                                <option value="1" ${task.status == 1 ? 'selected' : ''}>🔵 Đang xử lý</option>
                                <option value="2" ${task.status == 2 ? 'selected' : ''}>🟠 Chờ phản hồi</option>
                                <option value="3" ${task.status == 3 ? 'selected' : ''}>🟢 Đã hoàn thành</option>
                                <option value="4" ${task.status == 4 ? 'selected' : ''}>🔴 Quá hạn</option>
                            </select>
                            <button class="btn btn-sm btn-primary" onclick="quickReply('${task.id}')" title="Trả lời nhanh">
                                <i class="fas fa-reply"></i>
                            </button>
                            <a href="@Url.Action("Details", "Tasks")?id=${task.id}" class="btn btn-sm btn-info" title="Xem chi tiết">
                                <i class="fas fa-eye"></i>
                            </a>
                        </div>
                    </div>
                `;
            });
            container.innerHTML = html;
        }

        function getStatusClass(status) {
            switch(status) {
                case 0: return 'pending';
                case 1: return 'inprogress';
                case 2: return 'waiting';
                case 3: return 'completed';
                case 4: return 'paused';
                default: return '';
            }
        }

        function getStatusText(status) {
            switch(status) {
                case 0: return '🟡 Chưa xử lý';
                case 1: return '🔵 Đang xử lý';
                case 2: return '🟠 Chờ phản hồi';
                case 3: return '🟢 Đã hoàn thành';
                case 4: return '🔴 Quá hạn';
                default: return '';
            }
        }

        function getStatusBadgeClass(status) {
            switch(status) {
                case 0: return 'bg-warning';
                case 1: return 'bg-info';
                case 2: return 'bg-warning';
                case 3: return 'bg-success';
                case 4: return 'bg-danger';
                default: return 'bg-secondary';
            }
        }

        function formatDate(dateString) {
            if (!dateString) return '-';
            const date = new Date(dateString);
            return date.toLocaleDateString('vi-VN');
        }

        function updateTaskStatus(taskId, status) {
            updateStatus(taskId, status);
        }

        function quickReply(taskId) {
            const response = prompt('Nhập nội dung trả lời khách hàng:');
            if (response && response.trim()) {
                // Tạo form ẩn để lấy AntiForgeryToken
                const form = document.createElement('form');
                form.method = 'POST';
                form.action = '@Url.Action("ReplyToCustomer", "Tasks")';
                
                const tokenInput = document.createElement('input');
                tokenInput.type = 'hidden';
                tokenInput.name = '__RequestVerificationToken';
                tokenInput.value = document.querySelector('input[name="__RequestVerificationToken"]')?.value || '';
                
                const idInput = document.createElement('input');
                idInput.type = 'hidden';
                idInput.name = 'id';
                idInput.value = taskId;
                
                const responseInput = document.createElement('input');
                responseInput.type = 'hidden';
                responseInput.name = 'responseToCustomer';
                responseInput.value = response;
                
                form.appendChild(tokenInput);
                form.appendChild(idInput);
                form.appendChild(responseInput);
                document.body.appendChild(form);
                
                const formData = new FormData(form);
                
                fetch('@Url.Action("ReplyToCustomer", "Tasks")', {
                    method: 'POST',
                    body: formData
                })
                .then(response => response.json())
                .then(data => {
                    if (data.success) {
                        alert('Trả lời đã được gửi thành công!');
                        location.reload();
                    } else {
                        alert('Có lỗi xảy ra: ' + (data.message || 'Không thể gửi trả lời'));
                    }
                    document.body.removeChild(form);
                })
                .catch(error => {
                    console.error('Error:', error);
                    alert('Có lỗi xảy ra khi gửi trả lời');
                    document.body.removeChild(form);
                });
            }
        }
    </script>
End Section

<!-- Modal hiển thị danh sách công việc của kỹ thuật viên -->
<div class="modal fade" id="technicianModal" tabindex="-1" aria-labelledby="technicianModalLabel" aria-hidden="true">
    <div class="modal-dialog modal-xl">
        <div class="modal-content" style="border-radius: 8px; overflow: hidden;">
            <div class="modal-header bg-primary text-white" style="padding: 1rem 1.25rem; border-bottom: none;">
                <h5 class="modal-title mb-0" id="technicianModalTitle" style="font-size: 1.1rem; font-weight: 600;">
                    <i class="fas fa-user-tie me-2"></i>Công việc của kỹ thuật viên
                </h5>
                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body" style="max-height: 70vh; overflow-y: auto; padding: 1.25rem;">
                @Html.AntiForgeryToken()
                <div id="technicianTasksList">
                    <!-- Danh sách công việc sẽ được load ở đây -->
                </div>
            </div>
        </div>
    </div>
</div>
