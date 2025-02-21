using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;



namespace Backend.Helper
{
	public static class MiddleWare
	{
		public static int GetUserIdFromCookie(HttpRequest request)
		{
			var token = request.Cookies["access"];
			if (string.IsNullOrEmpty(token))
			{
				return -1;
			}

			var tokenHandler = new JwtSecurityTokenHandler();
			var jwtToken = tokenHandler.ReadJwtToken(token);
			var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

			return int.Parse(userId);
		}

	}
}