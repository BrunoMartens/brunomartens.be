using Yarp.ReverseProxy.Transforms;

var builder = WebApplication.CreateBuilder(args);

if(!builder.Environment.IsDevelopment())
{
    builder.Services.AddLettuceEncrypt();

    builder.WebHost.ConfigureKestrel(kestrel =>
    {
        kestrel.ListenAnyIP(8081, portOptions =>
        {
            portOptions.UseHttps(h =>
            {
                h.UseLettuceEncrypt(kestrel.ApplicationServices);
            });
        });
    });
}

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
    /*.AddTransforms(transformBuilderContext =>
    {
        transformBuilderContext.AddRequestTransform(async transformContext => {

            if (string.Equals("authPolicy", transformBuilderContext.Route.AuthorizationPolicy))
            {
                var tokenService = new TokenService();
                var token = await tokenService.GetToken(oidcConfig["ClientId"], oidcConfig["ClientSecret"], "api://brunomartens.be/.default", oidcConfig["TenantId"]);

                var headers = transformContext.ProxyRequest.Headers;
                headers.Add("Authorization", $"Bearer {token}");
            }
        });
    });*/

var app = builder.Build();

app.MapReverseProxy();

app.Run();
