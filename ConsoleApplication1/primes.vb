Public Module primes
    Public pCache As New List(Of BigInteger) From {2, 3}
    Public Sub FindNextPrime(ByVal limit As BigInteger)
        Static n As BigInteger = 6
        Do
            If n - 1 > limit Then Exit Do
            If AddIfPrime(n - 1) Or AddIfPrime(n + 1) Then
                n += 6
                Exit Do
            End If
            n += 6
        Loop
    End Sub
    Private Function AddIfPrime(ByVal n As UInt32) As Boolean
        Dim limit As Integer = n \ 2
        If pCache.Take(pCache.IndexOf(pCache.Last(Function(p) p <= limit))).AsParallel.All(Function(p) n Mod p <> 0) Then
            pCache.Add(n)
            Return True
        End If
        Return False
    End Function
End Module
