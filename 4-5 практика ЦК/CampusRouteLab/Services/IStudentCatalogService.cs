using CampusRouteLab.Models;

namespace CampusRouteLab.Services;

public interface IStudentCatalogService
{
    IEnumerable<Student> GetAllStudents();
    IEnumerable<Student> GetByGroup(string group);
    Student? GetByGroupId(string group, int id);
}