﻿using Dapper;
using LighthouseSocial.Domain.Entities;
using LighthouseSocial.Domain.Interfaces;

namespace LighthouseSocial.Data.Repositories;

internal sealed class UserRepository(IDbConnectionFactory connFactory) : IUserRepository
{
    private readonly IDbConnectionFactory _connFactory = connFactory;

    public async Task<User> GetByIdAsync(Guid userId)
    {
        const string sql = "SELECT id, external_id, full_name, email, joined_at FROM users WHERE id = @Id";

        using var conn = _connFactory.CreateConnection();
        var user = await conn.QuerySingleAsync<User>(sql, new { Id = userId });

        return user;
    }
}