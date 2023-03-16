using System.Linq.Expressions;
using System.Transactions;
using Castle.Components.DictionaryAdapter.Xml;
using Castle.DynamicProxy;
using Core.Utilities.Interceptors;

namespace Core.Aspects.Transaction
{
    public class TransactionAspect : MethodInterception
    {
        public override void Intercept(IInvocation invocation)
        {
            using (TransactionScope transaction = new TransactionScope())
            {
                try
                {
                    invocation.Proceed();
                    transaction.Complete();
                }
                catch (Exception e)
                {
                    transaction.Dispose();
                    throw;
                }
            }
        }
    }
}
