using Microsoft.EntityFrameworkCore;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// 1. Configuração do banco de dados
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Configuração do MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllUsersQuery).Assembly));

// 3. Configuração do AutoMapper
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<UserProfile>(), typeof(UserProfile).Assembly);

// 4. Configuração do FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

// 5. Configurações padrão do Web API
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build(); // ← TODAS as configurações devem vir ANTES desta linha

// Configuração do pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();