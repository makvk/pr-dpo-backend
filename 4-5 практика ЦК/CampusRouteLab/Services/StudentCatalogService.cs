using CampusRouteLab.Models;

namespace CampusRouteLab.Services;

public class StudentCatalogService : IStudentCatalogService
{
    private readonly List<Student> _students = new()
    {
        new Student {Id = 1, Name = "Alice", Group = "ИКБО-30-24"},
        new Student {Id = 2, Name = "Bob", Group = "ИКБО-30-24"},
        new Student {Id = 3, Name = "Charlie", Group = "ИКБО-30-25"},
    };

    public IEnumerable<Student> GetAllStudents() => _students;
    
    public IEnumerable<Student> GetByGroup(string group)
    {
        List<Student> res = new();
        foreach (var student in _students)
        {
            if (student.Group.Equals(group, StringComparison.OrdinalIgnoreCase))
            {
                res.Add(student);
            }
        }
        return res;
    }
    public Student? GetByGroupId(string group, int id)
    {
        IEnumerable<Student> listGroup = GetByGroup(group);

        Student? res = null;

        foreach (var student in listGroup)
        {
            if (student.Id == id)
            {
                res = student;
            }
        }
        return res;
    }
}