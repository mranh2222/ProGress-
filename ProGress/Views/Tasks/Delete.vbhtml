@ModelType Task
@Code
    ViewData("Title") = "Xóa công việc"
End Code

<div class="container-fluid">
    <div class="row mb-4">
        <div class="col-12">
            <h1><i class="fas fa-trash me-2"></i>Xóa công việc</h1>
        </div>
    </div>

    <div class="row">
        <div class="col-lg-6 offset-lg-3">
            <div class="card">
                <div class="card-header bg-danger text-white">
                    <h5 class="mb-0">Xác nhận xóa</h5>
                </div>
                <div class="card-body">
                    <p class="alert alert-warning">
                        <i class="fas fa-exclamation-triangle me-2"></i>
                        Bạn có chắc chắn muốn xóa công việc này? Hành động này không thể hoàn tác.
                    </p>

                    <dl class="row">
                        <dt class="col-sm-4">Tag:</dt>
                        <dd class="col-sm-8">
                            @If Not String.IsNullOrEmpty(Model.Tag) Then
                                @<span class="badge bg-primary">@Model.Tag</span>
                            Else
                                @<span class="text-muted">Không có</span>
                            End If
                        </dd>

                        <dt class="col-sm-4">Mô tả:</dt>
                        <dd class="col-sm-8">
                            @If Not String.IsNullOrEmpty(Model.Description) Then
                                @Model.Description
                            Else
                                @<span class="text-muted">Không có mô tả</span>
                            End If
                        </dd>

                        <dt class="col-sm-4">Khách hàng:</dt>
                        <dd class="col-sm-8">@Model.CustomerName</dd>

                        <dt class="col-sm-4">Người phụ trách:</dt>
                        <dd class="col-sm-8">@Model.AssignedToName</dd>

                        <dt class="col-sm-4">Trạng thái:</dt>
                        <dd class="col-sm-8">
                            @Select Case Model.Status
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
                        </dd>
                    </dl>

                    @Using Html.BeginForm("Delete", "Tasks", FormMethod.Post)
                        @Html.AntiForgeryToken()
                        @Html.HiddenFor(Function(m) m.Id)
                        
                        @<div>
                            <div class="d-flex justify-content-between">
                            <a href="@Url.Action("Index", "Tasks")" class="btn btn-secondary">
                                <i class="fas fa-arrow-left me-2"></i>Hủy
                            </a>
                            <button type="submit" class="btn btn-danger">
                                <i class="fas fa-trash me-2"></i>Xác nhận xóa
                            </button>
                        </div>
                        </div>
                    End Using
                </div>
            </div>
        </div>
    </div>
</div>
