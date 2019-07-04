using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public static class DataModel {
    private const string ProductsSessionKey = "products";

    public static List<Product> GetProducts() {
        if (HttpContext.Current.Session[ProductsSessionKey] == null)
            GenerateProducts();
        return (List<Product>)HttpContext.Current.Session[ProductsSessionKey];
    }
    public static void UpdateProducts(Product product) {
        List<Product> products = GetProducts();
        int index = products.FindIndex(p => p.ID == product.ID);
        products[index] = product;
        SaveProducts(products);
    }
    public static void InsertProduct(Product product) {
        List<Product> products = GetProducts();
        product.ID = products.Count;
        products.Add(product);
        SaveProducts(products);
    }
    public static void RemoveProduct(Product product) {
        List<Product> products = GetProducts();
        products.RemoveAll(p => p.ID == product.ID);
        SaveProducts(products);
    }
    public static void GenerateProducts() {
        List<Product> products = new List<Product>();
        for (int j = 0; j < 10; j++) {
            for (int i = 0; i < 5; i++) {
                int ratingValue = j % 2 == 0 ? i + 1 : 5 - i;
                products.Add(new Product() { ID = j * 5 + i, RatingValue = ratingValue });
            }
        }
        HttpContext.Current.Session[ProductsSessionKey] = products;
    }
    private static void SaveProducts(List<Product> products) {
        HttpContext.Current.Session[ProductsSessionKey] = products;
    }
}
public class Product {
    public int ID { get; set; }
    public int RatingValue { get; set; }
}