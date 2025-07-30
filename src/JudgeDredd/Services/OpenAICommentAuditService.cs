using OpenAI.Moderations;

namespace JudgeDredd.Services;

internal sealed class OpenAICommentAuditService(IConfiguration configuration) : ICommentAuditService
{
    private readonly string _apiKey = configuration.GetValue<string>("OpenAI:ModerationApiKey") ?? string.Empty;

    public async Task<bool> IsFlagged(string comment)
    {
        if (string.IsNullOrWhiteSpace(comment))
        {
            return false;
        }

        ModerationClient client = new("omni-moderation-latest", _apiKey);

        var response = await client.ClassifyTextAsync(comment);

        return !response.Value.Flagged;
    }
}


public class ModerateRequest
{
    public string Comment { get; set; }
}