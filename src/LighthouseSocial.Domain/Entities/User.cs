﻿using LighthouseSocial.Domain.Common;

namespace LighthouseSocial.Domain.Entities;

public class User : EntityBase
{
    protected User() { }

    public User(string fullname, string email)
    {
        Fullname = fullname;
        Email = email;
    }

    public string Fullname { get; set; }
    public string Email { get; set; }

    public List<Photo> Photos { get; } = [];
    public List<Comment> Comments { get; } = [];
}
