using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapPost("/products", (Product product) =>{
    ProductRepository.Add(product);
    return Results.Created($"/products/{product.Code}", product.Code);
});

// api.start.com/getproduct/{code}
app.MapGet("/products/{code}", ([FromRoute] string code) =>{
    var product = ProductRepository.GetBy(code);
    if(product != null)
        return Results.Ok(product);
    return Results.NotFound();
});

app.MapPut("/products", (Product product)=>{
    var productSaved = ProductRepository.GetBy(product.Code);
    productSaved.Name = product.Name;
    return Results.Ok();
});

app.MapDelete("/products/{code}", ([FromRoute] string code)=>{
    var productDel = ProductRepository.GetBy(code);
    ProductRepository.Remove(productDel);
    return Results.Ok();
});

app.Run();


public static class ProductRepository{
    public static List<Product> Products { get; set; }

    public static void Add(Product product){
        if(Products == null)
            Products = new List<Product>();
        
        Products.Add(product);
    }

    public static Product GetBy(string code){
        return Products.FirstOrDefault(p => p.Code == code);
    }

    public static void Remove(Product product){
        Products.Remove(product);
    }
}

public class Product{
    public string Code { get; set;}
    public string Name { get; set;}
}

// ----------------------- ANOTAÇÕES ----------------------------------
// app.MapGet("/AddHeader", (HttpResponse response) => {
//     response.Headers.Add("Teste", "Claudiney Dutra");
//     return new {Name= "Claudiney Dutra", Age=19};
// });

// // api.start.com/getproduct?datastart={data}&dataend={data}
// app.MapGet("/getproduct", ([FromQuery] string dataStart, [FromQuery] string dataEnd) =>{
//     return dataStart + " + " + dataEnd;
// });

// //Parametros pelo header
// app.MapGet("/getproductbyheader", (HttpRequest request)=>{
//     return request.Headers["product-code"].ToString();
// });
