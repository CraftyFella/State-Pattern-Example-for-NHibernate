using System.IO;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace VoucherCompany.Data
{
	public static class NHibernateSessionHelper
	{

		private const string DbName = "swiftvoucher.db";

		public static ISessionFactory CreateSessionFactory()
		{
			return Fluently.Configure()
				.Database(
					SQLiteConfiguration.Standard
						.UsingFile(DbName)
				)
				.Mappings(m => m.FluentMappings.AddFromAssemblyOf<VoucherMap>())
				.ExposeConfiguration(BuildSchema)
				.BuildSessionFactory();
		}

		private static void BuildSchema(Configuration config)
		{
			// Only create once
			if (File.Exists(DbName))
				return;

			// this NHibernate tool takes a configuration (with mapping info in)
			// and exports a database schema from it
			new SchemaExport(config)
				.Create(false, true);
		}
	}
}