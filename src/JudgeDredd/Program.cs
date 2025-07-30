using JudgeDredd.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddScoped<ICommentAuditService, OpenAICommentAuditService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapPost("/moderate", async (ModerateRequest request, ICommentAuditService commentAuditService) =>
{
    bool isFlagged = await commentAuditService.IsFlagged(request.Comment);

    return Results.Ok(new
    {
        isFlagged
    });
});

app.Run();
