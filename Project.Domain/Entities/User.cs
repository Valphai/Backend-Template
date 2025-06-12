namespace Project.Domain.Entities;

public class User(string email, string password) : EntityBase
{
    public string Email { get; set; } = email;
    public string Password { get; set; } = password;
    public List<Role> Roles { get; set; } = new();
    public Guid RefreshToken { get; set; } = Guid.NewGuid();

    public void UpdateUser(User newUser)
    {
        Email = newUser.Email;
        Password = newUser.Password;
        Roles = newUser.Roles;
    }

    public void GenerateRefreshToken() => RefreshToken = Guid.NewGuid();

    public void AddRole(Role role) => Roles.Add(role);
}