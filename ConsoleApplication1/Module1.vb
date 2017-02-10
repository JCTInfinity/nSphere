Imports System.IO
Module Module1
    Sub Main()
        While doThings() : End While
    End Sub
function doThings() as boolean
        Dim input As String, dimensions As UInteger
        Console.WriteLine("Enter a number of dimensions (integer >= 0)")
        input = Console.ReadLine
        If Not Integer.TryParse(input, dimensions) OrElse dimensions < 1 Then
            Console.WriteLine("Invalid input. Enter a positive integer or 0.")
            Return True
        End If
        Dim track As New Stopwatch
        If pCache Is Nothing OrElse pCache.Count < dimensions * 2 Then
            pCache = SieveOfAtkin.Generate(dimensions * 2, track)
            Console.WriteLine(pCache.Count & " primes generated in " & track.Elapsed.ToString("g") & " ending with " & pCache.Last.ToString)
        End If
        Dim s As SphereFormula = Nothing
        Console.WriteLine("Time" & vbTab & vbTab & vbTab & "Dim" & vbTab & "Prime" & vbTab & "Formula")
        Dim lastUpdate As New TimeSpan(0)
        Dim startDim As UInteger = If(SphereFormula.sCache IsNot Nothing, SphereFormula.sCache.Dimensions, 1)
        If startDim > dimensions Then startDim = 1
        For i = startDim To dimensions
            Try
                track.Start()
                s = SphereFormula.ForDimension(i)
                track.Stop()
                If lastUpdate + New TimeSpan(0, 0, 6) <= track.Elapsed AndAlso i < dimensions Then
                    Console.WriteLine(track.Elapsed.ToString("g") & vbTab & vbTab & i & vbTab & fraction.maxPrime & vbTab & s.ToString)
                    lastUpdate = track.Elapsed
                End If
            Catch ex As Exception
                track.Stop()
                Console.WriteLine("Unable to calculate beyond " & s.Dimensions & " dimensions due to " & ex.Message)
                Exit For
            End Try
            'Console.WriteLine(track.Elapsed.ToString("g") & vbTab & vbTab & i & vbTab & fraction.maxPrime & vbTab & s.ToString)
            'fraction.maxPrime = 0
        Next
        Console.WriteLine(track.Elapsed.ToString("g") & vbTab & vbTab & dimensions & vbTab & fraction.maxPrime & vbTab & s.ToString)
        Console.WriteLine("A unit (r=1) " & s.Dimensions & "-sphere has a " & s.Dimensions & "-volume of " & s.nVolume(1) & " (e^" & s.nVolumeExp(1) & ")")
        Console.WriteLine()
        Console.WriteLine("If you would like to calculate the " & s.Dimensions & "-volume for a different value of r, enter a floating-point value to use.")
        Console.WriteLine("To exit, type Exit. To start over, type New. To display the full fractional form, type Fraction.")
        Do
            input = Console.ReadLine
            Dim r As Double
            If Double.TryParse(input, r) Then
                Console.WriteLine("With r=" & r & " the " & s.Dimensions & "-volume = " & s.nVolume(r) & " (e^" & s.nVolumeExp(r) & ")")
                Console.WriteLine("Calculate with another value, Exit, or New?")
            Else
                Select Case input.ToLower.Trim
                    Case ""
                    Case "exit"
                        Return False
                    Case "new"
                        Return True
                    Case "fraction"
                        Console.WriteLine(s.ToString(True))
                    Case Else
                        Console.WriteLine("Unable to parse that value. Will now exit.")
                End Select
            End If
        Loop
    End Function
End Module
