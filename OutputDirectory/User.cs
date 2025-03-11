using System;
using System.Collections.Generic;

namespace ContactManagementApi.OutputDirectory;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public byte[]? ProfilePhoto { get; set; }

    public virtual ICollection<Contact> Contacts { get; set; } = new List<Contact>();

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
}
