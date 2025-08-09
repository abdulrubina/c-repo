using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// In-memory list of todos
var todos = new List<string> { "Learn C#", "Build API" };

// GET: Retrieve all todos
app.MapGet("/api/todos", () => todos);

// POST: Add a new todo from JSON body
app.MapPost("/api/todos", async (HttpContext context) =>
{
    var todo = await context.Request.ReadFromJsonAsync<string>();
    if (string.IsNullOrEmpty(todo))
        return Results.BadRequest("Todo cannot be empty");
    todos.Add(todo);
    return Results.Created($"/api/todos/{todos.Count - 1}", todo);
});

// DELETE: Remove a todo by index
app.MapDelete("/api/todos/{index}", (int index) =>
{
    if (index < 0 || index >= todos.Count)
        return Results.NotFound("Todo not found");
    var todo = todos[index];
    todos.RemoveAt(index);
    return Results.Ok($"Deleted: {todo}");
});

app.Run();