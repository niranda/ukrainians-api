namespace NomadChat.WebAPI.Middlewares
{
    public class AuthTokenMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthTokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Cookies["Token"];
            if (!string.IsNullOrEmpty(token) && !context.Request.Headers.ContainsKey("Authorization"))
                context.Request.Headers.Add("Authorization", "Bearer " + token);

            await _next(context);
        }
    }
}
