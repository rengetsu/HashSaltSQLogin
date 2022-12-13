Public Class Encryption

    Private cryotiServiceProvider As System.Security.Cryptography.RNGCryptoServiceProvider

    Public Shared Function HashString(str As String) As String
        Return EncryptString(str)
    End Function

    Public Shared Function HashStringUTF8(str As String) As String
        Return EncryptStringSHA512(str)
    End Function

    Public Shared Function GenerateSalt() As String

        Using cryotiServiceProvider As New System.Security.Cryptography.RNGCryptoServiceProvider
            Dim sb As New System.Text.StringBuilder()
            Dim data As Byte() = New Byte(4) {}
            For i = 0 To 6
                cryotiServiceProvider.GetBytes(data)
                Dim value As String = BitConverter.ToString(data, 0)
                sb.Append(value)
            Next
            Return EncryptString(sb.ToString())
        End Using

    End Function

    Private Shared Function EncryptString(str As String) As String
        Dim bytes As Byte() = System.Text.Encoding.ASCII.GetBytes(str)
        Dim hashed = System.Security.Cryptography.SHA256.Create().ComputeHash(bytes)
        Return Convert.ToBase64String(hashed)
    End Function

    Private Shared Function EncryptStringSHA512(str As String) As String
        Dim new_bytes As Byte() = System.Text.Encoding.UTF8.GetBytes(str)
        Dim hashed_512 = System.Security.Cryptography.SHA512.Create().ComputeHash(new_bytes)
        Return Convert.ToBase64String(hashed_512)
    End Function
End Class
