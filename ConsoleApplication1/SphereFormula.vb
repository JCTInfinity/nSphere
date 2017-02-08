﻿Public Class SphereFormula
    Private Shared sCache As New List(Of SphereFormula)
    Public Property Dimensions As UInt16
    Public Property Pis As Integer = 0
    Public Property Rs As Integer
    Public Property Frac As fraction = 1
    Public Overrides Function ToString() As String
        Dim factors As New List(Of String)
        If Frac <> 1 Then factors.Add(Frac.SimplestForm.ToString)
        If Pis > 0 Then factors.Add(powerString("π", Pis))
        factors.Add(powerString("r", Rs))
        Return String.Join(" * ", factors)
    End Function
    Private Function powerString(ByVal value As String, ByVal power As Integer) As String
        If power = 1 Then Return value
        Return value & "^" & power
    End Function
    Private Sub New(ByVal d As UInt16)
        Dimensions = d
        Rs = d
        If d > 0 Then
            With sCache(d - 1)
                Frac = .Frac
                Pis = .Pis
            End With
            If d Mod 2 = 0 Then
                Pis += 1
            Else
                Frac *= 2
            End If
            f(d)
        End If
    End Sub
    Public Shared Function ForDimension(ByVal dimensions As UInt16) As SphereFormula
        If sCache.Count - 1 < dimensions Then
            For i As UInt16 = sCache.Count To dimensions
                sCache.Add(New SphereFormula(i))
            Next
        End If
        Return sCache(dimensions)
    End Function
    Public Function nVolume(ByVal r As Double) As Double
        Return Frac.Value * Math.PI ^ Pis * r ^ Rs
    End Function
    Private Shared fCache As New Dictionary(Of Integer, fraction)
    Private Sub f(ByVal n As Integer)
        For i As Integer = n To 2 Step -2
            If Not fCache.ContainsKey(i) Then fCache.Add(i, (New fraction(i - 1, i)).SimplestForm)
            Frac *= fCache(i)
        Next
    End Sub
End Class