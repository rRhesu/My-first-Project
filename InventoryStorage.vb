Imports System.IO

' Handles saving/loading inventory to/from a CSV file
Public Module InventoryStorage
    Private ReadOnly FilePath As String = "inventory.csv"

    Public Sub SaveInventory(inventory As List(Of InventoryItem))
        Using writer As New StreamWriter(FilePath, False)
            For Each item In inventory
                writer.WriteLine($"{item.Id},{item.Name},{item.Category},{item.Quantity},{item.Price},{item.CreatedAt:o},{item.UpdatedAt:o}")
            Next
        End Using
    End Sub

    Public Function LoadInventory() As List(Of InventoryItem)
        Dim inventory As New List(Of InventoryItem)
        If Not File.Exists(FilePath) Then Return inventory
        For Each line In File.ReadAllLines(FilePath)
            Dim parts = line.Split(","c)
            If parts.Length >= 7 Then
                inventory.Add(New InventoryItem With {
                    .Id = Integer.Parse(parts(0)),
                    .Name = parts(1),
                    .Category = parts(2),
                    .Quantity = Integer.Parse(parts(3)),
                    .Price = Decimal.Parse(parts(4)),
                    .CreatedAt = DateTime.Parse(parts(5)),
                    .UpdatedAt = DateTime.Parse(parts(6))
                })
            End If
        Next
        Return inventory
    End Function
End Module
