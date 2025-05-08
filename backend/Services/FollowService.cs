using Models;
using Results;
using Services.Interfaces;
using UoW;

namespace Services;

public class FollowService(IUnitOfWork unitOfWork) : IFollowService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    
    public async Task<CustomResult<bool>> FollowUser(string sourceUserId, string targetUserId)
    {
        // validate users
        var validationResult = await ValidateUsers(sourceUserId, targetUserId);
        if (validationResult.IsFailure)
        {
            return CustomResult<bool>.Failure(validationResult.Error!);
        }

        // check if the source user is already following the target user
        var isFollowing = await CheckFollowRelationship(sourceUserId, targetUserId);
        if (isFollowing)
        {
            return CustomResult<bool>.Failure(CustomError.ValidationError("Already following the target user"));
        }

        // create a new follow relationship
        var follow = new Follow
        {
            Id = Guid.NewGuid().ToString(),
            FollowerId = sourceUserId,
            FollowedId = targetUserId
        };

        // add the follow relationship to the database
        await _unitOfWork.Repository<Follow>().AddAsync(follow);
        await _unitOfWork.SaveChangesAsync();

        return CustomResult<bool>.Success(true);
    }

    public async Task<CustomResult<bool>> UnfollowUser(string sourceUserId, string targetUserId)
    {
        // validate users
        var validationResult = await ValidateUsers(sourceUserId, targetUserId);
        if (validationResult.IsFailure)
        {
            return CustomResult<bool>.Failure(validationResult.Error!);
        }

        // check if the source user is following the target user
        var isFollowing = await CheckFollowRelationship(sourceUserId, targetUserId);
        if (!isFollowing)
        {
            return CustomResult<bool>.Failure(CustomError.ValidationError("Not following the target user"));
        }

        // delete the follow relationship
        var follow = (await _unitOfWork.Repository<Follow>().FindAsync(
            f => f.FollowerId == sourceUserId && f.FollowedId == targetUserId
        )).FirstOrDefault();
        if (follow is null)
        {
            return CustomResult<bool>.Failure(CustomError.RecordNotFound("Follow relationship not found"));
        }

        _unitOfWork.Repository<Follow>().Delete(follow);
        await _unitOfWork.SaveChangesAsync();

        return CustomResult<bool>.Success(true);
    }

    public async Task<CustomResult<bool>> IsFollowing(string sourceUserId, string targetUserId)
    {
        // validate users
        var validationResult = await ValidateUsers(sourceUserId, targetUserId);
        if (validationResult.IsFailure)
        {
            return CustomResult<bool>.Failure(validationResult.Error!);
        }

        // check if the source user is following the target user
        var isFollowing = await CheckFollowRelationship(sourceUserId, targetUserId);
        return CustomResult<bool>.Success(isFollowing);
    }

    private async Task<CustomResult<bool>> ValidateUsers(string sourceUserId, string targetUserId)
    {
        // Check if the source user exists
        var sourceUser = await _unitOfWork.Repository<User>().GetByIdAsync(sourceUserId);
        if (sourceUser is null)
        {
            return CustomResult<bool>.Failure(CustomError.RecordNotFound("Source user not found"));
        }

        // Check if the target user exists
        var targetUser = await _unitOfWork.Repository<User>().GetByIdAsync(targetUserId);
        if (targetUser is null)
        {
            return CustomResult<bool>.Failure(CustomError.RecordNotFound("Target user not found"));
        }

        // Check if the source user is trying to perform an action on themselves
        if (sourceUserId == targetUserId)
        {
            return CustomResult<bool>.Failure(CustomError.ValidationError("Cannot perform this action on yourself"));
        }

        return CustomResult<bool>.Success(true);
    }

    private async Task<bool> CheckFollowRelationship(string sourceUserId, string targetUserId)
    {
        return await _unitOfWork.Repository<Follow>().AnyAsync(
            f => f.FollowerId == sourceUserId && f.FollowedId == targetUserId
        );
    }
}