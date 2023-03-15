using Business.Repositories.OperationClaimRepository.Constans;
using Business.Repositories.OperationClaimRepository.Validation.FleuntValidation;
using Core.Aspects.Validation;
using Core.Utilities.Business;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Repositories.OperationClaimRepository;
using Entities.Concrete;

namespace Business.Repositories.OperationClaimRepository
{
    public class OperationClaimManagers : IOperationClaimService
    {
        private readonly IOperationClaimDal _operationClaimDal;

        public OperationClaimManagers(IOperationClaimDal operationClaimDal)
        {
            _operationClaimDal = operationClaimDal;
        }

        [ValidationAspect(typeof(OperationClaimValidator))]
        public IResult Add(OperationClaim operationClaim)
        {
            var result = BusinessRules.Run(IsNameAvailable(operationClaim.Name));
            if (result != null) return result;

            _operationClaimDal.Add(operationClaim);
            return new SuccessResult(OperationClaimMessages.Added);
        }

        [ValidationAspect(typeof(OperationClaimValidator))]
        public IResult Update(OperationClaim operationClaim)
        {
            _operationClaimDal.Update(operationClaim);
            return new SuccessResult(OperationClaimMessages.Updated);
        }

        public IResult Delete(OperationClaim operationClaim)
        {
            _operationClaimDal.Delete(operationClaim);
            return new SuccessResult(OperationClaimMessages.Deleted);
        }

        public IDataResult<List<OperationClaim>> GetList()
        {
            return new SuccessDataResult<List<OperationClaim>>(_operationClaimDal.GetAll());
        }

        public IDataResult<OperationClaim> GetById(int id)
        {
            return new SuccessDataResult<OperationClaim>(_operationClaimDal.Get(p => p.Id == id));
        }

        private IResult IsNameAvailable(string name)
        {
            var result = _operationClaimDal.Get(p => p.Name == name);
            if (result != null)
                return new ErrorResult(OperationClaimMessages.NameIsNotAvailable);

            return new SuccessResult();
        }
    }
}
