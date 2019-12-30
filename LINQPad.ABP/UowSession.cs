using System;
using Abp.Dependency;
using Abp.Domain.Uow;

namespace LINQPad.ABP
{
    public class UowSession : IDisposable
    {
        private IUnitOfWorkCompleteHandle _unitOfWorkCompleteHandle;
        private IDisposable _session;

        public UowSession(IUnitOfWorkCompleteHandle unitOfWorkCompleteHandle,
            IDisposable session)
        {
            _unitOfWorkCompleteHandle = unitOfWorkCompleteHandle;
            _session = session;
        }

        public void Dispose()
        {
            _unitOfWorkCompleteHandle?.Complete();
            _unitOfWorkCompleteHandle?.Dispose();
            IocManager.Instance.Release(_unitOfWorkCompleteHandle);
            _unitOfWorkCompleteHandle = null;

            _session?.Dispose();
            IocManager.Instance.Release(_session);
            _session = null;
        }
    }
}