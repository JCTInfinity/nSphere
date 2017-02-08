Imports System.IO
Module Module1
    Sub Main()
        Dim input As String, dimensions As Integer
        Console.WriteLine("Enter a number of dimensions (integer > 0)")
        input = Console.ReadLine
        If Not Integer.TryParse(input, dimensions) OrElse dimensions < 1 Then
            Console.WriteLine("Invalid input. Enter an integer greater than 0.")
            Main()
            Exit Sub
        End If
        Dim track As New Stopwatch
        pCache = SieveOfAtkin.Generate(1000000, track)
        Console.WriteLine(pCache.Count & " primes generated in " & track.Elapsed.ToString("g") & " ending with " & pCache.Last.ToString)
        Dim s As SphereFormula = Nothing
        Console.WriteLine("Time" & vbTab & vbTab & vbTab & "Dim" & vbTab & "Formula")
        For i = 0 To dimensions
            Try
                track.Start()
                s = SphereFormula.ForDimension(i)
                track.Stop()
            Catch ex As Exception
                track.Stop()
                Console.WriteLine("Unable to calculate beyond " & s.Dimensions & " dimensions due to " & ex.Message)
                Exit For
            End Try
            Console.WriteLine(track.Elapsed.ToString("g") & vbTab & vbTab & i & vbTab & s.ToString)
        Next
        Console.WriteLine("A unit (r=1) " & s.Dimensions & "-sphere has a " & s.Dimensions & "-volume of " & s.nVolume(1))
        Console.WriteLine("If you would like to calculate the " & s.Dimensions & "-volume for a different value of r, enter a floating-point value to use.")
        input = Console.ReadLine
        If String.IsNullOrWhiteSpace(input) Then Exit Sub
        Dim r As Double
        If Double.TryParse(input, r) Then
            Console.WriteLine("With r=" & r & " the " & s.Dimensions & "-volume = " & s.nVolume(r))
        Else
            Console.WriteLine("Unable to parse that value. Will now exit.")
        End If
        Console.ReadKey()
    End Sub
End Module
