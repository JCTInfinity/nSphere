Option Strict On
Public Class fraction
    Private _num As BigIntPrime
    Private _den As BigIntPrime
    Private _simplestForm As fraction = Nothing
    Public Shared maxPrime As UInteger = 0
    Public ReadOnly Property Numerator As BigInteger
        Get
            Return _num.value
        End Get
    End Property
    Public ReadOnly Property Denominator As BigInteger
        Get
            Return _den.value
        End Get
    End Property
    Public ReadOnly Property ExpValue As Double
        Get
            Return BigInteger.Log(Numerator) - BigInteger.Log(Denominator)
        End Get
    End Property
    ' If this weren't using only positive numbers, I'd include a sign boolean

    Public ReadOnly Property SimplestForm() As fraction
        Get
            If _simplestForm Is Nothing Then findSimplestForm()
            Return _simplestForm
        End Get
    End Property
    Private Sub findSimplestForm()
        If Denominator = 1 OrElse Numerator = 1 Then
            _simplestForm = Me
        Else
            Dim n As BigIntPrime = _num, d As BigIntPrime = _den
            Dim NumIsMin As Boolean = n.value <= d.value
            If If(NumIsMin, d, n).value Mod If(NumIsMin, n, d).value = 0 Then
                If NumIsMin Then
                    d /= n
                    n = 1
                Else
                    n /= d
                    d = 1
                End If
            Else
                Dim k As Integer = 0
                Do While pCache(k) <= n \ d
                    If n.value Mod pCache(k) = 0 AndAlso d.value Mod pCache(k) = 0 Then
                        n /= pCache(k)
                        d /= pCache(k)
                        If pCache(k) > maxPrime Then maxPrime = pCache(k)
                        If If(NumIsMin, n, d).value = 1 Then Exit Do
                    Else
                        k += 1
                        If k = pCache.Count Then Exit Do
                    End If
                Loop
            End If
            If d.value = Denominator Then
                _simplestForm = Me
            Else
                _simplestForm = New fraction(n, d)
            End If
        End If
    End Sub
    Public Sub New(ByVal n As BigIntPrime, ByVal d As BigIntPrime)
        _num = n
        _den = d
    End Sub
    Public Overloads Function ToString(ByVal FullFraction As Boolean) As String
        If Not FullFraction AndAlso Denominator > UInteger.MaxValue Then Return "e^" & BigInteger.Log(Numerator) - BigInteger.Log(Denominator)
        Return If(Denominator = 1, "", "(") & Numerator.ToString("n0") & If(Denominator = 1, "", "/" & Denominator.ToString("n0") & ")")
    End Function
    Public Overrides Function ToString() As String
        Return ToString(False)
    End Function
    Public Overloads Shared Operator *(ByVal left As fraction, ByVal right As fraction) As fraction
        Return (New fraction(left._num * right._num, left._den * right._den)).SimplestForm
    End Operator
    Public Overloads Shared Operator =(ByVal left As fraction, ByVal right As fraction) As Boolean
        Return left.SimplestForm.Numerator = right.SimplestForm.Numerator AndAlso left.SimplestForm.Denominator = right.SimplestForm.Denominator
    End Operator
    Public Overloads Shared Operator <>(ByVal left As fraction, ByVal right As fraction) As Boolean
        Return Not (left = right)
    End Operator
    Public Overloads Shared Operator =(ByVal left As fraction, ByVal right As UInteger) As Boolean
        If left.Denominator > left.Numerator Then Return False
        Dim iForm As BigInteger = left.Numerator Mod left.Denominator
        If iForm <> 0 Then Return False
        Return left.Numerator / left.Denominator = right
    End Operator
    Public Overloads Shared Operator <>(ByVal left As fraction, ByVal right As UInteger) As Boolean
        Return Not (left = right)
    End Operator
    Public Shared Widening Operator CType(ByVal value As UInteger) As fraction
        Return New fraction(value, 1)
    End Operator
End Class
