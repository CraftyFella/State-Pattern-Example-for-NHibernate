
using FluentNHibernate.Mapping;
using VoucherCompany.Domain;

namespace VoucherCompany.Data
{
	public class VoucherMap : ClassMap<Voucher>
	{
		public VoucherMap()
		{
			Not.LazyLoad();

			Id(x => x.VoucherCode);

			Map(x => x.VoucherState)
				.CustomType(typeof (string))
				.Access.CamelCaseField(Prefix.None);
		}

	}
}
