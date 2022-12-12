Imports System.Data.SqlClient

Public Class DbManager
    'Insert new user into our MS SQL database
    Public Shared Sub InsertNewUser(username As String, password As String, salt As String)

        If Not DuplicateUser(username) Then

            'Change to your database parameters (!)
            Dim ConnectionString As String = "Server=my_server;Database=name_of_db;User Id=user_name;Password=my_password"

            Using sqlConn As New SqlConnection(ConnectionString)

                Dim insertNewUser As String = "INSERT INTO loginTable(username,password,salt) VALUES(@user,@pass,@salt)"

                Dim SqlCommand As New SqlCommand(insertNewUser, sqlConn)
                SqlCommand.Parameters.AddWithValue("@user", username)
                SqlCommand.Parameters.AddWithValue("@pass", password)
                SqlCommand.Parameters.AddWithValue("@salt", salt)

                If ConnectedToServerAsync(sqlConn).Result Then
                    SqlCommand.ExecuteNonQuery()
                    Console.WriteLine(String.Format("{0} has been added to the system", username))
                End If
                Return
            End Using
        End If

        Console.WriteLine(String.Format("{0} is already in the system", username))

    End Sub


    'Check If user have duplicates in out MS SQL database
    Private Shared Function DuplicateUser(username As String) As Boolean

        'Change to your database parameters (!)
        Dim ConnectionString As String = "Server=my_server;Database=name_of_db;User Id=user_name;Password=my_password"

        Using sqlConn As New SqlConnection(ConnectionString)

            Dim checkUserQuery As String = "SELECT COUNT(username) FROM loginTable WHERE username=@user"

            Dim sqlCommand As New SqlCommand(checkUserQuery, sqlConn)

            sqlCommand.Parameters.AddWithValue("@user", username)

            If ConnectedToServerAsync(sqlConn).Result Then

                Dim result As Integer = Convert.ToInt32(sqlCommand.ExecuteScalar())

                If result > 0 Then
                    Return True
                Else
                    Return False
                End If
            End If
        End Using
        Return True
    End Function

    Public Shared Sub Login(username As String, password As String)

        'Change to your database parameters (!)
        Dim ConnectionString As String = "Server=my_server;Database=name_of_db;User Id=user_name;Password=my_password"

        Dim salt As String = ""

        Using sqlConn As New SqlConnection(ConnectionString)

            Dim readSaltQuery As String = "SELECT * FROM loginTable WHERE username=@user"

            Dim sqlCommand As New SqlCommand(readSaltQuery, sqlConn)

            sqlCommand.Parameters.AddWithValue("@user", username)

            If ConnectedToServerAsync(sqlConn).Result Then

                Dim reader As SqlDataReader = sqlCommand.ExecuteReader()

                While reader.Read()

                    salt = reader("salt").ToString()

                End While

                reader.Close()

                Dim pass = Encryption.HashString(password)
                Dim hashedAndSalted = Encryption.HashString(String.Format("{0}{1}", pass, salt))

                Dim checkLoginQuery As String = "SELECT COUNT(*) FROM loginTable WHERE username =@user AND password = @pass"
                Dim sqlCommand0 As New SqlCommand(checkLoginQuery, sqlConn)

                sqlCommand0.Parameters.AddWithValue("@user", username)
                sqlCommand0.Parameters.AddWithValue("@pass", hashedAndSalted)

                Dim results As Integer = Convert.ToInt32(sqlCommand0.ExecuteScalar())

                If results = 1 Then
                    Console.WriteLine("Welcome: " & username)
                Else
                    Console.WriteLine("Your username or password was incorrect")
                End If
            End If

        End Using
    End Sub

    Public Shared Async Function ConnectedToServerAsync(sqlConn As SqlConnection) As Task(Of Boolean)

        Try
            Await sqlConn.OpenAsync()
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try
        Return sqlConn.State

    End Function
End Class
