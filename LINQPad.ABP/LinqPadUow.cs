using System;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.EntityFramework;
using Abp.EntityFrameworkCore;
using Abp.EntityFrameworkCore.Uow;
using Abp.MultiTenancy;

namespace LINQPad.ABP
{
    /// <summary>
    /// An ABP Unit of Work that uses an existing (LINQPad) data context rather than creating one as the
    /// EfCoreUnitOfWork traditionally does. It retrieves the LINQPad data context via a
    /// ILinqPadContextGetter object that is initialized in LinqPadAbp.StartUow()
    /// </summary>
    public class LinqPadUow : EfCoreUnitOfWork
    {
        private readonly ILinqPadContextGetter _linqPadContextGetter;

        public LinqPadUow(IIocResolver iocResolver, IConnectionStringResolver connectionStringResolver,
            IUnitOfWorkFilterExecuter filterExecuter, IDbContextResolver dbContextResolver, IUnitOfWorkDefaultOptions defaultOptions,
            IDbContextTypeMatcher dbContextTypeMatcher, IEfCoreTransactionStrategy transactionStrategy, ILinqPadContextGetter linqPadContextGetter)
            : base(iocResolver, connectionStringResolver, filterExecuter, dbContextResolver, defaultOptions, dbContextTypeMatcher, transactionStrategy)
        {
            Console.WriteLine("LinqPadUow.ctor");
            _linqPadContextGetter = linqPadContextGetter;
        }

        public override TDbContext GetOrCreateDbContext<TDbContext>(MultiTenancySides? multiTenancySide = null, string name = null)
        {
            if (_linqPadContextGetter == null) throw new ArgumentNullException(nameof(LinqPadContextGetter));
            if (_linqPadContextGetter.DbContext == null) throw new ArgumentNullException("DbContext");
            return (TDbContext)_linqPadContextGetter.DbContext;
        }
    }
}