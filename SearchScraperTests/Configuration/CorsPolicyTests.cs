using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace SearchScraperTests.Configuration
{
    public class CorsPolicyTests
    {
        [Fact]
        public async Task CorsPolicy_AllowsAnyOrigin()
        {
            var hostBuilder = new WebHostBuilder()
                .ConfigureServices(services =>
                {
                    services.AddCors(options =>
                    {
                        options.AddPolicy("AllowSpecificOrigins",
                            policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
                    });
                })
                .Configure(app =>
                {
                    app.UseCors("AllowSpecificOrigins");
                    app.Map("/some-endpoint", builder =>
                    {
                        builder.Run(async context =>
                        {
                            await context.Response.WriteAsync("CORS Test");
                        });
                    });
                });

            using var testServer = new TestServer(hostBuilder);
            var client = testServer.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Options, "/some-endpoint");
            request.Headers.Add("Origin", "http://example.com");

            var response = await client.SendAsync(request);
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
