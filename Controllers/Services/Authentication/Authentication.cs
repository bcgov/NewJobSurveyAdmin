using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace NewJobSurveyAdmin.Services
{
    public class Authentication
    {
        public static void SetAuthenticationOptions(AuthenticationOptions options)
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }

        public static void SetJwtBearerOptions(
            JwtBearerOptions options, string authority, string audience
        )
        {
            // TODO: Review this section.
            options.Authority = authority;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                // BC Dev Keycloak
                ValidAudiences = new string[]
                {
                    audience, "account", "realm-management"
                },
                RoleClaimType = "role" // Roles in the token for the client.
            };
            options.RequireHttpsMetadata = false; // TODO: Remove?
            options.SaveToken = true;

            options.Validate();
        }

        public static void SetAuthorizationOptions(
            AuthorizationOptions options, string roleName
        )
        {
            options.AddPolicy("UserRole", policy =>
                policy.RequireClaim("role", $"[{roleName}]")
            );
        }
    }
}