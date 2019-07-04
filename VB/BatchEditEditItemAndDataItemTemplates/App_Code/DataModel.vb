Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web

Public NotInheritable Class DataModel

	Private Sub New()
	End Sub
	Private Const ProductsSessionKey As String = "products"

	Public Shared Function GetProducts() As List(Of Product)
		If HttpContext.Current.Session(ProductsSessionKey) Is Nothing Then
			GenerateProducts()
		End If
		Return CType(HttpContext.Current.Session(ProductsSessionKey), List(Of Product))
	End Function
	Public Shared Sub UpdateProducts(ByVal product As Product)
		Dim products As List(Of Product) = GetProducts()
		Dim index As Integer = products.FindIndex(Function(p) p.ID = product.ID)
		products(index) = product
		SaveProducts(products)
	End Sub
	Public Shared Sub InsertProduct(ByVal product As Product)
		Dim products As List(Of Product) = GetProducts()
		product.ID = products.Count
		products.Add(product)
		SaveProducts(products)
	End Sub
	Public Shared Sub RemoveProduct(ByVal product As Product)
		Dim products As List(Of Product) = GetProducts()
		products.RemoveAll(Function(p) p.ID = product.ID)
		SaveProducts(products)
	End Sub
	Public Shared Sub GenerateProducts()
		Dim products As New List(Of Product)()
		For j As Integer = 0 To 9
			For i As Integer = 0 To 4
				Dim ratingValue As Integer = If(j Mod 2 = 0, i + 1, 5 - i)
				products.Add(New Product() With {.ID = j * 5 + i, .RatingValue = ratingValue})
			Next i
		Next j
		HttpContext.Current.Session(ProductsSessionKey) = products
	End Sub
	Private Shared Sub SaveProducts(ByVal products As List(Of Product))
		HttpContext.Current.Session(ProductsSessionKey) = products
	End Sub
End Class
Public Class Product
	Public Property ID() As Integer
	Public Property RatingValue() As Integer
End Class