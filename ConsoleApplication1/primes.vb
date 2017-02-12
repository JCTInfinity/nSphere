Public Module primes
    Public pCache As List(Of UInteger)
    Public Function LargestPrime(ByVal composite As UInteger) As UInteger
        If composite = 1 Then Return 1
        If pCache.Contains(composite) Then Return composite
        Dim start As Integer = pCache.IndexOf(pCache.LastOrDefault(Function(p) p <= composite \ 2))
        If start < 0 Then Return composite
        For i As Integer = start To 0 Step -1
            If composite Mod pCache(i) = 0 Then Return pCache(i)
        Next
        Return composite
    End Function
    Public Function LargestPrime(ByVal composite As BigInteger, ByVal oldLargestPrime As UInteger) As UInteger
        If composite = 1 Then Return 1
        For i As Integer = pCache.IndexOf(oldLargestPrime) To 0 Step -1
            If composite Mod pCache(i) = 0 Then Return pCache(i)
        Next
        Return 1 ' it will never get to this line, but this eliminates a warning. *shrug*
    End Function
End Module
