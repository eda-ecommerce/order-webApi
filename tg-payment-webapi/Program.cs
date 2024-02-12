using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// CORS
var OrderAllowSpecificOrigins = "_orderAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: OrderAllowSpecificOrigins,
        builder => {
            builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});


// Services
builder.Services.AddScoped<IOrderService, OrderService>();

// Repos
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

// Add Mapster Mapping
var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
typeAdapterConfig.Scan(Assembly.GetAssembly(typeof(OrderToOrderDtoRegister)));
// register the mapper as Singleton service for my application
var mapperConfig = new Mapper(typeAdapterConfig);
builder.Services.AddSingleton<IMapper>(mapperConfig);



//DbContext
var sqlstring = "";
if (String.IsNullOrEmpty(Environment.GetEnvironmentVariable("DBSTRING"))) {
    sqlstring = builder.Configuration.GetConnectionString("SqlServer");
} else {
    sqlstring = Environment.GetEnvironmentVariable("DBSTRING");
};
builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseSqlServer(sqlstring)
);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

app.UseCors("CorsPolicy");

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();


// Configure the HTTP request pipeline.
app.UseCors(OrderAllowSpecificOrigins);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// check connection
// using (var scope = app.Services.CreateScope())
// {
//     var context = scope.ServiceProvider.GetService<OrderDbContext>();

//     SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder(sqlstring);
//     sqlConnectionStringBuilder.InitialCatalog = "master";

//     context.Database.SetConnectionString(sqlConnectionStringBuilder.ConnectionString);


//     Console.WriteLine("Waiting for DB connection...");

//     while (!context.Database.CanConnect())
//     {
//         int milliseconds = 2000;
//         Thread.Sleep(milliseconds);
//         // we need to wait, since we need to run migrations
//     }

//     Console.WriteLine("DB connected");

//     context.Database.SetConnectionString(sqlstring);
// }


app.Run();
