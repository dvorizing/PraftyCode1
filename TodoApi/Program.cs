using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<Item>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<ToDoDbContext>();
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
});
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseCors("CorsPolicy");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSwagger(options =>
{
    options.SerializeAsV2 = true;
});

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

app.MapGet("/todoitems", async (ToDoDbContext db) =>
    await db.Items.ToListAsync());
// Post
app.MapPost("/todoitems", async ([FromBody]Item item, ToDoDbContext db) =>
{
    db.Items.Add(item);
    await db.SaveChangesAsync();

    return Results.Created($"/todoitems/{item.IdItem}", item);
});
// put
app.MapPut("/todoitems/{item.IdItem}", async (int id,Item item, ToDoDbContext db) =>
{
    var todo = await db.Items.FindAsync(id);

    if (todo is null) return Results.NotFound();
    todo.Name = item.Name;
    todo.IdItem=item.IdItem;
    todo.IsComplete = item.IsComplete;
    await db.SaveChangesAsync();

    return Results.NoContent();
});
// delete
app.MapDelete("/todoitems/{id}", async (int id, ToDoDbContext db) =>
{
    var item = await db.Items.FindAsync(id);

    if (item is null )
     return Results.NotFound();
    
        db.Items.Remove(item);
        await db.SaveChangesAsync();
        return Results.Ok(item);
    
   
});
app.Run();
