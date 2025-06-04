Imports System.IO

' Module for logging audit trails
Public Module AuditTrail
    Private ReadOnly AuditFile As String = "audit_trail.txt"

    ' Logs an action with details
    Public Sub LogAction(username As String, role As String, action As String, details As String)
        Dim logEntry As String = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {username} ({role}) - {action} - {details}"
        File.AppendAllText(AuditFile, logEntry & Environment.NewLine)
    End Sub

    ' Displays the audit trail in the console
    Public Sub ShowAuditTrails()
        Console.Clear()
        Console.WriteLine("=== Audit Trails ===")
        If File.Exists(AuditFile) Then
            Dim lines() As String = File.ReadAllLines(AuditFile)
            For Each line In lines
                Console.WriteLine(line)
            Next
        Else
            Console.WriteLine("No audit trail found.")
        End If
        Console.WriteLine("Press any key to return to the main menu...")
        Console.ReadKey()
    End Sub
End Module