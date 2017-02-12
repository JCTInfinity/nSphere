Public Class SieveOfAtkin
    ' Sieve Of Atkin, based on the wikipedia article: https://en.wikipedia.org/wiki/Sieve_of_Atkin
    ' And further developed based on Axel Magnuson's firstpass function found in the code he posted
    '   here: http://mathoverflow.net/questions/13116/c-sieve-of-atkin-overlooks-a-few-prime-numbers

    Private _limit As UInteger
    Private is_prime As New Dictionary(Of UInteger, Boolean)
    Private Shared s As UInteger() = {1, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 49, 53, 59}
    Private Sub New(ByVal limit As UInteger)
        _limit = limit
        For i As UInteger = 1 To _limit
            If s.Contains(i Mod 60) Then is_prime(i) = False
        Next
    End Sub
    Private Sub switch(ByVal n As UInteger)
        If is_prime.ContainsKey(n) Then is_prime(n) = Not is_prime(n)
    End Sub
    Private Shared Function sqrt(ByVal value As Double) As UInteger
        Return Math.Sqrt(Math.Abs(value))
    End Function
    Private Sub firstpass()
        Dim xroof As UInteger, x As UInteger, yroof As UInteger, y As UInteger, n As UInteger, nmod As UInteger
        ' n = 4x^2 + y^2
        xroof = sqrt((_limit - 1) / 4)
        For x = 1 To xroof
            yroof = sqrt(_limit - 4 * x * x)
            For y = 1 To yroof
                n = 4 * x * x + y * y
                nmod = n Mod 12
                If {1, 5}.Contains(nmod) Then switch(n)
            Next
        Next
        ' n = 3x^2 + y^2
        xroof = sqrt((_limit - 1) / 3)
        For x = 1 To xroof
            yroof = sqrt(_limit - 3 * x * x)
            For y = 1 To yroof
                n = 3 * x * x + y * y
                nmod = n Mod 12
                If nmod = 7 Then switch(n)
            Next
        Next
        ' n = 3x^2 - y^2 (for x > y)
        xroof = sqrt((_limit + 1) / 3)
        For x = 1 To xroof
            yroof = sqrt(3 * x * x - 1)
            If yroof >= x Then yroof = x - 1
            For y = 1 To yroof
                n = 3 * x * x - y * y
                nmod = n Mod 12
                If nmod = 11 Then switch(n)
            Next
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
            .firstpass()
            .sieve()
            Generate = New List(Of UInteger) From {2, 3, 5, 7}
            Generate.AddRange(.is_prime.Where(Function(p) p.Value AndAlso p.Key > 7).Select(Function(p) p.Key))
        End With
        timer.Stop()
    End Function
End Class
