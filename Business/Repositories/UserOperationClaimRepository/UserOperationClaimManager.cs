using Business.Repositories.UserOperationClaimRepository.Constans;
using Business.Repositories.UserOperationClaimRepository.Validation.FluentValidation;
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

        public UserOperationClaimManager(IUserOperationClaimDal userOperationClaimDal)
        {
            _userOperationClaimDal = userOperationClaimDal;
        }

        [ValidationAspect(typeof(UserOperationClaimValidator))]
        public IResult Add(UserOperationClaim userOperationClaim)
        {
            var result = BusinessRules.Run(IsOperationSetAvailable(userOperationClaim));
            if (result != null) return result;

            _userOperationClaimDal.Add(userOperationClaim);
            return new SuccessResult(UserOperationClaimMessages.Added);
        }

        [ValidationAspect(typeof(UserOperationClaimValidator))]
        public IResult Update(UserOperationClaim userOperationClaim)
        {
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

        private IResult IsOperationSetAvailable(UserOperationClaim userOperationClaim)
        {
            var result = _userOperationClaimDal.Get(p =>
                p.UserId == userOperationClaim.UserId && p.OperationClaimId == userOperationClaim.OperationClaimId);
            if (result != null) 
                return new ErrorResult(UserOperationClaimMessages.OperationClaimSetNotAvailable);

            return new SuccessResult();
        }
    }
}
