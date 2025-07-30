using LighthouseSocial.Application.Common;
using LighthouseSocial.Domain.Interfaces;

namespace LighthouseSocial.Application.Features.Comments.DeleteComment;

public class DeleteCommentHandler(ICommentRepository commentRepository)
{
    public async Task<Result> HandleAsync(Guid commentId)
    {
        var comment = await commentRepository.GetByIdAsync(commentId);

        if (comment == null)
        {
            return Result.Failure("Comment not found");
        }

        await commentRepository.DeleteAsync(commentId);

        return Result.Success();
    }
}
