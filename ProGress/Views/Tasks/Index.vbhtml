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
                @<div class="table-responsive table-scroll-container">
                    <table class="table table-hover table-bordered">
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
                                <th>Ngày dự kiến hoàn thành</th>
                                <th>Ngày thực tế hoàn thành</th>
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
                                    <td>
                                        @Code
                                            Dim desc = If(item.Description, "")
                                            If desc IsNot Nothing AndAlso desc.Length > 50 Then
                                                desc = desc.Substring(0, 50) & "..."
                                            End If
                                            If Not String.IsNullOrEmpty(desc) Then
                                                WriteLiteral(desc)
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
                                                @<span class="status-badge status-paused">🔴 Tạm dừng</span>
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