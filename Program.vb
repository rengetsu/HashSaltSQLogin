Imports System

Module Program
    Sub Main(args As String())

        Dim password = Encryption.HashString("password")
        Dim salt = Encryption.GenerateSalt
        Dim hashedAndSalted = Encryption.HashString(String.Format("{0}{1}", password, salt))

        Console.WriteLine(password)
        Console.WriteLine(salt)
        Console.WriteLine(hashedAndSalted)

    End Sub
End Module
