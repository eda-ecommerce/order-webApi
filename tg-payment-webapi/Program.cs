var builder = WebApplication.CreateBuilder(args);

// CORS
var OrderAllowSpecificOrigins = "_orderAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: OrderAllowSpecificOrigins,
        builder => {
            builder
                .AllowAnyHeader()
                .AllowAnyMethod();
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
builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.
app.UseCors(OrderAllowSpecificOrigins);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
