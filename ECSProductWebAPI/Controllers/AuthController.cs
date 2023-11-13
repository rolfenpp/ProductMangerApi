using System.IdentityModel.Tokens.Jwt;
using ECSProductWebAPI.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ECSProductWebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController: ControllerBase 
{
    private readonly ApplicationDbContext context;
    private readonly IConfiguration config;
    public AuthController(ApplicationDbContext context, IConfiguration config)
    {
        this.context = context;
        this.config = config;
    }

    [HttpPost]
    public ActionResult<TokenDto> Authenticate(AuthenticateRequest authenticateRequest) 
    {
        // 1 - kontrollera om användaren finns (användarnamn + lösenord) 
        var user = context
        .Users
        .FirstOrDefault(x => x.UserName == authenticateRequest.UserName
            && x.Password == authenticateRequest.Password);

        // 1:1 - Om användaren inte finns, returnera 401 Unauthorized

        if (user is null)
        {
            return Unauthorized(); // 401 Unauthorized
        }

        // 1:2 - Om användaren finns, generera token (JWT = JSON Web Token) och returnera denna
        var tokenDto = GenerateToken(); // Token "åkband"
        
        
        return tokenDto;
    }


    private TokenDto GenerateToken() // Token kan innehålla info om användare är admin eller inte
    {
        // Använder signeringsnyckel för att generera en signatur för token
        // skulle användaren manipulera token kommer det upptäckas genom att signaturen förändras.
        var signingKey = Convert.FromBase64String(config["JWT:SigningSecret"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(signingKey),
                SecurityAlgorithms.HmacSha256Signature)
        };
        var jwtTokenHandler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = jwtTokenHandler
        .CreateJwtSecurityToken(tokenDescriptor);

        var token = new TokenDto
        {
            //Generera token
            Token = jwtTokenHandler.WriteToken(jwtSecurityToken)
        };
        return token;
    }
}
public class TokenDto 
{
    public string Token {get; set;}
}
public class AuthenticateRequest
{
    public string UserName {get; set;}
    public string Password {get; set;}
}