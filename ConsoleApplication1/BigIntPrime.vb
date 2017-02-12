Public Class BigIntPrime
    Public Property value As BigInteger
    Public Property prime As UInteger
    Public Sub New(ByVal value As BigInteger, ByVal prime As UInteger)
        Me.value = value
        Me.prime = prime
    End Sub
    Public Sub New(ByVal value As UInteger)
        Me.value = value
        prime = LargestPrime(value)
    End Sub
    Public Overrides Function ToString() As String
        Return value.ToString & " (" & prime.ToString & ")"
    End Function
    Public Shared Widening Operator CType(ByVal value As UInteger) As BigIntPrime
        Return New BigIntPrime(value)
    End Operator
    Public Overloads Shared Operator *(ByVal left As BigIntPrime, ByVal right As BigIntPrime) As BigIntPrime
        Return New BigIntPrime(left.value * right.value, left \ right)
    End Operator
    Public Overloads Shared Operator /(ByVal left As BigIntPrime, ByVal right As BigIntPrime) As BigIntPrime
        Dim value As BigInteger = left.value / right.value
        Dim prime As UInteger = LargestPrime(value, left \ right)
        Return New BigIntPrime(value, prime)
    End Operator
    Public Overloads Shared Operator \(ByVal left As BigIntPrime, ByVal right As BigIntPrime) As UInteger
        Return If(left.prime < right.prime, right.prime, left.prime)
    End Operator
End Class
