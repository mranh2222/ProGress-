Imports System.Web.Mvc
Imports System.Linq
Imports System.Collections.Generic
Imports ThreadingTask = System.Threading.Tasks

Public Class QuestionsController
    Inherits Controller

    Private ReadOnly _dataService As SqlDataService
    Private ReadOnly _storageService As FileStorageService

    Public Sub New()
        _dataService = New SqlDataService()
        _storageService = New FileStorageService()
    End Sub

    ' GET: Questions/Index
    Async Function Index() As ThreadingTask.Task(Of ActionResult)
        Dim savedTasks = Await _dataService.GetSavedTasksAsync()
        Return View(savedTasks)
    End Function

    ' GET: Questions/Create
    Async Function Create() As ThreadingTask.Task(Of ActionResult)
        Dim technicians = Await _dataService.GetAllTechniciansAsync()
        ViewBag.Technicians = technicians.Where(Function(t) t.IsActive).ToList()
        Return View(New Question())
    End Function

    ' POST: Questions/Create
    <HttpPost>
    <ValidateAntiForgeryToken>
    <ValidateInput(False)>
    Async Function Create(question As Question, technicianId As String, uploadedFiles As HttpPostedFileBase(), uploadedImages As HttpPostedFileBase()) As ThreadingTask.Task(Of ActionResult)
        Try
            If question Is Nothing Then question = New Question()
            
            Dim techIdFromForm = Request.Unvalidated.Form("technicianId")
            Dim questionTextFromForm = Request.Unvalidated.Form("Question")
            
            If String.IsNullOrEmpty(techIdFromForm) OrElse String.IsNullOrEmpty(questionTextFromForm) Then
                ModelState.AddModelError("", "Vui lòng điền đầy đủ thông tin.")
                ViewBag.Technicians = (Await _dataService.GetAllTechniciansAsync()).Where(Function(t) t.IsActive).ToList()
                Return View(question)
            End If

            question.TechnicianId = techIdFromForm
            question.Question = questionTextFromForm
            
            If uploadedFiles IsNot Nothing Then
                For Each postedFile As HttpPostedFileBase In uploadedFiles
                    If postedFile IsNot Nothing AndAlso postedFile.ContentLength > 0 Then
                        Dim url = Await _storageService.UploadFileAsync(postedFile, "uploads")
                        If Not String.IsNullOrEmpty(url) Then question.Attachments.Add(url)
                    End If
                Next
            End If

            If uploadedImages IsNot Nothing Then
                For Each postedImage As HttpPostedFileBase In uploadedImages
                    If postedImage IsNot Nothing AndAlso postedImage.ContentLength > 0 Then
                        Dim url = Await _storageService.UploadFileAsync(postedImage, "images")
                        If Not String.IsNullOrEmpty(url) Then question.Images.Add(url)
                    End If
                Next
            End If

            Await _dataService.CreateQuestionAsync(question)
            Return RedirectToAction("Index")
        Catch ex As Exception
            Return View(question)
        End Try
    End Function

    ' GET: Questions/Details
    Async Function Details(id As String) As ThreadingTask.Task(Of ActionResult)
        Dim question = Await _dataService.GetQuestionByIdAsync(id)
        If question Is Nothing Then Return HttpNotFound()
        Return View(question)
    End Function

    ' POST: Questions/Reply
    <HttpPost>
    <ValidateAntiForgeryToken>
    Async Function Reply(id As String, answer As String) As ThreadingTask.Task(Of JsonResult)
        Try
            Dim question = Await _dataService.GetQuestionByIdAsync(id)
            If question Is Nothing Then Return Json(New With {.success = False})

            question.Answer = answer
            question.Status = QuestionStatus.Answered
            question.AnsweredDate = DateTime.Now

            Dim success = Await _dataService.UpdateQuestionAsync(question)
            Return Json(New With {.success = success})
        Catch ex As Exception
            Return Json(New With {.success = False})
        End Try
    End Function

    ' POST: Questions/Delete
    <HttpPost>
    <ValidateAntiForgeryToken>
    Async Function Delete(id As String) As ThreadingTask.Task(Of ActionResult)
        Await _dataService.DeleteQuestionAsync(id)
        Return RedirectToAction("Index")
    End Function
End Class
