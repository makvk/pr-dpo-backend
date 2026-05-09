using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.Routing;
using TaskManager.Models;
using Microsoft.OpenApi;

namespace TaskManager.Endpoints;

public static class JsonEndpoints
{
    private static List<TaskModel> ReadAllTasks(string filePath)
    {
        if (!File.Exists(filePath)) return new List<TaskModel>();
        var json = File.ReadAllText(filePath);

        return JsonSerializer.Deserialize<List<TaskModel>>(json) ?? new List<TaskModel>();
    }
    private static void WriteAllTasks(List<TaskModel> tasks, string filePath)
    {
        var options = new JsonSerializerOptions 
        { 
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        };
        var json = JsonSerializer.Serialize(tasks, options);

        File.WriteAllText(filePath, json);
    }
    public static void MapJsonEndpoints(this WebApplication app)
    {
        app.MapGet("api/tasks", (IWebHostEnvironment env) =>
        {
            var root = env.ContentRootPath;
            var filePath = Path.Combine(root, "Data", "tasks.json");

            var tasks = ReadAllTasks(filePath);

            return Results.Json(tasks);
        }).WithName("GetAllTasks").WithDescription("Возвращает список всех задач");

        app.MapGet("api/tasks/{id}", (int id, IWebHostEnvironment env) =>
        {
            var root = env.ContentRootPath;
            var filePath = Path.Combine(root, "Data", "tasks.json");

            var tasks = ReadAllTasks(filePath);

            var task = tasks.FirstOrDefault(t => t.id == id);
            if (task == null) return Results.NotFound();

            return Results.Json(task);
            
        }).WithName("GetById").WithDescription("Возвращает задачу по Id");

        app.MapGet("/api/tasks/completed", (IWebHostEnvironment env) =>
        {
            var root = env.ContentRootPath;
            var filePath = Path.Combine(root, "Data", "tasks.json");

            var tasks = ReadAllTasks(filePath);

            var message = new List<TaskModel>();
            foreach (var t in tasks)
            {
                if (t.isCompleted)
                {
                    message.Add(t);
                }
            }
            return Results.Json(message);
        }).WithName("GetCompleted").WithDescription("Возвращает только завершенные задачи");

        app.MapPost("api/tasks", (TaskCreateModel model, IWebHostEnvironment env) =>
        {
            var root = env.ContentRootPath;
            var filePath = Path.Combine(root, "Data", "tasks.json");

            var tasks = ReadAllTasks(filePath);

            var currId = tasks.Last().id + 1; 

            var task = new TaskModel
            {
                id = currId,
                title = model.title,
                description = model.description,
                isCompleted = false,
            };
            tasks.Add(task);

            WriteAllTasks(tasks, filePath);

            return Results.Ok($"Задача успешно создана, ее id: {task.id}");
        }).WithName("PostTask").WithDescription("Создание новой задачи");

        app.MapPut("api/tasks/{id}", (int id, TaskUpdateModel model, IWebHostEnvironment env) =>
        {
            var root = env.ContentRootPath;
            var filePath = Path.Combine(root, "Data", "tasks.json");

            var tasks = ReadAllTasks(filePath);
            var task = tasks.FirstOrDefault(t => t.id == id);

            if (task == null)
                return Results.NotFound($"Задача с id {id} не найдена");

            task.title = model.title;
            task.description = model.description;
            task.isCompleted = model.isCompleted;

            WriteAllTasks(tasks, filePath);

            return Results.Ok(task);
            
        }).WithName("UpdateTaskBuId").WithDescription("Обновление задачи по id");

        app.MapDelete("api/tasks/{id}", (int id, IWebHostEnvironment env) =>
        {
            var root = env.ContentRootPath;
            var filePath = Path.Combine(root, "Data", "tasks.json");

            var tasks = ReadAllTasks(filePath);
            var isDeleted = false;
            TaskModel? deleteTask = null;
            foreach (var t in tasks)
            {
                if (!isDeleted) {
                    if (t.id == id)
                    {
                        isDeleted = true;
                        deleteTask = t;
                    }
                }
                else
                {
                    t.id -= 1;
                }
            }
            if (deleteTask != null) {
                tasks.Remove(deleteTask);
                WriteAllTasks(tasks, filePath);
                return Results.Ok($"Задача {deleteTask.title}, c id {deleteTask.id} успешно удалена");
            }
            return Results.NotFound("Task not found");
        }).WithName("DeleteTaskById").WithDescription("Удаление задачи по id");
    }
}