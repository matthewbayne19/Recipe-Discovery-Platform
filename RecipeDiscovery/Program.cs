using RecipeDiscovery.GraphQL;
using RecipeDiscovery.Services;
using HotChocolate;

var builder = WebApplication.CreateBuilder(args);

// Register the services
builder.Services.AddHttpClient<IRecipeService, RecipeService>();
builder.Services.AddSingleton<Query>();

// Configure GraphQL
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>();  // Register Query type

var app = builder.Build();

app.UseHttpsRedirection();
app.MapGraphQL();  // Maps the GraphQL endpoint

app.Run();
