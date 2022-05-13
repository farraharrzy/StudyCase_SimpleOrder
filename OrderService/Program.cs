using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OrderService.GraphQL;
using OrderService.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//Database
var conString = builder.Configuration.GetConnectionString("MyDatabase");
builder.Services.AddDbContext<StudyCase_SimpleOrderContext>(options =>
     options.UseSqlServer(conString)
);

//Kafka
builder.Services.Configure<KafkaSettings>(builder.Configuration.GetSection("KafkaSettings"));

// graphql
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddAuthorization();

//JWT Token
// DI Dependency Injection
builder.Services.Configure<TokenSettings>(builder.Configuration.GetSection("TokenSettings"));

// role-based identity
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = builder.Configuration.GetSection("TokenSettings").GetValue<string>("Issuer"),
            ValidateIssuer = true,
            ValidAudience = builder.Configuration.GetSection("TokenSettings").GetValue<string>("Audience"),
            ValidateAudience = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("TokenSettings").GetValue<string>("Key"))),
            ValidateIssuerSigningKey = true
        };

    });




builder.Services.AddCors(options =>
{
    options.AddPolicy("allowedOrigin", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
}
);




var app = builder.Build();

app.UseAuthentication();

app.MapGraphQL();
app.MapGet("/", () => "Hello World!");

app.Run();
