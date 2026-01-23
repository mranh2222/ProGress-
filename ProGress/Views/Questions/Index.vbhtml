@ModelType IEnumerable(Of Task)
@Code
    ViewData("Title") = "Câu trả lời đã lưu"
    
    Dim hasSavedTasks As Boolean = False
    Dim taskList As List(Of Task) = New List(Of Task)
    
    If Model IsNot Nothing Then
        Try
            taskList = Model.ToList()
            hasSavedTasks = (taskList.Count > 0)
        Catch ex As Exception
            hasSavedTasks = False
            taskList = New List(Of Task)
        End Try
    End If
End Code

<div class="container-fluid py-3">
    <div class="d-flex justify-content-between align-items-center mb-3">
        <h4 class="mb-0" style="font-weight: 600; color: var(--dark-color);">
            <i class="fas fa-bookmark me-2 text-primary"></i>Câu trả lời đã lưu
        </h4>
        <div class="text-muted small">
            Hiển thị các công việc được đánh dấu lưu từ danh sách công việc
        </div>
    </div>

    @If TempData("SuccessMessage") IsNot Nothing Then
        @<div class="alert alert-success alert-dismissible fade show" role="alert">
            <i class="fas fa-check-circle me-2"></i>@TempData("SuccessMessage")
            <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
        </div>
    End If

    @If hasSavedTasks Then
        @<div class="card shadow-sm border-0">
            <div class="card-body p-0">
                <div class="table-responsive">
                    <table class="table table-hover mb-0 saved-tasks-table">
                        <thead>
                            <tr>
                                <th style="width: 5%;">STT</th>
                                <th style="width: 15%;">Khách hàng</th>
                                <th style="width: 10%;">Phần mềm</th>
                                <th style="width: 30%;">Nội dung công việc</th>
                                <th style="width: 30%;">Giải pháp / Câu trả lời</th>
                                <th style="width: 10%;">Thao tác</th>
                            </tr>
                        </thead>
                        <tbody>
                            @For i = 0 To taskList.Count - 1
                                Dim taskItem = taskList(i)
                                @<tr id="row-@taskItem.Id">
                                    <td class="text-center">@(i + 1)</td>
                                    <td>
                                        <div class="fw-semibold">@taskItem.CustomerName</div>
                                        <div class="text-muted small">@taskItem.CreatedDate.ToString("dd/MM/yyyy")</div>
                                    </td>
                                    <td>
                                        <span class="badge bg-light text-dark border">@taskItem.SoftwareName</span>
                                    </td>
                                    <td>
                                        <div class="content-truncate" title="@taskItem.Description">
                                            @If Not String.IsNullOrEmpty(taskItem.Description) Then
                                                @Html.Raw(taskItem.Description)
                                            Else
                                                @<span class="text-muted">-</span>
                                            End If
                                        </div>
                                    </td>
                                    <td>
                                        <div class="content-truncate" title="@(If(Not String.IsNullOrEmpty(taskItem.ResponseToCustomer), taskItem.ResponseToCustomer, taskItem.Solution))">
                                            @If Not String.IsNullOrEmpty(taskItem.ResponseToCustomer) Then
                                                @Html.Raw(taskItem.ResponseToCustomer)
                                            ElseIf Not String.IsNullOrEmpty(taskItem.Solution) Then
                                                @Html.Raw(taskItem.Solution)
                                            Else
                                                @<span class="text-muted italic small">Chưa có giải pháp</span>
                                            End If
                                        </div>
                                    </td>
                                    <td class="text-center">
                                        <div class="btn-group btn-group-sm">
                                            <a href="@Url.Action("Details", "Tasks", New With {.id = taskItem.Id})" class="btn btn-outline-dark btn-action-dark" title="Xem chi tiết">
                                                <i class="fas fa-eye"></i>
                                            </a>
                                            <button type="button" class="btn btn-outline-dark btn-action-dark btn-unsave" data-id="@taskItem.Id" title="Bỏ lưu">
                                                <i class="fas fa-bookmark"></i>
                                            </button>
                                        </div>
                                    </td>
                                </tr>
                            Next
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    Else
        @<div class="card shadow-sm border-0">
            <div class="card-body text-center py-5">
                <div class="mb-3">
                    <i class="fas fa-bookmark fa-3x text-muted opacity-25"></i>
                </div>
                <h5 class="text-muted">Chưa có câu trả lời nào được lưu</h5>
                <p class="text-muted small mb-4">Bạn có thể đánh dấu lưu các công việc quan trọng trong danh sách công việc để xem lại tại đây.</p>
                <a href="@Url.Action("Index", "Tasks")" class="btn btn-primary">
                    <i class="fas fa-list me-1"></i>Đến danh sách công việc
                </a>
            </div>
        </div>
    End If
</div>

<style>
    .saved-tasks-table thead th {
        background: #f8fafc;
        color: #475569;
        font-weight: 600;
        text-transform: uppercase;
        font-size: 0.75rem;
        padding: 0.75rem 1rem;
        border-bottom: 2px solid #e2e8f0;
    }
    
    .saved-tasks-table tbody td {
        padding: 0.75rem 1rem;
        vertical-align: middle;
        border-bottom: 1px solid #f1f5f9;
        font-size: 0.875rem;
    }
    
    .content-truncate {
        max-height: 4.5em;
        overflow: hidden;
        display: -webkit-box;
        -webkit-line-clamp: 3;
        -webkit-box-orient: vertical;
        line-height: 1.5;
    }

    /* nút action dùng style chung .btn-action-dark (định nghĩa ở _Layout) */
</style>

@Section scripts
    <script>
        $(document).ready(function() {
            $('.btn-unsave').click(function() {
                var btn = $(this);
                var id = btn.data('id');
                
                if (confirm('Bạn có chắc chắn muốn bỏ lưu công việc này khỏi danh sách câu trả lời đã lưu?')) {
                    $.ajax({
                        url: '@Url.Action("ToggleSaved", "Tasks")',
                        type: 'POST',
                        data: { id: id, isSaved: false },
                        success: function(response) {
                            if (response.success) {
                                $('#row-' + id).fadeOut(400, function() {
                                    $(this).remove();
                                    if ($('.saved-tasks-table tbody tr').length === 0) {
                                        location.reload();
                                    }
                                });
                            } else {
                                alert('Có lỗi xảy ra khi thực hiện thao tác.');
                            }
                        }
                    });
                }
            });
        });
    </script>
End Section
