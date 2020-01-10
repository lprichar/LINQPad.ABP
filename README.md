# LINQPad.ABP
Adds ASP.Net Boilerplate support to LINQPad Queries.  This requires two steps

# 1. Update DataContext 

It needs a constructor overload that takes a connection string:

```
#if DEBUG
        private string _connectionString;
 
        /// <summary>
        /// For LINQPad
        /// </summary>
        public MyProjDbContext(string connectionString) 
            : base(new DbContextOptions<MyProjDbContext>())
        {
            _connectionString = connectionString;
        }
 
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_connectionString == null)
            {
                base.OnConfiguring(optionsBuilder); // Normal operation
                return;
            }
 
            // We have a connection string
            var dbContextOptionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseSqlServer(_connectionString);
            base.OnConfiguring(dbContextOptionsBuilder);
        }
#endif

```

# 2. Add an ABP Module to LINQPad

```
[DependsOn(typeof(MyProjEntityFrameworkModule))]
public class LinqPadModule : LinqPadModuleBase {
	public LinqPadModule(MyProjEntityFrameworkModule abpProjectNameEntityFrameworkModule)
	{
		abpProjectNameEntityFrameworkModule.SkipDbSeed = true;
	}
	
	public override void InitializeServices(ServiceCollection services) {
		IdentityRegistrar.Register(services);
	}
}

async Task Main()
{
	var abpCtx = Util.Cache(LinqPadAbp.InitModule<LinqPadModule>, "LinqPadAbp");

	using (var uowManager = abpCtx.StartUow(this, tenantId: 5, userId: 8))
	{
		var fundService = abpCtx.IocManager.Resolve<IFundService>();
		var entity = await thingService.GetEntityByIdAsync(1045);
		entity.Dump();
	}
}
```
