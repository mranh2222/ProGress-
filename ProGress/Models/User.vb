Imports System
Imports System.ComponentModel.DataAnnotations

Public Class User
    Public Property Id As Integer
    
    <Required(ErrorMessage:="Vui lòng nhập tên đăng nhập")>
    <Display(Name:="Tên đăng nhập")>
    Public Property Username As String
    
    <Required(ErrorMessage:="Vui lòng nhập mật khẩu")>
    <DataType(DataType.Password)>
    <Display(Name:="Mật khẩu")>
    Public Property Password As String
    
    <Required(ErrorMessage:="Vui lòng nhập email")>
    <EmailAddress(ErrorMessage:="Email không hợp lệ")>
    Public Property Email As String
    
    <Display(Name:="Họ và tên")>
    Public Property FullName As String
    
    Public Property CreatedDate As DateTime
End Class





