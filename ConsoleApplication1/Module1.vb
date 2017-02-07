Imports System.IO

Module Module1

    Sub Main()
        Dim input As String, dimensions As Integer
        Console.WriteLine("Enter a number of dimensions (integer > 0)")
        input = Console.ReadLine
        If Not Integer.TryParse(input, dimensions) OrElse dimensions < 1 Then
            Console.WriteLine("Invalid input. Enter an integer greater than 0.")
            Main()
            Exit Sub
        End If
        Dim track As New Stopwatch
        Dim s As SphereFormula
        Console.WriteLine("Time" & vbTab & "Dim" & vbTab & "Prime" & vbTab & "Formula")
        For i = 1 To dimensions
            Try
                track.Start()
                s = New SphereFormula(i)
                track.Stop()
            Catch
                track.Stop()
                Console.WriteLine("Unable to calculate beyond " & s.Dimensions & " dimensions due to arithmetic limitations.")
                Exit For
            End Try
            Console.WriteLine(track.Elapsed.Seconds & vbTab & i & vbTab & pCache.Last & vbTab & s.ToString)
        Next
        Console.WriteLine("A unit (r=1) " & s.Dimensions & "-sphere has a " & s.Dimensions & "-volume of " & s.nVolume(1))
        Console.WriteLine("If you would like to calculate the " & s.Dimensions & "-volume for a different value of r, enter a floating-point value to use.")
        input = Console.ReadLine
        If String.IsNullOrWhiteSpace(input) Then Exit Sub
        Dim r As Double
        If Double.TryParse(input, r) Then
            Console.WriteLine("With r=" & r & " the " & s.Dimensions & "-volue = " & s.nVolume(r))
        Else
            Console.WriteLine("Unable to parse that value. Will now exit.")
        End If
        Console.ReadKey()
    End Sub
    Public Class SphereFormula
        Private Shared sCache As New List(Of SphereFormula)
        Public Property Dimensions As Integer
        Public Property Pis As Integer = 0
        Public Property Rs As Integer
        Public Property Frac As fraction = 1
        Public Overrides Function ToString() As String
            Dim factors As New List(Of String)
            If Frac <> 1 Then factors.Add(Frac.SimplestForm.ToString)
            If Pis > 0 Then factors.Add(powerString("π", Pis))
            If Rs > 0 Then factors.Add(powerString("r", Rs))
            Return String.Join(" * ", factors)
        End Function
        Private Function powerString(ByVal value As String, ByVal power As Integer) As String
            If power = 1 Then Return value
            Return value & "^" & power
        End Function
        Public Sub New(ByVal d As Integer)
            If d < 1 Then d = 1
            Dimensions = d
            s(Dimensions)
            sCache.Add(Me)
        End Sub
        Public Function nVolume(ByVal r As Double) As Double
            Return Frac.Value * Math.PI ^ Pis * r ^ Rs
        End Function
        Private Sub s(ByVal n As Integer)
            Rs = n
            If n = 1 Then
                Frac = 2
                Pis = 0
            Else
                With sCache(n - 2)
                    Frac = .Frac
                    Pis = .Pis
                End With
                If n Mod 2 = 0 Then
                    Pis += 1
                Else
                    Frac *= 2
                End If
                f(n)
            End If
        End Sub
        Private Shared fCache As New Dictionary(Of Integer, fraction)
        Private Sub f(ByVal n As Integer)
            For i As Integer = n To 2 Step -2
                If Not fCache.ContainsKey(i) Then fCache.Add(i, (New fraction(i - 1, i)).SimplestForm)
                Frac *= fCache(i)
            Next
        End Sub
    End Class
    Public Class fraction
        Private _num As UInt64
        Private _den As UInt64
        Private _simplestForm As fraction = Nothing
        Public ReadOnly Property Numerator As UInt64
            Get
                Return _num
            End Get
        End Property
        Public ReadOnly Property Denominator As UInt64
            Get
                Return _den
            End Get
        End Property
        Public ReadOnly Property Value As Double
            Get
                Return _num / _den
            End Get
        End Property
        ' If this weren't using only positive numbers, I'd include a sign boolean

        Public ReadOnly Property SimplestForm() As fraction
            Get
                If _simplestForm Is Nothing Then
                    If Denominator = 1 OrElse Numerator = 1 Then
                        _simplestForm = Me
                    Else
                        Dim n As UInt64 = Numerator, d As UInt64 = Denominator
                        Dim NumIsMin As Boolean = n <= d
                        If If(NumIsMin, d, n) Mod If(NumIsMin, n, d) = 0 Then
                            If NumIsMin Then
                                d \= n
                                n = 1
                            Else
                                n \= d
                                d = 1
                            End If
                        Else
                            Dim k As Integer = 1
                            Do
                                If n Mod pCache(k) = 0 AndAlso d Mod pCache(k) = 0 Then
                                    n \= pCache(k)
                                    d \= pCache(k)
                                Else
                                    k += 1
                                    If k = pCache.Count Then FindNextPrime(If(NumIsMin, n, d) \ 2)
                                    If k = pCache.Count Then Exit Do
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
        Public Sub New(ByVal n As UInt64, ByVal d As UInt64)
            _num = n
            _den = d
        End Sub
        Public Overrides Function ToString() As String
            Return If(_den = 1, "", "(") & _num.ToString("n0") & If(_den = 1, "", "/" & _den.ToString("n0") & ")")
        End Function
        Public Overloads Shared Operator *(ByVal left As fraction, ByVal right As fraction)
            Dim c1 As New fraction(left.SimplestForm.Numerator, right.SimplestForm.Denominator), c2 As New fraction(right.SimplestForm.Numerator, left.SimplestForm.Denominator)
            Return (New fraction(c1.SimplestForm.Numerator * c2.SimplestForm.Numerator, c1.SimplestForm.Denominator * c2.SimplestForm.Denominator)).SimplestForm
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
    Dim pCache As New List(Of Integer) From {2, 3, 5, 7, 11}
    Sub FindNextPrime(ByVal limit As Integer)
        Static timer As New Stopwatch
        timer.Start()
        Dim n As Integer = pCache.Last
        Do
            n += 2
            If n > limit Then Exit Do
            If Not pCache.AsParallel.Any(Function(p) n Mod p = 0) Then
                pCache.Add(n)
                Exit Do
            End If
        Loop
        timer.Stop()
    End Sub
End Module
