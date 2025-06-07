using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

using Battle.net.WorldOfWarcraft.Models.OAuth;

using Microsoft.Extensions.Configuration;

namespace Battle.net.WorldOfWarcraft;

public class ApiRequestExecutor : IApiRequestExecutor
{
    private static readonly HttpClient client = new();
    private static readonly SemaphoreSlim _semaphore = new(1, 1);
    private static OAuthTokenResponse oAuthToken = new();

    public static void Initialize(Func<IConfigurationSection?> initialiseOAuthToken)
    {
        if (initialiseOAuthToken() is { } config)
        {
            _semaphore.Wait();
            try
            {
                config.Bind(oAuthToken);
                // token lasts one day. set expires in to difference between CreatedAt and now in seconds
                var expiresAt = oAuthToken.CreatedAt.AddDays(1);
                oAuthToken.ExpiresIn = (long)(expiresAt - DateTime.Now).TotalSeconds;
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }

    public async Task<T?> ExecuteAsync<T>(HttpRequestMessage request, CancellationToken cancellationToken = default) where T : new()
    {
        await EnsureOAuthTokenIsValid();
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", oAuthToken.AccessToken);
        var resp = await client.SendAsync(request, cancellationToken);
        if (!resp.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Request failed: {await resp.Content.ReadAsStringAsync(cancellationToken)}", null, resp.StatusCode);
        }
        try
        {
            return await resp.Content.ReadFromJsonAsync<T>(cancellationToken: cancellationToken);
        }
        catch
        {
            File.WriteAllText($"{DateTime.Now:yyyy-MM-dd hh.mm.ss.fff}.json", await resp.Content.ReadAsStringAsync(cancellationToken));
            return default;
        }
    }

    private static async Task EnsureOAuthTokenIsValid()
    {
        if (oAuthToken is null || oAuthToken.IsExpiredOrExpiring())
        {
            await _semaphore.WaitAsync();
            try
            {
                var request = new HttpRequestMessage()
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("https://oauth.battle.net/token", UriKind.Absolute),
                    Content = new MultipartFormDataContent
                    {
                        { new StringContent("client_credentials"), "grant_type" }
                    }
                };
                request.Headers.Authorization = new AuthenticationHeaderValue("basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{Config.ClientId}:{Config.ClientSecret}")));

                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    oAuthToken = (await response.Content.ReadFromJsonAsync<OAuthTokenResponse>())!;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }

    private static StringContent SerializeContent(object requestContent) => new(JsonSerializer.Serialize(requestContent), Encoding.UTF8, "application/json");
}

public interface IApiRequestExecutor
{
    Task<T?> ExecuteAsync<T>(HttpRequestMessage request, CancellationToken cancellationToken = default) where T : new();
}