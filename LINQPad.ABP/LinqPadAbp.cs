using System;
using Abp;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Modules;
using Abp.Runtime.Session;
using Microsoft.EntityFrameworkCore;

namespace LINQPad.ABP
{
    public interface ILinqPadContextGetter
    {
        DbContext DbContext { get; set; }
    }

    public class LinqPadContextGetter : ISingletonDependency, ILinqPadContextGetter
    {
        public DbContext DbContext { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class LinqPadAbp : IDisposable
    {
        public AbpBootstrapper AbpBootstrapper { get; }
        public DbContext DbContext { get; set; }

        private LinqPadAbp(AbpBootstrapper abpBootstrapper)
        {
            AbpBootstrapper = abpBootstrapper;
        }

        public static LinqPadAbp InitModule<TModule>()
            where TModule : AbpModule
        {
            Console.WriteLine("Initializing Module");
            var bootstrapper = AbpBootstrapper.Create<TModule>();
            bootstrapper.IocManager.Register<ILinqPadContextGetter, LinqPadContextGetter>();
            bootstrapper.IocManager.Register<IUnitOfWork, LinqPadUow>(DependencyLifeStyle.Transient);
            bootstrapper.Initialize();
            return new LinqPadAbp(bootstrapper);
        }

        public IIocManager IocManager => AbpBootstrapper.IocManager;

        public IDisposable StartUow(DbContext dbContext, int? tenantId, long? userId)
        {
            Console.WriteLine($"Starting Unit of Work with userId: {userId}, and tenantId: {tenantId}");

            var linqPadContextGetter = Abp.Dependency.IocManager.Instance.Resolve<ILinqPadContextGetter>();
            linqPadContextGetter.DbContext = dbContext;

            var unitOfWorkManager = Abp.Dependency.IocManager.Instance.Resolve<IUnitOfWorkManager>();

            var unitOfWorkCompleteHandle = unitOfWorkManager.Begin(new UnitOfWorkOptions());
            unitOfWorkManager.Current.SetTenantId(tenantId);

            var abpSession = Abp.Dependency.IocManager.Instance.Resolve<IAbpSession>();
            var session = abpSession.Use(tenantId, userId);

            return new UowSession(unitOfWorkCompleteHandle, session);
        }

        public void Dispose()
        {
            AbpBootstrapper?.Dispose();
            DbContext?.Dispose();
        }
    }
}
