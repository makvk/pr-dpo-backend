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
            return Results.Redirect("/index.html");
        });
        app.MapGet("/students", (IStudentCatalogService service) =>
        {
            var students = service.GetAllStudents();
            
            return Results.Json(students);
        });

        app.MapGet("/students/{group}", (string group, IStudentCatalogService service) =>
        {
            var res = service.GetByGroup(group);

            return Results.Json(res);
        });

        app.MapGet("/students/{group}/{id:int}", (string group, int id, IStudentCatalogService service) =>
        {
            Student? res = service.GetByGroupId(group, id);

            if (res == null)
            {
                return Results.NotFound();
            } 
            return Results.Json(res);
        });

        app.MapGet("/reports/{section?}", (string? section) =>
        {
            if (section == null)
            {
                section = "overview";
            }
            return Results.Text(section);
        });

        app.MapGet("/portal/{module=home}/{page=index}/{id:int?}", (string module, string page, int? id) =>
        {
            var msg = $"Module: {module}, Page: {page}, Id: {id}";
            return Results.Text(msg);
        });

        app.MapGet("/files/{**path}", (string? path) =>
        {
            if (path == null)
            {
                return Results.Text("/");
            }
            return Results.Text("/" + path);
        });
    }
}