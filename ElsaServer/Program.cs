using Elsa.EntityFrameworkCore.Extensions;
using Elsa.EntityFrameworkCore.Modules.Management;
using Elsa.EntityFrameworkCore.Modules.Runtime;
using Elsa.Extensions;
using ElsaServer.Activities;
using ElsaServer.Data;
using ElsaServer.Interfaces;
using ElsaServer.Models;
using ElsaServer.Services;
using ElsaServer.Workflows;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("ElsaSqlServer") ??
                       "Data Source=DESKTOP-DPC7CQ2\\SQLEXPRESS;Initial Catalog=ElsaDB;Integrated Security=SSPI;MultipleActiveResultSets=true;TrustServerCertificate=true;";

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

// Add services to the container
builder.Services.AddControllers(); // Ensure this line is present

builder.Services.AddElsa(elsa =>
{
    // Configure Management layer to use EF Core.
    elsa.UseWorkflowManagement(management => management.UseEntityFrameworkCore(options => options.UseSqlServer(connectionString)));

    // Configure Runtime layer to use EF Core.
    elsa.UseWorkflowRuntime(runtime => runtime.UseEntityFrameworkCore(options => options.UseSqlServer(connectionString)));

    elsa.UseIdentity(identity =>
    {
        identity.TokenOptions = options => options.SigningKey = "a08fb9f7b56d7bde4f2f9cd48f3fe5628231e53e0af49769ce4043fd4e474ada";
        identity.UseAdminUserProvider();
    });

    // Configure ASP.NET authentication/authorization.
    elsa.UseDefaultAuthentication(auth => auth.UseAdminApiKey());

    // Expose Elsa API endpoints.
    elsa.UseWorkflowsApi();

    // Setup a SignalR hub for real-time updates from the server.
    elsa.UseRealTimeWorkflows();

    // Enable C# workflow expressions
    elsa.UseCSharp();

    // Enable HTTP activities.
    elsa.UseHttp();

    // Use timer activities.
    elsa.UseScheduling();

    // Register custom activities from the application, if any.
    elsa.AddActivitiesFrom<CustomActivity>();

    // Register custom workflows from the application, if any.
    //elsa.AddWorkflowsFrom<Program>();
    //elsa.AddWorkflow<SaveDynamicDataInDBWorkflow>();
    elsa.AddWorkflow<DocumentApprovalWorkflow>();
});

// Configure CORS to allow designer app hosted on a different origin to invoke the APIs.
builder.Services.AddCors(cors => cors
    .AddDefaultPolicy(policy => policy
        .AllowAnyOrigin() // For demo purposes only. Use a specific origin instead.
        .AllowAnyHeader()
        .AllowAnyMethod()
        .WithExposedHeaders("x-elsa-workflow-instance-id"))); // Required for Elsa Studio in order to support running workflows from the designer. Alternatively, you can use the `*` wildcard to expose all headers.

// Add Health Checks.
builder.Services.AddHealthChecks();

// NEW
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDocumentService, DocumentService>();
builder.Services.AddScoped<CreateUserActivity>();
builder.Services.AddScoped<CustomActivity>();

// NEW
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();

// Build the web application.
var app = builder.Build();

// Configure web application's middleware pipeline.
app.UseCors();
app.UseRouting(); // Required for SignalR.
app.UseAuthentication();
app.UseAuthorization();

// NEW
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers(); // Map API controllers
});

app.UseWorkflowsApi(); // Use Elsa API endpoints.
app.UseWorkflows(); // Use Elsa middleware to handle HTTP requests mapped to HTTP Endpoint activities.
app.UseWorkflowsSignalRHubs(); // Optional SignalR integration. Elsa Studio uses SignalR to receive real-time updates from the server. 

app.MapGet("/", () => "Elsa Workflow API is running.");

app.Run();