Imports System.Data
Imports System.IO

' Handles user authentication and registration
Public Module UserAuth
    Private ReadOnly FilePath As String = "users.csv"
    Public Property Role As Integer
    Public Property Username As String

    Public Sub EnsureAdminUser()
        Dim adminExists As Boolean = False
        If File.Exists(FilePath) Then
            adminExists = File.ReadAllLines(FilePath).Any(Function(line) line.StartsWith("admin,"))
        End If
        If Not adminExists Then
            Using writer As New StreamWriter(FilePath, True)
                writer.WriteLine("rRhesu,Initryuk,admin")
            End Using
        End If
    End Sub

    Public Function Authenticate(username As String, password As String) As User
        If Not File.Exists(FilePath) Then Return Nothing
        For Each line In File.ReadAllLines(FilePath)
            Dim parts = line.Split(","c)
            If parts.Length = 3 AndAlso parts(0) = username AndAlso parts(1) = password Then
                Return New User With {.Username = parts(0), .Password = parts(1), .Role = parts(2)}
            End If
        Next
        Return Nothing
    End Function

    Public Function RegisterUser(username As String, password As String, role As String) As Boolean
        If File.Exists(FilePath) AndAlso File.ReadAllLines(FilePath).Any(Function(line) line.StartsWith(username & ",")) Then
            Return False ' User exists
        End If
        Using writer As New StreamWriter(FilePath, True)
            writer.WriteLine($"{username},{password},{role}")
        End Using
        Return True
    End Function
End Module
