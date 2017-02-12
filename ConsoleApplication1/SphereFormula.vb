Public Class SphereFormula
    Public Shared sCache As SphereFormula
    Public Property Dimensions As UInt16
    Public Property πs As UInt16 = 0
    Public Property Rs As UInt16
    Public Property Frac As fraction = 1
    Public Overloads Function ToString(ByVal FullFraction As Boolean) As String
        Dim factors As New List(Of String)
        If Frac <> 1 Then factors.Add(Frac.SimplestForm.ToString(FullFraction))
        If πs > 0 Then factors.Add(powerString("π", πs))
        factors.Add(powerString("r", Rs))
        Return String.Join(" * ", factors)
    End Function
    Public Overrides Function ToString() As String
        Return ToString(False)
    End Function
    Private Function powerString(ByVal value As String, ByVal power As Integer) As String
        If power = 1 Then Return value
        Return value & "^" & power
    End Function
    Private Sub New(ByVal d As UInt16)
        Dimensions = d
        Rs = d
        If d > 0 Then
            Frac = sCache.Frac
            πs = d \ 2
            If d Mod 2 <> 0 Then Frac *= 2
            f(d)
        End If
    End Sub
    Public Shared Function ForDimension(ByVal dimensions As UInt16) As SphereFormula
        If sCache Is Nothing OrElse sCache.Dimensions > dimensions Then sCache = New SphereFormula(0)
        If sCache.Dimensions < dimensions Then
            For i As UInt16 = sCache.Dimensions + 1 To dimensions
                sCache = New SphereFormula(i)
            Next
        End If
        Return sCache
    End Function
    Public Function nVolume(ByVal r As Double) As Double
        Return Math.Exp(nVolumeExp(r))
    End Function
    Public Function nVolumeExp(ByVal r As Double) As Double
        Return Frac.ExpValue + Math.Log(Math.PI) * πs + Math.Log(r) * Rs
    End Function
    Private Shared fCache As New Dictionary(Of Integer, fraction)
    Private Sub f(ByVal n As Integer)
        For i As Integer = n To 2 Step -2
            If Not fCache.ContainsKey(i) Then fCache.Add(i, (New fraction(i - 1, i)).SimplestForm)
            Frac *= fCache(i)
        Next
    End Sub
End Class
