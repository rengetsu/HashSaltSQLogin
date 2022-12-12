Imports System

Module Program
    Sub Main(args As String())

        Dim username As String = "PAVEL TROSTIANKO"
        Dim password = Encryption.HashString("password")
        Dim salt = Encryption.GenerateSalt
        Dim hashedAndSalted = Encryption.HashString(String.Format("{0}{1}", password, salt))

        'Showing password, salt and hashed and salted
        Console.WriteLine(password)
        Console.WriteLine(salt)
        Console.WriteLine(hashedAndSalted)

        'Work with MS SQL Database part (bandom pridet nauja useri)
        DbManager.InsertNewUser(username, hashedAndSalted, salt)
        DbManager.Login(username, "password")

    End Sub
End Module
