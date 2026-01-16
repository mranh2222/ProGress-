@ModelType IEnumerable(Of Task)
@Code
    ViewData("Title") = "Danh sách công việc"
    
    ' Tính toán trước để tránh conflict
    Dim hasTasks As Boolean = False
    Dim taskList As List(Of Task) = New List(Of Task)
    
    If Model IsNot Nothing Then
        Try
            taskList = Model.ToList()
            hasTasks = (taskList.Count > 0)
        Catch ex As Exception
            hasTasks = False
            taskList = New List(Of Task)
        End Try
    End If
End Code

<style>
    .table-responsive {
        overflow-x: auto;
        -webkit-overflow-scrolling: touch;
    }
    
    .table {
        font-size: 0.8rem;
        width: 100%;
        table-layout: auto;
        min-width: 1400px;
    }
    
    .table thead th {
        font-size: 0.75rem;
        font-weight: 600;
        padding: 0.5rem 0.4rem;
        white-space: nowrap;
        background-color: var(--primary-color) !important;
        color: white !important;
        border: 1px solid var(--primary-darker);
        vertical-align: middle;
        text-align: center;
    }
    
    .table thead th:nth-child(10),
    .table thead th:nth-child(11) {
        white-space: normal;
        line-height: 1.3;
    }
    
    .table tbody td {
        font-size: 0.8rem;
        padding: 0.5rem 0.4rem;
        vertical-align: middle;
        word-wrap: break-word;
        word-break: break-word;
        max-width: 200px;
    }
    
    .table tbody td:first-child {
        max-width: 100px;
    }
    
    .table tbody td:nth-child(2),
    .table tbody td:nth-child(10),
    .table tbody td:nth-child(11) {
        max-width: 120px;
        white-space: nowrap;
    }
    
    .table tbody td:nth-child(3),
    .table tbody td:nth-child(4),
    .table tbody td:nth-child(6),
    .table tbody td:nth-child(8) {
        max-width: 150px;
    }
    
    .table tbody td:nth-child(5) {
        max-width: 180px;
    }
    
    .table tbody td:nth-child(7) {
        max-width: 250px;
        min-width: 200px;
    }
    
    .table tbody td:nth-child(9) {
        max-width: 140px;
        white-space: nowrap;
    }
    
    .table tbody td:last-child {
        max-width: 120px;
        white-space: nowrap;
    }
    
    .table .badge {
        font-size: 0.7rem;
        padding: 0.25rem 0.5rem;
    }
    
    .table .status-badge {
        font-size: 0.7rem;
        padding: 0.25rem 0.5rem;
        white-space: nowrap;
    }
    
    .table .btn-group .btn {
        font-size: 0.7rem;
        padding: 0.25rem 0.5rem;
    }
    
    .table .btn-group .btn i {
        font-size: 0.7rem;
    }
    
    .table-hover tbody tr:hover {
        background-color: rgba(37, 99, 235, 0.05);
    }
    
    .table-bordered {
        border: 1px solid #dee2e6;
    }
    
    .table-bordered th,
    .table-bordered td {
        border: 1px solid #dee2e6;
    }
    
    @@media (max-width: 1400px) {
        .table {
            min-width: 100%;
        }
    }
</style>

<div class="container-fluid">
    <div class="row mb-4">
        <div class="col-12 text-center mb-3">
            <h2 style="font-size: 1.5rem; font-weight: 600; color: var(--primary-darker);">
                <i class="fas fa-tasks me-2"></i>Danh sách công việc
            </h2>
        </div>
        <div class="col-12 text-center">
            <a href="@Url.Action("Create", "Tasks")" class="btn btn-primary">
                <i class="fas fa-plus me-2"></i>Tạo công việc mới
            </a>
        </div>
    </div>

    <div class="card">
        <div class="card-body">
            @If hasTasks Then
                @<div class="table-responsive" style="max-height: 80vh; overflow-y: auto;">
                    <table class="table table-hover table-bordered table-sm">
                        <thead class="table-light">
                            <tr>
                                <th>Tag</th>
                                <th>Ngày nhận file</th>
                                <th>Nền tảng hỗ trợ</th>
                                <th>Sale quản lý</th>
                                <th>Khách hàng</th>
                                <th>Phần mềm sử dụng</th>
                                <th>Mô tả lỗi / nội dung hỗ trợ</th>
                                <th>Kỹ thuật phụ trách</th>
                                <th>Tình trạng</th>
                                <th style="white-space: normal; line-height: 1.3;">Ngày dự kiến<br />hoàn thành</th>
                                <th style="white-space: normal; line-height: 1.3;">Ngày thực tế<br />hoàn thành</th>
                                <th>Thao tác</th>
                            </tr>
                        </thead>
                        <tbody>
                            @For Each item In taskList
                                @<tr>
                                    <td>
                                        @Code
                                            If Not String.IsNullOrEmpty(item.Tag) Then
                                                WriteLiteral("<span class=""badge bg-secondary"">" & item.Tag & "</span>")
                                            Else
                                                WriteLiteral("<span class=""text-muted"">-</span>")
                                            End If
                                        End Code
                                    </td>
                                    <td>
                                        @Code
                                            If item.FileReceivedDate.HasValue Then
                                                WriteLiteral(item.FileReceivedDate.Value.ToString("dd/MM/yyyy"))
                                            Else
                                                WriteLiteral("<span class=""text-muted"">-</span>")
                                            End If
                                        End Code
                                    </td>
                                    <td>
                                        @Code
                                            Select Case item.SupportPlatform
                                                Case SupportPlatform.Zalo
                                                    WriteLiteral("Zalo")
                                                Case SupportPlatform.MemberSupport
                                                    WriteLiteral("Member Support")
                                                Case SupportPlatform.CustomerContactSale
                                                    WriteLiteral("Khách liên hệ Sale")
                                                Case Else
                                                    WriteLiteral("<span class=""text-muted"">-</span>")
                                            End Select
                                        End Code
                                    </td>
                                    <td>
                                        @Code
                                            If Not String.IsNullOrEmpty(item.SaleManagerName) Then
                                                WriteLiteral(item.SaleManagerName)
                                            Else
                                                WriteLiteral("<span class=""text-muted"">-</span>")
                                            End If
                                        End Code
                                    </td>
                                    <td>@item.CustomerName</td>
                                    <td>
                                        @Code
                                            If Not String.IsNullOrEmpty(item.SoftwareName) Then
                                                WriteLiteral(item.SoftwareName)
                                            Else
                                                WriteLiteral("<span class=""text-muted"">-</span>")
                                            End If
                                        End Code
                                    </td>
                                    <td style="max-width: 250px;">
                                        @Code
                                            Dim desc = If(item.Description, "")
                                            ' Strip HTML tags để hiển thị preview
                                            If Not String.IsNullOrEmpty(desc) Then
                                                Dim plainText = System.Text.RegularExpressions.Regex.Replace(desc, "<.*?>", "")
                                                If plainText.Length > 80 Then
                                                    plainText = plainText.Substring(0, 80) & "..."
                                                End If
                                                WriteLiteral(plainText)
                                            Else
                                                WriteLiteral("<span class=""text-muted"">-</span>")
                                            End If
                                        End Code
                                    </td>
                                    <td>@item.AssignedToName</td>
                                    <td>
                                        @Select Case item.Status
                                            Case TaskStatus.Pending
                                                @<span class="status-badge status-pending">🟡 Chưa xử lý</span>
                                            Case TaskStatus.InProgress
                                                @<span class="status-badge status-inprogress">🔵 Đang xử lý</span>
                                            Case TaskStatus.Waiting
                                                @<span class="status-badge status-waiting">🟠 Chờ phản hồi</span>
                                            Case TaskStatus.Completed
                                                @<span class="status-badge status-completed">🟢 Đã hoàn thành</span>
                                            Case TaskStatus.Paused
                                                @<span class="status-badge status-paused">🔴 Quá hạn</span>
                                        End Select
                                    </td>
                                    <td>
                                        @Code
                                            If item.ExpectedCompletionDate.HasValue Then
                                                WriteLiteral(item.ExpectedCompletionDate.Value.ToString("dd/MM/yyyy"))
                                            Else
                                                WriteLiteral("<span class=""text-muted"">-</span>")
                                            End If
                                        End Code
                                    </td>
                                    <td>
                                        @Code
                                            If item.CompletedDate.HasValue Then
                                                WriteLiteral(item.CompletedDate.Value.ToString("dd/MM/yyyy"))
                                            Else
                                                WriteLiteral("<span class=""text-muted"">-</span>")
                                            End If
                                        End Code
                                    </td>
                                    <td>
                                        <div class="btn-group" role="group">
                                            <a href="@Url.Action("Details", "Tasks", New With {.id = item.Id})" class="btn btn-sm btn-info" title="Xem chi tiết">
                                                <i class="fas fa-eye"></i>
                                            </a>
                                            <button type="button" class="btn btn-sm btn-outline-primary btn-save-task" data-id="@item.Id" data-saved="@item.IsSaved.ToString().ToLower()" title="@(If(item.IsSaved, "Bỏ lưu", "Lưu câu trả lời"))">
                                                <i class="fas fa-bookmark @(If(item.IsSaved, "text-danger", ""))"></i>
                                            </button>
                                            <a href="@Url.Action("Edit", "Tasks", New With {.id = item.Id})" class="btn btn-sm btn-warning" title="Chỉnh sửa">
                                                <i class="fas fa-edit"></i>
                                            </a>
                                            <a href="@Url.Action("Delete", "Tasks", New With {.id = item.Id})" class="btn btn-sm btn-danger" title="Xóa" onclick="return confirm('Bạn có chắc chắn muốn xóa công việc này?');">
                                                <i class="fas fa-trash"></i>
                                            </a>
                                        </div>
                                    </td>
                                </tr>
                            Next
                        </tbody>
                    </table>
                </div>
            Else
                @<div class="text-center py-5">
                    <i class="fas fa-inbox fa-3x text-muted mb-3"></i>
                    <p class="text-muted">Chưa có công việc nào. Hãy tạo công việc mới!</p>
                    <a href="@Url.Action("Create", "Tasks")" class="btn btn-primary">
                        <i class="fas fa-plus me-2"></i>Tạo công việc đầu tiên
                    </a>
                </div>
            End If
        </div>
    </div>
</div>

@Section scripts
    <script>
        $(document).ready(function() {
            $('.btn-save-task').click(function() {
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
        });
    </script>
End Section