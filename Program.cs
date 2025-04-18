using Microsoft.AspNetCore.Mvc;
using System.Data;
using WebApplication1;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
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


app.MapGet("/users{id}", (int id) =>
{
    try
    {
        dbConnection db = new dbConnection();
        DataRow users = db.GetInfoForTable("users", id);

        if (users == null)
        {
            return Results.NotFound("Пользователь с таким ID несушествует.");
        }

        var result = new Dictionary<string, object>();

        foreach (DataColumn col in users.Table.Columns)
        {
            result[col.ColumnName] = users[col];
        }

        return Results.Ok(result);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
}).WithTags("User");

app.MapPatch("/users{id}", ([FromBody] UserUpdate user, int id) =>
{
    try
    {
        dbConnection db = new dbConnection();
        DataRow users = db.GetInfoForTable("users", id);

        if (users == null)
        {
            return Results.NotFound("Пользователь с таким ID несушествует.");
        }

        string name = user.name;
        string lastName = user.lastName;

        if (name == "string")
            name = users["name"].ToString();
        if (lastName == "string")
            lastName = users["lastName"].ToString();

        db.UpdateUsers(name, lastName, id);

        return Results.NoContent();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
}).WithTags("User");

app.MapDelete("/users{id}", (int id) =>
{
    try
    {
        dbConnection db = new dbConnection();
        DataRow users = db.GetInfoForTable("users", id);

        if (users == null)
        {
            return Results.NotFound("Пользователь с таким ID несушествует.");
        }

        db.RemoveUsers(id);
        return Results.NoContent();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
}).WithTags("User");

app.MapPost("/users{id}", ([FromBody] User user) =>
{
    try
    {
        dbConnection db = new dbConnection();

        db.AddUsers(user.name, user.lastName);

        return Results.NoContent();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
}).WithTags("User");


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


public class User
{
    public string name { get; set; }
    public string lastName { get; set; }
}

public class UserUpdate : User
{
    public int id { get; set; }
}

