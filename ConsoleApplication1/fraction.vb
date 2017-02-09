Public Class fraction
    Private _num As BigInteger
    Private _den As BigInteger
    Private _simplestForm As fraction = Nothing
    Public Shared maxPrime As UInteger = 0
    Public ReadOnly Property Numerator As BigInteger
        Get
            Return _num
        End Get
    End Property
    Public ReadOnly Property Denominator As BigInteger
        Get
            Return _den
        End Get
    End Property
    Public ReadOnly Property ExpValue As Double
        Get
            Return BigInteger.Log(_num) - BigInteger.Log(_den)
        End Get
    End Property
    ' If this weren't using only positive numbers, I'd include a sign boolean

    Public ReadOnly Property SimplestForm() As fraction
        Get
            If _simplestForm Is Nothing Then
                If Denominator = 1 OrElse Numerator = 1 Then
                    _simplestForm = Me
                Else
                    Dim n As BigInteger = Numerator, d As BigInteger = Denominator
                    Dim NumIsMin As Boolean = n <= d
                    If If(NumIsMin, d, n) Mod If(NumIsMin, n, d) = 0 Then
                        If NumIsMin Then
                            d /= n
                            n = 1
                        Else
                            n /= d
                            d = 1
                        End If
                    Else
                        Dim k As Integer = 0
                        Do
                            If n Mod pCache(k) = 0 AndAlso d Mod pCache(k) = 0 Then
                                n /= pCache(k)
                                d /= pCache(k)
                                If pCache(k) > maxPrime Then maxPrime = pCache(k)
                                If If(NumIsMin, n, d) = 1 Then Exit Do
                            Else
                                k += 1
                                If k = pCache.Count OrElse pCache(k) > If(NumIsMin, n, d) / 2 Then Exit Do
                            End If
                        Loop
                    End If
                    If d = Denominator Then
                        _simplestForm = Me
                    Else
                        _simplestForm = New fraction(n, d)
                    End If
                End If
            End If
            Return _simplestForm
        End Get
    End Property
    Public Sub New(ByVal n As BigInteger, ByVal d As BigInteger)
        _num = n
        _den = d
    End Sub
    Public Overrides Function ToString() As String
        If _den > UInteger.MaxValue Then Return "e^" & BigInteger.Log(_num) - BigInteger.Log(_den)
        Return If(_den = 1, "", "(") & _num.ToString("n0") & If(_den = 1, "", "/" & _den.ToString("n0") & ")")
    End Function
    Public Overloads Shared Operator *(ByVal left As fraction, ByVal right As fraction)
        Return (New fraction(left.Numerator * right.Numerator, left.Denominator * right.Denominator)).SimplestForm
    End Operator
    Public Overloads Shared Operator =(ByVal left As fraction, ByVal right As fraction) As Boolean
        Return left.SimplestForm.Numerator = right.SimplestForm.Numerator AndAlso left.SimplestForm.Denominator = right.SimplestForm.Denominator
    End Operator
    Public Overloads Shared Operator <>(ByVal left As fraction, ByVal right As fraction) As Boolean
        Return Not (left = right)
    End Operator
    Public Shared Widening Operator CType(ByVal value As Integer) As fraction
        Return New fraction(value, 1)
    End Operator
End Class
