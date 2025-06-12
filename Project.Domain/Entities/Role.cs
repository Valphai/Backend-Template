namespace Project.Domain.Entities;

public class Role(string name) : EntityBase
{
    public string Name { get; set; } = name;
    public List<User> Users { get; set; } = new();
}