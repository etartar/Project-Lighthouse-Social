using LighthouseSocial.Domain.Interfaces;
using System.Net.Http.Json;

namespace LighthouseSocial.Infrastructure.Auditors;

internal sealed class ExternalCommentAuditor(HttpClient httpClient) : ICommentAuditor
{
    public async Task<bool> IsTextCleanAsync(string text)
    {
        var response = await httpClient.PostAsJsonAsync("https://api.audit/analyze/comment", new { Content = text });

        var result = await response.Content.ReadFromJsonAsync<AuditResult>();

        return result?.IsClean ?? false;
    }
}


public class AuditResult
{
    public bool IsClean { get; set; }
}