
''' <summary>
''' Used to set the status for different objects
''' </summary>
''' <remarks></remarks>
Public Enum TheStatus As Integer
    Active = 1
    Inactive = 2
End Enum

Public Enum SecurityRole As Integer
    Developer = 1
    Admin = 2
    Worker = 3
    Client = 4
End Enum


Public Class Helper

    Public Shared Function getIntegerValue(ByRef dr As System.Data.SqlClient.SqlDataReader, ByVal FieldName As String) As Integer
        If IsDBNull(dr.Item(FieldName)) Then
            Return 0
        Else
            Return CType(dr.Item(FieldName), Integer)
        End If
    End Function

    Public Shared Function getLongValue(ByRef dr As System.Data.SqlClient.SqlDataReader, ByVal FieldName As String) As Long
        If IsDBNull(dr.Item(FieldName)) Then
            Return 0
        Else
            Return CType(dr.Item(FieldName), Long)
        End If
    End Function

    Public Shared Function getStringValue(ByRef dr As System.Data.SqlClient.SqlDataReader, ByVal FieldName As String) As String
        If IsDBNull(dr.Item(FieldName)) Then
            Return String.Empty
        Else
            Return CType(dr.Item(FieldName), String)
        End If
    End Function

    Public Shared Function getDecimalValue(ByRef dr As System.Data.SqlClient.SqlDataReader, ByVal FieldName As String) As Decimal
        If IsDBNull(dr.Item(FieldName)) Then
            Return 0
        Else
            Return CType(dr.Item(FieldName), Decimal)
        End If
    End Function

    Public Shared Function getDateTimeValue(ByRef dr As System.Data.SqlClient.SqlDataReader, ByVal FieldName As String) As Date
        If IsDBNull(dr.Item(FieldName)) Then
            Return Date.Parse("1 Jan 1900")
        Else
            Return CType(dr.Item(FieldName), Date)
        End If
    End Function

    Public Shared Function getBooleanValue(ByRef dr As System.Data.SqlClient.SqlDataReader, ByVal FieldName As String) As Boolean
        If IsDBNull(dr.Item(FieldName)) Then
            Return False
        Else
            Return CType(dr.Item(FieldName), Boolean)
        End If
    End Function

    Public Shared Function getString(ByVal myString As Object) As String
        If IsDBNull(myString) Then
            myString = ""
        End If
        Return myString
    End Function

    Public Shared Function getDate(ByVal myDate As Object) As String
        If IsDBNull(myDate) Then
            myDate = Today
        End If
        Return myDate
    End Function

End Class
