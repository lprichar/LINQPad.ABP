using System;
using Abp.Events.Bus;
using Abp.Modules;
using Abp.Zero;
using Abp.Zero.EntityFrameworkCore;
using Castle.MicroKernel.Registration;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace LINQPad.ABP
{
    [DependsOn(typeof(AbpZeroCoreModule))]
    [DependsOn(typeof(AbpZeroCoreEntityFrameworkCoreModule))]
    public abstract class LinqPadModuleBase : AbpModule
    {
        protected LinqPadModuleBase()
        {
            Console.WriteLine("Loading module");
        }

        public override void PreInitialize()
        {
            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
            Configuration.ReplaceService(
                typeof(IEventBus),
                () => IocManager.IocContainer.Register(
                    Component.For<IEventBus>().Instance(NullEventBus.Instance)
                )
            );
        }

        public override void Initialize()
        {
            var services = new ServiceCollection();
            InitializeServices(services);
            WindsorRegistrationHelper.CreateServiceProvider(IocManager.IocContainer, services);
        }

        public abstract void InitializeServices(ServiceCollection serviceCollection);
    }
}