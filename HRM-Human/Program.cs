using Microsoft.EntityFrameworkCore;
using ASP;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<EmpDb>(opt => opt.UseInMemoryDatabase("ASP"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

app.MapGet("/empitems", async (EmpDb db) =>
    await db.Mems.ToListAsync()
);

app.MapGet("/empitems/complete", async (EmpDb db) =>
    await db.Mems.Where(t => t.IsComple).ToListAsync()
);

app.MapGet("/empitems/search/{name}", async (string name, EmpDb db) =>
{
    var emps = await db.Mems
        .Where(e => e.Name!.Contains(name))
        .ToListAsync();

    if (emps is null || emps.Count == 0) return Results.NotFound();

    return Results.Ok(emps);
});

app.MapGet("/empitems/{id}", async (int id, EmpDb db) =>
    await db.Mems.FindAsync(id)
        is Employee emp
            ? Results.Ok(emp)
            : Results.NotFound()
);

app.MapPost("/empitems", async (Employee emp, EmpDb db) =>
{
    db.Mems.Add(emp);
    await db.SaveChangesAsync();

    return Results.Created($"/empitems/{emp.Id}", emp);
});

app.MapPut("/empitems/{id}", async (int id, Employee inputEmp, EmpDb db) =>
{
    var emp = await db.Mems.FindAsync(id);

    if (emp is null) return Results.NotFound();

    emp.Name = inputEmp.Name;
    emp.IsComple = inputEmp.IsComple;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/empitems/{id}", async (int id, EmpDb db) =>
{
    if (await db.Mems.FindAsync(id) is Employee emp)
    {
        db.Mems.Remove(emp);
        await db.SaveChangesAsync();
        return Results.Ok(emp);
    }

    return Results.NotFound();
});

app.Run();