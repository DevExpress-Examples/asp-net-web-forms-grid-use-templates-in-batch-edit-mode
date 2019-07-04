Imports System
Imports System.Collections.Generic
Imports System.Collections.Specialized
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls

Partial Public Class _Default
	Inherits System.Web.UI.Page

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
		If (Not IsPostBack) AndAlso (Not IsCallback) Then
			Session.Clear()
		End If
	End Sub
End Class
