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
                ElseIf Numerator = Denominator Then : _simplestForm = 1
                Else
                    Dim NumIsMin As Boolean = Numerator <= Denominator
                    Dim smaller As BigInteger = If(NumIsMin, Numerator, Denominator), bigger As BigInteger = If(NumIsMin, Denominator, Numerator)
                    If bigger Mod smaller = 0 Then
                        bigger /= smaller
                        smaller = 1
                    Else
                        Dim k As Integer = 0, check As BigInteger = smaller
                        While check > 1
                            If smaller Mod pCache(k) = 0 Then
                                check /= pCache(k)
                                If bigger Mod pCache(k) = 0 Then
                                    smaller /= pCache(k)
                                    bigger /= pCache(k)
                                    If pCache(k) > maxPrime Then maxPrime = pCache(k)
                                    Continue While
                                End If
                            End If
                            k += 1
                            If k = pCache.Count OrElse pCache(k) > check / 2 Then Exit While
                        End While
                    End If
                    If If(NumIsMin, bigger, smaller) = Denominator Then
                        _simplestForm = Me
                    Else
                        _simplestForm = If(NumIsMin, New fraction(smaller, bigger), New fraction(bigger, smaller))
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
    Public Overloads Function ToString(Optional ByVal FullFraction As Boolean = False) As String
        If Not FullFraction AndAlso _den > UInteger.MaxValue Then Return "e^" & BigInteger.Log(_num) - BigInteger.Log(_den)
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
