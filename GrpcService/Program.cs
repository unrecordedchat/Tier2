using GrpcService.Services;
using Domain.Managers.User;
using HTTPClient.HTTPUserClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();

// Register the UserServiceImpl
builder.Services.AddScoped<UserServiceImpl>();

// Register the IUserManager and IUserClient implementations
builder.Services.AddScoped<IUserManager, UserManager>();
builder.Services.AddScoped<IUserClient, UserClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<UserServiceImpl>();
app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();