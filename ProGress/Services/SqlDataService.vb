Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Collections.Generic
Imports ThreadingTask = System.Threading.Tasks

Public Class SqlDataService
    Private ReadOnly _connectionString As String
    
    Public Sub New()
        Dim configConnectionString = ConfigurationManager.ConnectionStrings("DefaultConnection")
        If configConnectionString IsNot Nothing AndAlso Not String.IsNullOrEmpty(configConnectionString.ConnectionString) Then
            _connectionString = configConnectionString.ConnectionString
        Else
            ' Fallback cho local development
            _connectionString = "Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ProGressDB;Integrated Security=True"
        End If
    End Sub

    ' ========== TECHNICIANS ==========
    Public Async Function GetAllTechniciansAsync() As ThreadingTask.Task(Of List(Of Technician))
        Dim list As New List(Of Technician)
        Using conn As New SqlConnection(_connectionString)
            Dim cmd As New SqlCommand("SELECT * FROM Technicians WHERE IsActive = 1", conn)
            Await conn.OpenAsync()
            Using reader = Await cmd.ExecuteReaderAsync()
                While Await reader.ReadAsync()
                    list.Add(New Technician With {
                        .Id = reader("Id").ToString(),
                        .Name = reader("Name").ToString(),
                        .Email = If(reader("Email") Is DBNull.Value, "", reader("Email").ToString()),
                        .Phone = If(reader("Phone") Is DBNull.Value, "", reader("Phone").ToString()),
                        .IsActive = Convert.ToBoolean(reader("IsActive"))
                    })
                End While
            End Using
        End Using
        Return list
    End Function

    Public Async Function CreateTechnicianAsync(tech As Technician) As ThreadingTask.Task(Of Boolean)
        Using conn As New SqlConnection(_connectionString)
            Dim cmd As New SqlCommand("INSERT INTO Technicians (Id, Name, Email, Phone, IsActive) VALUES (@Id, @Name, @Email, @Phone, @IsActive)", conn)
            cmd.Parameters.AddWithValue("@Id", tech.Id)
            cmd.Parameters.AddWithValue("@Name", tech.Name)
            cmd.Parameters.AddWithValue("@Email", If(tech.Email, DBNull.Value))
            cmd.Parameters.AddWithValue("@Phone", If(tech.Phone, DBNull.Value))
            cmd.Parameters.AddWithValue("@IsActive", tech.IsActive)
            Await conn.OpenAsync()
            Return Await cmd.ExecuteNonQueryAsync() > 0
        End Using
    End Function

    ' ========== SALE MANAGERS ==========
    Public Async Function GetAllSaleManagersAsync() As ThreadingTask.Task(Of List(Of SaleManager))
        Dim list As New List(Of SaleManager)
        Using conn As New SqlConnection(_connectionString)
            Dim cmd As New SqlCommand("SELECT * FROM SaleManagers WHERE IsActive = 1", conn)
            Await conn.OpenAsync()
            Using reader = Await cmd.ExecuteReaderAsync()
                While Await reader.ReadAsync()
                    list.Add(New SaleManager With {
                        .Id = reader("Id").ToString(),
                        .Name = reader("Name").ToString(),
                        .IsActive = Convert.ToBoolean(reader("IsActive"))
                    })
                End While
            End Using
        End Using
        Return list
    End Function

    ' ========== SOFTWARE ==========
    Public Async Function GetAllSoftwareAsync() As ThreadingTask.Task(Of List(Of Software))
        Dim list As New List(Of Software)
        Using conn As New SqlConnection(_connectionString)
            Dim cmd As New SqlCommand("SELECT * FROM Software WHERE IsActive = 1", conn)
            Await conn.OpenAsync()
            Using reader = Await cmd.ExecuteReaderAsync()
                While Await reader.ReadAsync()
                    list.Add(New Software With {
                        .Id = reader("Id").ToString(),
                        .Name = reader("Name").ToString(),
                        .IsActive = Convert.ToBoolean(reader("IsActive"))
                    })
                End While
            End Using
        End Using
        Return list
    End Function

    ' ========== TASKS ==========
    Public Async Function GetAllTasksAsync() As ThreadingTask.Task(Of List(Of Task))
        Dim list As New List(Of Task)
        Using conn As New SqlConnection(_connectionString)
            Dim query = "SELECT t.*, tech.Name as TechName, s.Name as SoftName, sm.Name as SaleName " &
                        "FROM Tasks t " &
                        "LEFT JOIN Technicians tech ON t.AssignedTo = tech.Id " &
                        "LEFT JOIN Software s ON t.SoftwareId = s.Id " &
                        "LEFT JOIN SaleManagers sm ON t.SaleManagerId = sm.Id " &
                        "ORDER BY t.CreatedDate DESC"
            
            Dim cmd As New SqlCommand(query, conn)
            Await conn.OpenAsync()
            Using reader = Await cmd.ExecuteReaderAsync()
                While Await reader.ReadAsync()
                    Dim taskItem = MapTaskFromReader(reader)
                    taskItem.AssignedToName = If(reader("TechName") Is DBNull.Value, "", reader("TechName").ToString())
                    taskItem.SoftwareName = If(reader("SoftName") Is DBNull.Value, "", reader("SoftName").ToString())
                    taskItem.SaleManagerName = If(reader("SaleName") Is DBNull.Value, "", reader("SaleName").ToString())
                    list.Add(taskItem)
                End While
            End Using
            
            For Each t In list
                t.Attachments = Await GetTaskLinks(t.Id, "TaskAttachments")
                t.Images = Await GetTaskLinks(t.Id, "TaskImages")
                t.ResponseAttachments = Await GetTaskLinks(t.Id, "TaskResponseAttachments")
                t.ResponseImages = Await GetTaskLinks(t.Id, "TaskResponseImages")
            Next
        End Using
        Return list
    End Function

    Public Async Function GetSavedTasksAsync() As ThreadingTask.Task(Of List(Of Task))
        Dim list As New List(Of Task)
        Using conn As New SqlConnection(_connectionString)
            Dim query = "SELECT t.*, tech.Name as TechName, s.Name as SoftName, sm.Name as SaleName " &
                        "FROM Tasks t " &
                        "LEFT JOIN Technicians tech ON t.AssignedTo = tech.Id " &
                        "LEFT JOIN Software s ON t.SoftwareId = s.Id " &
                        "LEFT JOIN SaleManagers sm ON t.SaleManagerId = sm.Id " &
                        "WHERE t.IsSaved = 1 " &
                        "ORDER BY t.CreatedDate DESC"
            
            Dim cmd As New SqlCommand(query, conn)
            Await conn.OpenAsync()
            Using reader = Await cmd.ExecuteReaderAsync()
                While Await reader.ReadAsync()
                    Dim taskItem = MapTaskFromReader(reader)
                    taskItem.AssignedToName = If(reader("TechName") Is DBNull.Value, "", reader("TechName").ToString())
                    taskItem.SoftwareName = If(reader("SoftName") Is DBNull.Value, "", reader("SoftName").ToString())
                    taskItem.SaleManagerName = If(reader("SaleName") Is DBNull.Value, "", reader("SaleName").ToString())
                    list.Add(taskItem)
                End While
            End Using
            
            For Each t In list
                t.Attachments = Await GetTaskLinks(t.Id, "TaskAttachments")
                t.Images = Await GetTaskLinks(t.Id, "TaskImages")
                t.ResponseAttachments = Await GetTaskLinks(t.Id, "TaskResponseAttachments")
                t.ResponseImages = Await GetTaskLinks(t.Id, "TaskResponseImages")
            Next
        End Using
        Return list
    End Function

    Public Async Function ToggleTaskSavedAsync(id As String, isSaved As Boolean) As ThreadingTask.Task(Of Boolean)
        Using conn As New SqlConnection(_connectionString)
            Dim cmd As New SqlCommand("UPDATE Tasks SET IsSaved = @IsSaved WHERE Id = @Id", conn)
            cmd.Parameters.AddWithValue("@Id", id)
            cmd.Parameters.AddWithValue("@IsSaved", If(isSaved, 1, 0))
            Await conn.OpenAsync()
            Return Await cmd.ExecuteNonQueryAsync() > 0
        End Using
    End Function

    Public Async Function GetTaskByIdAsync(id As String) As ThreadingTask.Task(Of Task)
        Using conn As New SqlConnection(_connectionString)
            Dim query = "SELECT t.*, tech.Name as TechName, s.Name as SoftName, sm.Name as SaleName " &
                        "FROM Tasks t " &
                        "LEFT JOIN Technicians tech ON t.AssignedTo = tech.Id " &
                        "LEFT JOIN Software s ON t.SoftwareId = s.Id " &
                        "LEFT JOIN SaleManagers sm ON t.SaleManagerId = sm.Id " &
                        "WHERE t.Id = @Id"
            
            Dim cmd As New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@Id", id)
            Await conn.OpenAsync()
            Using reader = Await cmd.ExecuteReaderAsync()
                If Await reader.ReadAsync() Then
                    Dim taskItem = MapTaskFromReader(reader)
                    taskItem.AssignedToName = If(reader("TechName") Is DBNull.Value, "", reader("TechName").ToString())
                    taskItem.SoftwareName = If(reader("SoftName") Is DBNull.Value, "", reader("SoftName").ToString())
                    taskItem.SaleManagerName = If(reader("SaleName") Is DBNull.Value, "", reader("SaleName").ToString())
                    
                    taskItem.Attachments = Await GetTaskLinks(taskItem.Id, "TaskAttachments")
                    taskItem.Images = Await GetTaskLinks(taskItem.Id, "TaskImages")
                    taskItem.ResponseAttachments = Await GetTaskLinks(taskItem.Id, "TaskResponseAttachments")
                    taskItem.ResponseImages = Await GetTaskLinks(taskItem.Id, "TaskResponseImages")
                    taskItem.History = Await GetTaskHistory(taskItem.Id)
                    
                    Return taskItem
                End If
            End Using
        End Using
        Return Nothing
    End Function

    Public Async Function CreateTaskAsync(task As Task) As ThreadingTask.Task(Of Boolean)
        Using conn As New SqlConnection(_connectionString)
            Dim query = "INSERT INTO Tasks (Id, Tag, Description, CustomerName, FileReceivedDate, SupportPlatform, " &
                        "SaleManagerId, SoftwareId, AssignedTo, Status, ExpectedCompletionDate, CompletedDate, " &
                        "CreatedDate, Solution, ResponseToCustomer) VALUES (@Id, @Tag, @Description, @CustomerName, " &
                        "@FileReceivedDate, @SupportPlatform, @SaleManagerId, @SoftwareId, @AssignedTo, @Status, " &
                        "@ExpectedCompletionDate, @CompletedDate, @CreatedDate, @Solution, @ResponseToCustomer)"
            
            Dim cmd As New SqlCommand(query, conn)
            AddCommonTaskParameters(cmd, task)
            
            Await conn.OpenAsync()
            Dim rows = Await cmd.ExecuteNonQueryAsync()
            
            Await SaveTaskLinks(task.Id, task.Attachments, "TaskAttachments")
            Await SaveTaskLinks(task.Id, task.Images, "TaskImages")
            Await SaveTaskLinks(task.Id, task.ResponseAttachments, "TaskResponseAttachments")
            Await SaveTaskLinks(task.Id, task.ResponseImages, "TaskResponseImages")
            
            Return rows > 0
        End Using
    End Function

    Public Async Function UpdateTaskAsync(task As Task) As ThreadingTask.Task(Of Boolean)
        Using conn As New SqlConnection(_connectionString)
            Dim query = "UPDATE Tasks SET Tag=@Tag, Description=@Description, CustomerName=@CustomerName, " &
                        "FileReceivedDate=@FileReceivedDate, SupportPlatform=@SupportPlatform, SaleManagerId=@SaleManagerId, " &
                        "SoftwareId=@SoftwareId, AssignedTo=@AssignedTo, Status=@Status, " &
                        "ExpectedCompletionDate=@ExpectedCompletionDate, CompletedDate=@CompletedDate, UpdatedDate=GETDATE(), " &
                        "Solution=@Solution, ResponseToCustomer=@ResponseToCustomer WHERE Id=@Id"
            
            Dim cmd As New SqlCommand(query, conn)
            AddCommonTaskParameters(cmd, task)
            
            Await conn.OpenAsync()
            Dim rows = Await cmd.ExecuteNonQueryAsync()
            
            Await SaveTaskLinks(task.Id, task.Attachments, "TaskAttachments")
            Await SaveTaskLinks(task.Id, task.Images, "TaskImages")
            Await SaveTaskLinks(task.Id, task.ResponseAttachments, "TaskResponseAttachments")
            Await SaveTaskLinks(task.Id, task.ResponseImages, "TaskResponseImages")
            
            Return rows > 0
        End Using
    End Function

    Public Async Function DeleteTaskAsync(id As String) As ThreadingTask.Task(Of Boolean)
        Using conn As New SqlConnection(_connectionString)
            Dim cmd As New SqlCommand("DELETE FROM Tasks WHERE Id=@Id", conn)
            cmd.Parameters.AddWithValue("@Id", id)
            Await conn.OpenAsync()
            Return Await cmd.ExecuteNonQueryAsync() > 0
        End Using
    End Function

    ' ========== QUESTIONS ==========
    Public Async Function GetAllQuestionsAsync() As ThreadingTask.Task(Of List(Of Question))
        Dim list As New List(Of Question)
        Using conn As New SqlConnection(_connectionString)
            Dim query = "SELECT q.*, tech.Name as TechName FROM Questions q " &
                        "LEFT JOIN Technicians tech ON q.TechnicianId = tech.Id " &
                        "ORDER BY q.CreatedDate DESC"
            Dim cmd As New SqlCommand(query, conn)
            Await conn.OpenAsync()
            Using reader = Await cmd.ExecuteReaderAsync()
                While Await reader.ReadAsync()
                    Dim q = New Question With {
                        .Id = reader("Id").ToString(),
                        .TechnicianId = reader("TechnicianId").ToString(),
                        .TechnicianName = If(reader("TechName") Is DBNull.Value, "", reader("TechName").ToString()),
                        .Question = reader("Question").ToString(),
                        .Answer = If(reader("Answer") Is DBNull.Value, "", reader("Answer").ToString()),
                        .Status = CType(reader("Status"), QuestionStatus),
                        .CreatedDate = Convert.ToDateTime(reader("CreatedDate")),
                        .AnsweredDate = If(reader("AnsweredDate") Is DBNull.Value, Nothing, CType(reader("AnsweredDate"), DateTime?))
                    }
                    list.Add(q)
                End While
            End Using
            
            For Each q In list
                q.Attachments = Await GetTaskLinks(q.Id, "QuestionAttachments", "QuestionId")
                q.Images = Await GetTaskLinks(q.Id, "QuestionImages", "QuestionId")
            Next
        End Using
        Return list
    End Function

    Public Async Function GetQuestionByIdAsync(id As String) As ThreadingTask.Task(Of Question)
        Using conn As New SqlConnection(_connectionString)
            Dim query = "SELECT q.*, tech.Name as TechName FROM Questions q " &
                        "LEFT JOIN Technicians tech ON q.TechnicianId = tech.Id WHERE q.Id=@Id"
            Dim cmd As New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@Id", id)
            Await conn.OpenAsync()
            Using reader = Await cmd.ExecuteReaderAsync()
                If Await reader.ReadAsync() Then
                    Dim q = New Question With {
                        .Id = reader("Id").ToString(),
                        .TechnicianId = reader("TechnicianId").ToString(),
                        .TechnicianName = If(reader("TechName") Is DBNull.Value, "", reader("TechName").ToString()),
                        .Question = reader("Question").ToString(),
                        .Answer = If(reader("Answer") Is DBNull.Value, "", reader("Answer").ToString()),
                        .Status = CType(reader("Status"), QuestionStatus),
                        .CreatedDate = Convert.ToDateTime(reader("CreatedDate")),
                        .AnsweredDate = If(reader("AnsweredDate") Is DBNull.Value, Nothing, CType(reader("AnsweredDate"), DateTime?))
                    }
                    q.Attachments = Await GetTaskLinks(q.Id, "QuestionAttachments", "QuestionId")
                    q.Images = Await GetTaskLinks(q.Id, "QuestionImages", "QuestionId")
                    Return q
                End If
            End Using
        End Using
        Return Nothing
    End Function

    Public Async Function CreateQuestionAsync(q As Question) As ThreadingTask.Task(Of Boolean)
        Using conn As New SqlConnection(_connectionString)
            Dim query = "INSERT INTO Questions (Id, TechnicianId, Question, Answer, Status, CreatedDate) " &
                        "VALUES (@Id, @TechnicianId, @Question, @Answer, @Status, @CreatedDate)"
            Dim cmd As New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@Id", q.Id)
            cmd.Parameters.AddWithValue("@TechnicianId", q.TechnicianId)
            cmd.Parameters.AddWithValue("@Question", q.Question)
            cmd.Parameters.AddWithValue("@Answer", If(q.Answer, DBNull.Value))
            cmd.Parameters.AddWithValue("@Status", CInt(q.Status))
            cmd.Parameters.AddWithValue("@CreatedDate", q.CreatedDate)
            
            Await conn.OpenAsync()
            Dim rows = Await cmd.ExecuteNonQueryAsync()
            Await SaveTaskLinks(q.Id, q.Attachments, "QuestionAttachments", "QuestionId")
            Await SaveTaskLinks(q.Id, q.Images, "QuestionImages", "QuestionId")
            Return rows > 0
        End Using
    End Function

    Public Async Function UpdateQuestionAsync(q As Question) As ThreadingTask.Task(Of Boolean)
        Using conn As New SqlConnection(_connectionString)
            Dim query = "UPDATE Questions SET Answer=@Answer, Status=@Status, AnsweredDate=@AnsweredDate WHERE Id=@Id"
            Dim cmd As New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@Id", q.Id)
            cmd.Parameters.AddWithValue("@Answer", q.Answer)
            cmd.Parameters.AddWithValue("@Status", CInt(q.Status))
            cmd.Parameters.AddWithValue("@AnsweredDate", If(q.AnsweredDate, DateTime.Now))
            
            Await conn.OpenAsync()
            Return Await cmd.ExecuteNonQueryAsync() > 0
        End Using
    End Function

    Public Async Function DeleteQuestionAsync(id As String) As ThreadingTask.Task(Of Boolean)
        Using conn As New SqlConnection(_connectionString)
            Dim cmd As New SqlCommand("DELETE FROM Questions WHERE Id=@Id", conn)
            cmd.Parameters.AddWithValue("@Id", id)
            Await conn.OpenAsync()
            Return Await cmd.ExecuteNonQueryAsync() > 0
        End Using
    End Function

    ' ========== HELPERS ==========
    Private Function MapTaskFromReader(reader As SqlDataReader) As Task
        Return New Task With {
            .Id = reader("Id").ToString(),
            .Tag = If(reader("Tag") Is DBNull.Value, "", reader("Tag").ToString()),
            .Description = If(reader("Description") Is DBNull.Value, "", reader("Description").ToString()),
            .CustomerName = If(reader("CustomerName") Is DBNull.Value, "", reader("CustomerName").ToString()),
            .FileReceivedDate = If(reader("FileReceivedDate") Is DBNull.Value, Nothing, CType(reader("FileReceivedDate"), DateTime?)),
            .SupportPlatform = CType(reader("SupportPlatform"), SupportPlatform),
            .SaleManagerId = If(reader("SaleManagerId") Is DBNull.Value, "", reader("SaleManagerId").ToString()),
            .SoftwareId = If(reader("SoftwareId") Is DBNull.Value, "", reader("SoftwareId").ToString()),
            .AssignedTo = If(reader("AssignedTo") Is DBNull.Value, "", reader("AssignedTo").ToString()),
            .Status = CType(reader("Status"), TaskStatus),
            .ExpectedCompletionDate = If(reader("ExpectedCompletionDate") Is DBNull.Value, Nothing, CType(reader("ExpectedCompletionDate"), DateTime?)),
            .CompletedDate = If(reader("CompletedDate") Is DBNull.Value, Nothing, CType(reader("CompletedDate"), DateTime?)),
            .CreatedDate = Convert.ToDateTime(reader("CreatedDate")),
            .UpdatedDate = If(reader("UpdatedDate") Is DBNull.Value, Nothing, CType(reader("UpdatedDate"), DateTime?)),
            .Solution = If(reader("Solution") Is DBNull.Value, "", reader("Solution").ToString()),
            .ResponseToCustomer = If(reader("ResponseToCustomer") Is DBNull.Value, "", reader("ResponseToCustomer").ToString()),
            .IsSaved = If(reader("IsSaved") Is DBNull.Value, False, Convert.ToBoolean(reader("IsSaved")))
        }
    End Function

    Private Sub AddCommonTaskParameters(cmd As SqlCommand, task As Task)
        cmd.Parameters.AddWithValue("@Id", task.Id)
        cmd.Parameters.AddWithValue("@Tag", If(task.Tag, DBNull.Value))
        cmd.Parameters.AddWithValue("@Description", If(task.Description, DBNull.Value))
        cmd.Parameters.AddWithValue("@CustomerName", If(task.CustomerName, DBNull.Value))
        cmd.Parameters.AddWithValue("@FileReceivedDate", If(task.FileReceivedDate, DBNull.Value))
        cmd.Parameters.AddWithValue("@SupportPlatform", CInt(task.SupportPlatform))
        cmd.Parameters.AddWithValue("@SaleManagerId", If(task.SaleManagerId, DBNull.Value))
        cmd.Parameters.AddWithValue("@SoftwareId", If(task.SoftwareId, DBNull.Value))
        cmd.Parameters.AddWithValue("@AssignedTo", If(task.AssignedTo, DBNull.Value))
        cmd.Parameters.AddWithValue("@Status", CInt(task.Status))
        cmd.Parameters.AddWithValue("@ExpectedCompletionDate", If(task.ExpectedCompletionDate, DBNull.Value))
        cmd.Parameters.AddWithValue("@CompletedDate", If(task.CompletedDate, DBNull.Value))
        cmd.Parameters.AddWithValue("@CreatedDate", task.CreatedDate)
        cmd.Parameters.AddWithValue("@Solution", If(task.Solution, DBNull.Value))
        cmd.Parameters.AddWithValue("@ResponseToCustomer", If(task.ResponseToCustomer, DBNull.Value))
        cmd.Parameters.AddWithValue("@IsSaved", If(task.IsSaved, 1, 0))
    End Sub

    Private Async Function GetTaskLinks(id As String, tableName As String, Optional fkName As String = "TaskId") As ThreadingTask.Task(Of List(Of String))
        Dim links As New List(Of String)
        Using conn As New SqlConnection(_connectionString)
            Dim cmd As New SqlCommand($"SELECT Url FROM {tableName} WHERE {fkName} = @Id", conn)
            cmd.Parameters.AddWithValue("@Id", id)
            Await conn.OpenAsync()
            Using reader = Await cmd.ExecuteReaderAsync()
                While Await reader.ReadAsync()
                    links.Add(reader("Url").ToString())
                End While
            End Using
        End Using
        Return links
    End Function

    Private Async Function SaveTaskLinks(id As String, links As List(Of String), tableName As String, Optional fkName As String = "TaskId") As ThreadingTask.Task
        If links Is Nothing Then Return
        Using conn As New SqlConnection(_connectionString)
            Await conn.OpenAsync()
            Dim delCmd As New SqlCommand($"DELETE FROM {tableName} WHERE {fkName} = @Id", conn)
            delCmd.Parameters.AddWithValue("@Id", id)
            Await delCmd.ExecuteNonQueryAsync()
            
            For Each url In links
                Dim insCmd As New SqlCommand($"INSERT INTO {tableName} ({fkName}, Url) VALUES (@Id, @Url)", conn)
                insCmd.Parameters.AddWithValue("@Id", id)
                insCmd.Parameters.AddWithValue("@Url", url)
                Await insCmd.ExecuteNonQueryAsync()
            Next
        End Using
    End Function

    Private Async Function GetTaskHistory(taskId As String) As ThreadingTask.Task(Of List(Of TaskHistory))
        Dim list As New List(Of TaskHistory)
        Using conn As New SqlConnection(_connectionString)
            Dim cmd As New SqlCommand("SELECT * FROM TaskHistory WHERE TaskId = @TaskId ORDER BY ChangedDate DESC", conn)
            cmd.Parameters.AddWithValue("@TaskId", taskId)
            Await conn.OpenAsync()
            Using reader = Await cmd.ExecuteReaderAsync()
                While Await reader.ReadAsync()
                    list.Add(New TaskHistory With {
                        .Id = reader("Id").ToString(),
                        .TaskId = reader("TaskId").ToString(),
                        .Action = reader("Action").ToString(),
                        .Description = reader("Description").ToString(),
                        .ChangedBy = reader("ChangedBy").ToString(),
                        .ChangedByName = reader("ChangedByName").ToString(),
                        .ChangedDate = Convert.ToDateTime(reader("ChangedDate")),
                        .OldValue = If(reader("OldValue") Is DBNull.Value, "", reader("OldValue").ToString()),
                        .NewValue = If(reader("NewValue") Is DBNull.Value, "", reader("NewValue").ToString())
                    })
                End While
            End Using
        End Using
        Return list
    End Function
End Class
