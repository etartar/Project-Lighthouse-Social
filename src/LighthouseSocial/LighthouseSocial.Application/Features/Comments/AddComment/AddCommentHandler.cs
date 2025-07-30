using FluentValidation;
using LighthouseSocial.Application.Common;
using LighthouseSocial.Application.Dtos;
using LighthouseSocial.Domain.Entities;
using LighthouseSocial.Domain.Interfaces;
using LighthouseSocial.Domain.ValueObjects;

namespace LighthouseSocial.Application.Features.Comments.AddComment;

public class AddCommentHandler(
    ICommentRepository commentRepository,
    IUserRepository userRepository,
    IPhotoRepository photoRepository,
    ICommentAuditor commentAuditor,
    IValidator<CommentDto> validator)
{
    public async Task<Result<Guid>> HandleAsync(CommentDto dto)
    {
        var validation = validator.Validate(dto);

        if (!validation.IsValid)
        {
            var errors = string.Join("; ", validation.Errors.Select(e => e.ErrorMessage));
            return Result<Guid>.Failure(errors);
        }

        var user = await userRepository.GetByIdAsync(dto.UserId);
        if (user is null)
            return Result<Guid>.Failure("User does not exist");

        var photo = await photoRepository.GetByIdAsync(dto.PhotoId);
        if (photo is null)
            return Result<Guid>.Failure("Photo does not exist");

        var alreadyCommented = await commentRepository.ExistsForUserAsync(dto.UserId, dto.PhotoId);
        if (alreadyCommented)
            return Result<Guid>.Failure("User has already commented...");

        var isCommentClean = await commentAuditor.IsTextCleanAsync(dto.Text);
        if (!isCommentClean)
        {
            return Result<Guid>.Failure("Comment contains inappropriate language");
        }

        var comment = new Comment(dto.UserId, dto.PhotoId, dto.Text, Rating.FromValue(dto.Rating));

        await commentRepository.AddAsync(comment);

        return Result<Guid>.Success(comment.Id);
    }
}
