using Business.Repositories.OperationClaimRepository;
using Business.Repositories.UserOperationClaimRepository.Constans;
using Business.Repositories.UserOperationClaimRepository.Validation.FluentValidation;
using Business.Repositories.UserRepository;
using Core.Aspects.Validation;
using Core.Utilities.Business;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Repositories.UserOperationClaimRepository;
using Entities.Concrete;

namespace Business.Repositories.UserOperationClaimRepository
{
    public class UserOperationClaimManager : IUserOperationClaimService
    {
        private readonly IUserOperationClaimDal _userOperationClaimDal;
        private readonly IOperationClaimService _operationClaimService;
        private readonly IUserService _userService;

        public UserOperationClaimManager(IUserOperationClaimDal userOperationClaimDal, IOperationClaimService operationClaimService, IUserService userService)
        {
            _userOperationClaimDal = userOperationClaimDal;
            _operationClaimService = operationClaimService; 
            _userService = userService;
        }

        [ValidationAspect(typeof(UserOperationClaimValidator))]
        public IResult Add(UserOperationClaim userOperationClaim)
        {
            var result = BusinessRules.Run(
                IsUserExist(userOperationClaim.UserId),
                IsOperationClaimExist(userOperationClaim.OperationClaimId),
                IsOperationSetExistToAdd(userOperationClaim));
            if (result != null) return result;

            _userOperationClaimDal.Add(userOperationClaim);
            return new SuccessResult(UserOperationClaimMessages.Added);
        }

        [ValidationAspect(typeof(UserOperationClaimValidator))]
        public IResult Update(UserOperationClaim userOperationClaim)
        {
            var result = BusinessRules.Run(
                IsUserExist(userOperationClaim.UserId),
                IsOperationClaimExist(userOperationClaim.OperationClaimId),
                IsOperationSetExistToUpdate(userOperationClaim));
            if (result != null) return result;

            _userOperationClaimDal.Update(userOperationClaim);
            return new SuccessResult(UserOperationClaimMessages.Updated);
        }

        public IResult Delete(UserOperationClaim userOperationClaim)
        {
            _userOperationClaimDal.Delete(userOperationClaim);
            return new SuccessResult(UserOperationClaimMessages.Deleted);
        }

        public IDataResult<List<UserOperationClaim>> GetList()
        {
            return new SuccessDataResult<List<UserOperationClaim>>(_userOperationClaimDal.GetAll());
        }

        public IDataResult<UserOperationClaim> GetById(int id)
        {
            return new SuccessDataResult<UserOperationClaim>(_userOperationClaimDal.Get(p => p.Id == id));
        }

        private IResult IsUserExist(int userId)
        {
            var result = _userService.GetById(userId).Data;
            if (result == null)
                return new ErrorResult(UserOperationClaimMessages.UserNotExist);

            return new SuccessResult();
        }

        private IResult IsOperationClaimExist(int operationClaimId)
        {
            var result = _operationClaimService.GetById(operationClaimId).Data;
            if (result == null) 
                return new ErrorResult(UserOperationClaimMessages.OperationClaimNotExist);

            return new SuccessResult();
        }

        private IResult IsOperationSetExistToAdd(UserOperationClaim userOperationClaim)
        {
            var result = _userOperationClaimDal.Get(p =>
                p.UserId == userOperationClaim.UserId && p.OperationClaimId == userOperationClaim.OperationClaimId);
            if (result != null)
                return new ErrorResult(UserOperationClaimMessages.OperationClaimSetNotAvailable);

            return new SuccessResult();
        }

        private IResult IsOperationSetExistToUpdate(UserOperationClaim userOperationClaim)
        {
            var currentUserOperationClaim = _userOperationClaimDal.Get(p => p.Id == userOperationClaim.Id);
            if (currentUserOperationClaim.UserId != userOperationClaim.UserId ||
                currentUserOperationClaim.OperationClaimId != userOperationClaim.OperationClaimId)
            {
                var result = _userOperationClaimDal.Get(p =>
                    p.UserId == userOperationClaim.UserId && p.OperationClaimId == userOperationClaim.OperationClaimId);
                if (result != null)
                    return new ErrorResult(UserOperationClaimMessages.OperationClaimSetNotAvailable);
            }

            return new SuccessResult();
        }
    }
}
