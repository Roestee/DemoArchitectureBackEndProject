﻿using Business.Aspects.Secured;
using Business.Repositories.OperationClaimRepository.Constans;
using Business.Repositories.OperationClaimRepository.Validation.FleuntValidation;
using Core.Aspects.Performance;
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
            var result = BusinessRules.Run(IsNameExistToAdd(operationClaim.Name));
            if (result != null) return result;

            _operationClaimDal.Add(operationClaim);
            return new SuccessResult(OperationClaimMessages.Added);
        }

        [ValidationAspect(typeof(OperationClaimValidator))]
        public IResult Update(OperationClaim operationClaim)
        {
            var result = BusinessRules.Run(IsNameExistToUpdate(operationClaim));
            if (result != null) return result;

            _operationClaimDal.Update(operationClaim);
            return new SuccessResult(OperationClaimMessages.Updated);
        }

        public IResult Delete(OperationClaim operationClaim)
        {
            _operationClaimDal.Delete(operationClaim);
            return new SuccessResult(OperationClaimMessages.Deleted);
        }

        [SecuredAspect("Admin")]
        [PerformanceAspect()]
        public IDataResult<List<OperationClaim>> GetList()
        {
            return new SuccessDataResult<List<OperationClaim>>(_operationClaimDal.GetAll());
        }

        public IDataResult<OperationClaim> GetById(int id)
        {
            return new SuccessDataResult<OperationClaim>(_operationClaimDal.Get(p => p.Id == id));
        }

        private IResult IsNameExistToAdd(string name)
        {
            var result = _operationClaimDal.Get(p => p.Name == name);
            if (result != null)
                return new ErrorResult(OperationClaimMessages.NameIsNotAvailable);

            return new SuccessResult();
        } 
        private IResult IsNameExistToUpdate(OperationClaim operationClaim)
        {
            var currentOperationClaim = _operationClaimDal.Get(p => p.Id == operationClaim.Id);
            if (currentOperationClaim.Name != operationClaim.Name)
            {
                var result = _operationClaimDal.Get(p => p.Name == operationClaim.Name);
                if (result != null)
                    return new ErrorResult(OperationClaimMessages.NameIsNotAvailable);
            }

            return new SuccessResult();
        }
    }
}
