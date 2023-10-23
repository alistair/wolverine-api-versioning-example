using Wolverine;
using Wolverine.Http;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddApiVersioning(setup => {
    setup.AssumeDefaultVersionWhenUnspecified = true;
    setup.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
})
.AddApiExplorer()
.EnableApiVersionBinding();



builder.Services.AddSwaggerGen();

builder.Host.UseWolverine(_ => {
    _.OptimizeArtifactWorkflow();
});

builder.Services.ConfigureSystemTextJsonForWolverineOrMinimalApi(o =>
{
    // Do whatever you want here to customize the JSON
    // serialization
    o.SerializerOptions.WriteIndented = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var orders = app.NewVersionedApi( "Orders" );
var ordersV1 = orders.MapGroup("/api/orders")
    .HasApiVersion( 1.0 );

ordersV1.MapGet( "/{id:int}", ( int id ) => new OrderV1(id, "John Doe") )
        .Produces<OrderV1>()
        .Produces( 404 );

var ordersV2 = orders.MapGroup("/api/orders")
    .HasApiVersion( 2.0 );

ordersV2.MapGet( "/{id:int}", ( int id ) => new OrderV2(id, "John Doe 2", "Blah") )
        .Produces<OrderV2>()
        .Produces( 404 );

// If you uncomment this then there are no
app.MapWolverineEndpoints();

app.Run();


public record OrderV1(int Id, string Customer);
public record OrderV2(int Id, string Customer, string Blah);
