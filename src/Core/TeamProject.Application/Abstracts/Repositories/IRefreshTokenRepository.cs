using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamProject.Domain.Entities;

namespace TeamProject.Application.Abstracts.Repositories;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> GetByTokenWithUserAsync(string token);

    Task AddAsync(RefreshToken refreshToken);

    Task DeleteByTokenAsync(string token);
}
