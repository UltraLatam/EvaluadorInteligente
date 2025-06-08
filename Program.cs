// Program.cs
using EvaluadorInteligente.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML;

// ────────────  alias a los tipos generados por el CLI  ────────────
// ▶️  Ajusta el namespace real si tu SentimentModel.*.cs declara otro
using In  = SentimentModel.ConsoleApp.SentimentModel.ModelInput;
using Out = SentimentModel.ConsoleApp.SentimentModel.ModelOutput;
////////////////////////////////////////////////////////////////////////

var builder = WebApplication.CreateBuilder(args);

// 1️⃣  PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(opts =>
    opts.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

// 2️⃣  MVC
builder.Services.AddControllersWithViews();

// 3️⃣  ML.NET (DI)
// ── 3 a) una sola instancia de MLContext
builder.Services.AddSingleton<MLContext>();

// ── 3 b) carga del modelo (*.mlnet) – se resuelve como ITransformer
builder.Services.AddSingleton<ITransformer>(sp =>
{
    var ctx      = sp.GetRequiredService<MLContext>();
    var rootPath = builder.Environment.ContentRootPath;

    // Ruta del archivo generado por el CLI:
    //   ML/SentimentCLI/SentimentModel/SentimentModel.mlnet
    var modelPath = Path.Combine(
        rootPath, "ML", "SentimentCLI", "SentimentModel", "SentimentModel.mlnet");

    return ctx.Model.Load(modelPath, out _);
});

// ── 3 c) PredictionEngine<In,Out> listo para inyectar en controladores
builder.Services.AddSingleton(sp =>
{
    var ctx   = sp.GetRequiredService<MLContext>();
    var model = sp.GetRequiredService<ITransformer>();
    return ctx.Model.CreatePredictionEngine<In, Out>(model);
});

var app = builder.Build();

// 4️⃣  Middleware / Rutas
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
