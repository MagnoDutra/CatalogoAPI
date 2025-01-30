using System.Text;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;
using API.Context;
using API.DTOs.Mappings;
using API.Extensions;
using API.Filters;
using API.Logging;
using API.Models;
using API.RateLimitOptions;
using API.Repositories;
using API.Services;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(ApiExceptionFilter));
}).AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
}).AddNewtonsoftJson();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
                            {
                                policy.WithOrigins("http://www.apirequest.io", "https://magno.seila")
                                .AllowAnyHeader()
                                .AllowAnyMethod()
                                .AllowCredentials();
                            });
    options.AddPolicy("PoliticaCORS", policy => policy.WithOrigins("https//teste.com").WithMethods("GET"));
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "apicatalogo", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Bearer JWT "
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme{
                Reference = new OpenApiReference{
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

var secretKey = builder.Configuration["JWT:SecretKey"] ?? throw new ArgumentException("Invalid secret key!");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("SuperAdminOnly", policy => policy.RequireRole("Admin").RequireClaim("id", "magno"));
    options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
    options.AddPolicy("ExclusiveOnly",
                    policy => policy.RequireAssertion(context =>
                             context.User.HasClaim(claim => claim.Type == "id" &&
                                                            claim.Value == "magno") ||
                                                            context.User.IsInRole("SuperAdmin")));
});

var myOptions = new MyRateLimitOptions();

builder.Configuration.GetSection(MyRateLimitOptions.MyRateLimit).Bind(myOptions);

builder.Services.AddRateLimiter(rateLimiterOptions =>
{
    rateLimiterOptions.AddFixedWindowLimiter(policyName: "fixedwindow", options =>
    {
        options.PermitLimit = myOptions.PermitLimit;
        options.Window = TimeSpan.FromSeconds(myOptions.Window);
        options.QueueLimit = myOptions.QueueLimit;
    });
    rateLimiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});

// builder.Services.AddRateLimiter(options =>
// {
//     options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

//     options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpcontext =>
//                             RateLimitPartition.GetFixedWindowLimiter(partitionKey: httpcontext.User.Identity?.Name ??
//                                                                     httpcontext.Request.Headers.Host.ToString(),
//                                                                     factory: partition => new FixedWindowRateLimiterOptions
//                                                                     {
//                                                                         AutoReplenishment = myOptions.AutoReplenishment,
//                                                                         PermitLimit = myOptions.PermitLimit,
//                                                                         QueueLimit = myOptions.QueueLimit,
//                                                                         Window = TimeSpan.FromSeconds(myOptions.Window)
//                                                                     }));
// });

builder.Services.AddApiVersioning(o =>
{
    o.DefaultApiVersion = new ApiVersion(1, 0); // define a versao padrao se nenhuma for especificada no request
    o.AssumeDefaultVersionWhenUnspecified = true; // Se a versao n for especificada a versao padrao eh utilizada
    o.ReportApiVersions = true; // as versoes da api devem ser incluidas no header do response.
    o.ApiVersionReader = ApiVersionReader.Combine(new QueryStringApiVersionReader(), new UrlSegmentApiVersionReader());
}).AddApiExplorer(options =>
{   // configuracoes para a documentacao no swagger
    options.GroupNameFormat = "'v'VVV"; // vai ter o caractere v e o numero da versao em seguida
    options.SubstituteApiVersionInUrl = true;
});

string? mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(mySqlConnection, ServerVersion.AutoDetect(mySqlConnection)));

builder.Services.AddTransient<IMeuServico, MeuServico>();
builder.Services.AddScoped<ApiLoggingFilter>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddAutoMapper(typeof(ProdutoDTOMappingProfile));

builder.Logging.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfiguration { LogLevel = LogLevel.Information }));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ConfigureExceptionHandler();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseRateLimiter();
app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
