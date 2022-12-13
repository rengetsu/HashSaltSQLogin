Imports System

Module Program
    Sub Main(args As String())

        Dim username As String = "mantas"
        Dim password = Encryption.HashString("pass12345")
        Dim passwordUTF8 = Encryption.HashStringUTF8("漢字漢字漢字漢字")
        Dim salt = Encryption.GenerateSalt
        Dim hashedAndSalted = Encryption.HashString(String.Format("{0}{1}", password, salt))

        'Showing password, salt and hashed and salted
        Console.WriteLine("Password: " + password)
        Console.WriteLine("Password UTF-8: " + passwordUTF8)

        Console.WriteLine("Salt: " + salt)
        Console.WriteLine("Hashed and salted: " + hashedAndSalted)

        'Work with MS SQL Database part (bandom pridet nauja useri)
        'DbManager.InsertNewUser(username, hashedAndSalted, salt)
        'DbManager.Login(username, "password")
    End Sub
End Module
