using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamProject.Application.Abstracts.Repositories;
using TeamProject.Domain.Entities;
using TeamProject.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace TeamProject.Persistence.Repositories;

public class RefreshTokenRepository:IRefreshTokenRepository
{
    private readonly TeamProjectDbContext _context;

    public RefreshTokenRepository(TeamProjectDbContext context)
    {
        _context = context;
    }

    public async Task<RefreshToken?> GetByTokenWithUserAsync(string token)
    {
        return await _context.RefreshTokens
            .Include(x => x.User) 
            .FirstOrDefaultAsync(x => x.Token == token);
    }

    public async Task AddAsync(RefreshToken refreshToken)
    {
        await _context.RefreshTokens.AddAsync(refreshToken);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteByTokenAsync(string token)
    {
        await _context.RefreshTokens
            .Where(x => x.Token == token)
            .ExecuteDeleteAsync();
    }
}
