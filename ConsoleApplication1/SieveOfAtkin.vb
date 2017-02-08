Public Class SieveOfAtkin
    Private _limit As UInteger
    Private is_prime As New Dictionary(Of UInteger, Boolean)
    Private Shared s As UInteger() = {1, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 49, 53, 59}
    Private Sub New(ByVal limit As BigInteger)
        _limit = limit
        For i As UInteger = 0 To _limit Step 60
            For k As Byte = 0 To 15
                is_prime.Add(i + s(k), False)
            Next
        Next
    End Sub
    Private Sub Step3_1()
        Dim x As UInteger = 1, xn As UInteger = 4, y As UInteger, yn As UInteger, n As UInteger
        Dim solutions As New List(Of UInteger)
        While xn < _limit
            y = 1 : yn = 1
            n = xn + yn
            While n < _limit
                If is_prime.ContainsKey(n) AndAlso {1, 13, 17, 29, 37, 41, 49, 53}.Contains(n Mod 60) AndAlso Not solutions.Contains(n) Then solutions.Add(n)
                y += 2
                yn = y * y
                n = xn + yn
            End While
            x += 2
            xn = 4 * x * x
        End While
        For Each n In solutions
            is_prime(n) = True 'Not sieveList(n), but all values are already false at this point
        Next
    End Sub
    Private Sub Step3_2()
        Dim x As UInteger = 1, xn As UInteger = 3, y As UInteger, yn As UInteger, n As UInteger
        Dim solutions As New List(Of UInteger)
        While xn < _limit
            y = 2 : yn = 4
            n = xn + yn
            While n < _limit
                If is_prime.ContainsKey(n) AndAlso {7, 19, 31, 43}.Contains(n Mod 60) AndAlso Not solutions.Contains(n) Then solutions.Add(n)
                y += 2
                yn = y * y
                n = xn + yn
            End While
            x += 2
            xn = 3 * x * x
        End While
        For Each n In solutions
            is_prime(n) = Not is_prime(n)
        Next
    End Sub
    Private Sub Step3_3()
        Dim x As UInteger = 2, xn As UInteger = 4, y As UInteger, yn As UInteger, n As UInteger
        Dim solutions As New List(Of UInteger)
        While xn < _limit
            y = x - 1 : yn = y * y
            n = xn - yn
            While n < _limit
                If is_prime.ContainsKey(n) AndAlso {11, 23, 47, 59}.Contains(n Mod 60) AndAlso Not solutions.Contains(n) Then solutions.Add(n)
                y += 2
                yn = y * y
                n = xn + yn
            End While
            x += 2
            xn = 3 * x * x
        End While
        For Each n In solutions
            is_prime(n) = Not is_prime(n)
        Next
    End Sub
    Private Sub sieve()
        Dim w As UInteger = 0, n As UInteger = 7, nSqr As BigInteger = 0, c As BigInteger = 0, k As UInteger = 0
        While nSqr < _limit
            For x As Byte = 0 To 15
                n = w + s(x)
                nSqr = n * n
                If n >= 7 AndAlso is_prime.ContainsKey(n) AndAlso is_prime(n) Then
                    c = nSqr
                    While c < _limit
                        For i As Byte = 0 To 15
                            c = nSqr * (k + s(i))
                            If c > _limit Then Exit While
                            If is_prime.ContainsKey(c) Then is_prime(c) = False
                        Next
                        k += 60
                    End While
                End If
            Next
            w += 60
        End While
    End Sub
    Public Shared Function Generate(ByVal limit As UInteger, ByVal timer As Stopwatch) As List(Of UInteger)
        timer.Start()
        With New SieveOfAtkin(limit)
            .Step3_1()
            .Step3_2()
            .Step3_3()
            .sieve()
            Generate = New List(Of UInteger) From {2, 3, 5}
            Generate.AddRange(.is_prime.Where(Function(p) p.Value AndAlso p.Key >= 7).Select(Function(p) p.Key))
        End With
        timer.Stop()

    End Function
End Class
