using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProEventos.API.Config;
using ProEventos.API.Contexto;
using ProEventos.API.Persistence.Contratos;
using ProEventos.API.Persistence;
using ProEventos.API.Services;
using ProEventos.API.Services.Impl;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Configuração do DATABASE
builder.Services.AddDbContext<ProEventosContext>(context => context.UseMySql(
   "server=localhost;DataBase=ProEventosDB;uid=root;pwd=1234",
   Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.28"))
);

//AutoMapper - Possível Problemas!!!
//IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
//builder.Services.AddSingleton(mapper);
//builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//Injeção de dependência
builder.Services.AddScoped<IEventoService, EventoService>();
builder.Services.AddScoped<IGeralPersist, GeralPersist>();
builder.Services.AddScoped<IEventoPersist, EventoPersist>();

builder.Services.AddControllers()
                .AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling =
                    Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors(cors => cors.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.MapControllers();

app.Run();
