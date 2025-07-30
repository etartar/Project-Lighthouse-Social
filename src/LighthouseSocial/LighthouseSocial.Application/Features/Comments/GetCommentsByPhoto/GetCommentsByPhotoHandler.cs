using LighthouseSocial.Application.Common;
using LighthouseSocial.Application.Dtos;
using LighthouseSocial.Domain.Interfaces;

namespace LighthouseSocial.Application.Features.Comments.GetCommentsByPhoto;

public class GetCommentsByPhotoHandler(ICommentRepository commentRepository)
{
    public async Task<Result<IEnumerable<CommentDto>>> HandleAsync(Guid photoId)
    {
        var comments = await commentRepository.GetByPhotoIdAsync(photoId);

        var dtos = comments.Select(c => new CommentDto(c.UserId, c.PhotoId, c.Text, c.Rating.Value));

        return Result<IEnumerable<CommentDto>>.Success(dtos);
    }
}
