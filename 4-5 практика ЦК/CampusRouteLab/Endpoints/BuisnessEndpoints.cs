using CampusRouteLab.Models;
using CampusRouteLab.Services;
using System.Text.Json;
using System.Text.Encodings.Web;

namespace CampusRouteLab.Endpoints;

public static class BuisnessEndpoints
{
    public static void MapBuisnessEndpoints(this WebApplication app)
    {
        app.MapGet("/", () =>
        {
            return Results.Redirect("/swagger");
        });
        app.MapGet("/students", (IStudentCatalogService service) =>
        {
            var students = service.GetAllStudents();
            
            return Results.Ok(students);
        });

        app.MapGet("/students/{group}", (string group, IStudentCatalogService service) =>
        {
            var res = service.GetByGroup(group);

            return Results.Ok(res);
        });

        app.MapGet("/students/{group}/{id}", (string group, int id, IStudentCatalogService service) =>
        {
            var res = service.GetByGroupId(group, id);

            if (res == null)
            {
                return Results.NotFound();
            } 
            return Results.Ok(res);
        });
    }
}