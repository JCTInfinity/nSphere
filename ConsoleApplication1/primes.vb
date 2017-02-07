Public Module primes
    Public pCache As New List(Of Integer) From {2, 3, 5, 7, 11}
    Public Sub FindNextPrime(ByVal limit As Integer)
        Static timer As New Stopwatch
        Dim n As Integer = pCache.Last
        Do
            n += 2
            If n > limit Then Exit Do
            If Not pCache.AsParallel.Any(Function(p) n Mod p = 0) Then
                pCache.Add(n)
                Exit Do
            End If
        Loop
    End Sub
End Module
