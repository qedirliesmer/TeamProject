using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Experimental;
using System.Text;
using TeamProject.Application.Options;

namespace TeamProject.WebApi.Options;

public class ConfigureJwtBearerOptions : IConfigureOptions<JwtBearerOptions>
{
    private readonly JwtOptions _jwtOptions;

    public ConfigureJwtBearerOptions(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }

    public void Configure(JwtBearerOptions options)
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, 
            ValidateAudience = true,  
            ValidateLifetime = true, 
            ValidateIssuerSigningKey = true,

            ValidIssuer = _jwtOptions.Issuer, 
           ValidAudience = _jwtOptions.Audience, 
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret)),  

            ClockSkew = TimeSpan.Zero
        };
    }
}
