Imports System.IO

Module InventoryManagementSystem

    ' Inventory item class
    Public Class InventoryItem
        Public Property Id As Integer
        Public Property Name As String
        Public Property Quantity As Integer
        Public Property Price As Decimal
    End Class

    ' The inventory list
    Dim Inventory As New List(Of InventoryItem)
    Dim NextId As Integer = 1
    Private ReadOnly InventoryFilePath As String = "inventory.csv"

    ' Store the currently logged-in user globally
    Public LoggedInUsername As String = ""
    Public LoggedInRole As String = ""

    Sub Main()
        UserAuth.EnsureAdminUser()
        LoadInventory()

        ' Login loop
        Dim currentUser As User = Nothing
        Do
            Console.Clear()
            Console.WriteLine("=== Login ===")
            Console.Write("Username: ")
            Dim username = Console.ReadLine()
            Dim password = ReadPassword("Password: ")

            currentUser = UserAuth.Authenticate(username, password)
            If currentUser Is Nothing Then
                Console.WriteLine("Invalid credentials. Press any key to try again...")
                Console.ReadKey()
            End If
        Loop While currentUser Is Nothing

        ' Set global variables for use in logging
        LoggedInUsername = currentUser.Username
        LoggedInRole = currentUser.Role

        ' Log the login event
        AuditTrail.LogAction(LoggedInUsername, LoggedInRole, "Login", "User logged in.")

        ' Welcome message
        Console.WriteLine($"Welcome, {LoggedInUsername}! Role: {LoggedInRole}")
        Console.WriteLine("Press any key to continue...")
        Console.ReadKey()

        Dim running As Boolean = True

        While running
            Console.Clear()
            Console.WriteLine("=== Simple Inventory Management System ===")
            Console.WriteLine("1. Add Item")
            Console.WriteLine("2. View Inventory")
            Console.WriteLine("3. Update Item")
            Console.WriteLine("4. Delete Item")
            Console.WriteLine("5. Reports")
            Console.WriteLine("6. Register new admin user")
            Console.WriteLine("7. Exit")
            Console.WriteLine("8. Show Audit Trail")
            Console.Write("Select an option: ")

            Dim input = Console.ReadLine()

            Select Case input
                Case "1"
                    AddItem()
                Case "2"
                    ViewInventory()
                Case "3"
                    UpdateItem()
                Case "4"
                    DeleteItem()
                Case "5"
                    ShowReportsMenu()
                Case "6"
                    RegisterAdmin()
                Case "7"
                    ShowAuditTrails()
                Case "8"
                    SaveInventory()
                    running = False
                Case Else
                    Console.WriteLine("Invalid option. Press any key to continue...")
                    Console.ReadKey()
            End Select
        End While

        Console.WriteLine("Goodbye!")
    End Sub

    ' Secure password input
    Function ReadPassword(prompt As String) As String
        Console.Write(prompt)
        Dim password As String = ""
        Dim key As ConsoleKeyInfo

        Do
            key = Console.ReadKey(True)
            If key.Key = ConsoleKey.Enter Then
                Exit Do
            ElseIf key.Key = ConsoleKey.Backspace Then
                If password.Length > 0 Then
                    password = password.Substring(0, password.Length - 1)
                    Dim pos = Console.CursorLeft
                    If pos > prompt.Length Then
                        Console.CursorLeft = pos - 1
                        Console.Write(" "c)
                        Console.CursorLeft = pos - 1
                    End If
                End If
            ElseIf Not Char.IsControl(key.KeyChar) Then
                password &= key.KeyChar
                Console.Write("*"c)
            End If
        Loop
        Console.WriteLine()
        Return password
    End Function

    Sub LoadInventory()
        Inventory.Clear()
        If File.Exists(InventoryFilePath) Then
            For Each line In File.ReadAllLines(InventoryFilePath)
                Dim parts = line.Split(","c)
                If parts.Length = 4 Then
                    Dim item As New InventoryItem With {
                        .Id = Integer.Parse(parts(0)),
                        .Name = parts(1),
                        .Quantity = Integer.Parse(parts(2)),
                        .Price = Decimal.Parse(parts(3))
                    }
                    Inventory.Add(item)
                End If
            Next
            If Inventory.Count > 0 Then
                NextId = Inventory.Max(Function(i) i.Id) + 1
            End If
        End If
    End Sub

    Sub SaveInventory()
        Using writer As New StreamWriter(InventoryFilePath, False)
            For Each item In Inventory
                writer.WriteLine($"{item.Id},{item.Name},{item.Quantity},{item.Price}")
            Next
        End Using
    End Sub

    Sub ShowReportsMenu()
        Console.Clear()
        Console.WriteLine("=== Reports Menu ===")
        Console.WriteLine("1. Low Stock Warnings")
        Console.WriteLine("2. Sales Trends")
        Console.WriteLine("3. Back to Main Menu")
        Console.Write("Select a report: ")
        Dim reportInput = Console.ReadLine()
        Select Case reportInput
            Case "1"
                ShowLowStockReport()
            Case "2"
                ShowSalesTrendsReport()
            Case "3"
                Return
            Case Else
                Console.WriteLine("Invalid option. Press any key to continue...")
                Console.ReadKey()
        End Select
    End Sub

    Sub ShowLowStockReport()
        Console.Clear()
        Console.WriteLine("=== Low Stock Report ===")
        Dim lowStockThreshold As Integer = 5
        Dim found As Boolean = False
        For Each item In Inventory
            If item.Quantity <= lowStockThreshold Then
                Console.WriteLine("Item: " & item.Name & " | Quantity: " & item.Quantity)
                found = True
            End If
        Next
        If Not found Then
            Console.WriteLine("No items with low stock found.")
        End If
        Console.WriteLine("Press any key to return to Reports Menu...")
        Console.ReadKey()
    End Sub

    Sub ShowSalesTrendsReport()
        Console.Clear()
        Console.WriteLine("=== Sales Trends Report ===")
        Console.WriteLine("Sales trends feature coming soon.")
        Console.WriteLine("Press any key to return to Reports Menu...")
        Console.ReadKey()
    End Sub

    Sub RegisterAdmin()
        Console.Clear()
        Console.WriteLine("=== Register Admin User ===")
        Console.Write("Enter new admin username: ")
        Dim username = Console.ReadLine()
        Dim password = ReadPassword("Enter new admin password: ")
        If UserAuth.RegisterUser(username, password, "admin") Then
            Console.WriteLine("Admin user registered successfully!")
            AuditTrail.LogAction(LoggedInUsername, LoggedInRole, "Register Admin", $"Registered new admin: {username}")
        Else
            Console.WriteLine("Username already exists.")
        End If
        Console.WriteLine("Press any key to continue...")
        Console.ReadKey()
    End Sub

    Sub ViewInventory()
        Console.Clear()
        Console.WriteLine("=== Inventory List ===")
        If Inventory.Count = 0 Then
            Console.WriteLine("No items in inventory.")
        Else
            Console.WriteLine("{0,-5} {1,-20} {2,-10} {3,-10}", "ID", "Name", "Quantity", "Price")
            For Each item In Inventory
                Console.WriteLine("{0,-5} {1,-20} {2,-10} {3,-10:C}", item.Id, item.Name, item.Quantity, item.Price)
            Next
        End If
        Console.WriteLine("Press any key to continue...")
        Console.ReadKey()
    End Sub

    Sub AddItem()
        Console.Clear()
        Console.WriteLine("=== Add New Item ===")
        Console.Write("Name: ")
        Dim name = Console.ReadLine()
        Console.Write("Quantity: ")
        Dim quantity = Integer.Parse(Console.ReadLine())
        Console.Write("Price: ")
        Dim price = Decimal.Parse(Console.ReadLine())

        Dim item As New InventoryItem With {
            .Id = NextId,
            .Name = name,
            .Quantity = quantity,
            .Price = price
        }
        Inventory.Add(item)
        NextId += 1
        SaveInventory()

        Console.WriteLine("Item added successfully! Press any key to continue...")
        Console.ReadKey()
        AuditTrail.LogAction(LoggedInUsername, LoggedInRole, "Add Item", $"Added item: {name}, Quantity: {quantity}")
    End Sub

    Sub UpdateItem()
        Console.Clear()
        Console.WriteLine("=== Update Item ===")
        Console.Write("Enter Item ID: ")
        Dim id = Integer.Parse(Console.ReadLine())
        Dim item = Inventory.FirstOrDefault(Function(x) x.Id = id)
        If item Is Nothing Then
            Console.WriteLine("Item not found!")
        Else
            Console.Write("New Name (leave blank to keep {0}): ", item.Name)
            Dim name = Console.ReadLine()
            If name <> "" Then item.Name = name

            Console.Write("New Quantity (leave blank to keep {0}): ", item.Quantity)
            Dim quantityStr = Console.ReadLine()
            If quantityStr <> "" Then item.Quantity = Integer.Parse(quantityStr)

            Console.Write("New Price (leave blank to keep {0}): ", item.Price)
            Dim priceStr = Console.ReadLine()
            If priceStr <> "" Then item.Price = Decimal.Parse(priceStr)

            SaveInventory()
            Console.WriteLine("Item updated!")
            AuditTrail.LogAction(LoggedInUsername, LoggedInRole, "Update Item", $"Updated item: {id}")
        End If
        Console.WriteLine("Press any key to continue...")
        Console.ReadKey()
    End Sub

    Sub DeleteItem()
        Console.Clear()
        Console.WriteLine("=== Delete Item ===")
        Console.Write("Enter Item ID: ")
        Dim id = Integer.Parse(Console.ReadLine())
        Dim item = Inventory.FirstOrDefault(Function(x) x.Id = id)
        If item Is Nothing Then
            Console.WriteLine("Item not found!")
        Else
            Inventory.Remove(item)
            SaveInventory()
            Console.WriteLine("Item deleted!")
            AuditTrail.LogAction(LoggedInUsername, LoggedInRole, "Delete Item", $"Deleted item: {id}")
        End If
        Console.WriteLine("Press any key to continue...")
        Console.ReadKey()
    End Sub

    Sub ShowAuditTrails()
        Console.Clear()
        Console.WriteLine("=== Audit Trail ===")
        Dim auditFile As String = "audit_trail.txt"
        If File.Exists(auditFile) Then
            For Each line In File.ReadAllLines(auditFile)
                Console.WriteLine(line)
            Next
        Else
            Console.WriteLine("No audit trail found.")
        End If
        Console.WriteLine("Press any key to continue...")
        Console.ReadKey()
    End Sub

End Module