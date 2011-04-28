using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using VoucherCompany.Data;
using VoucherCompany.Domain;

namespace VoucherCompany.Client
{
	class Program
	{
		static void Main()
		{
			Voucher voucher = null;
			var sessionFactory = NHibernateSessionHelper.CreateSessionFactory();
			var command = GetNextCommand();

			
			while (command != "q")
			{
				try
				{
					switch (command)
					{
						case "n":
							voucher = NewVoucher();
							break;
						case "a":
							ActivateVoucher(voucher);
							break;
						case "p":
							PreredeemVoucher(voucher);
							break;
						case "r":
							RedeemVoucher(voucher);
							break;
						case "s":
							SaveVoucher(voucher, sessionFactory);
							break;
						case "l":
							voucher = LoadVoucher(sessionFactory);
							break;
						case "q":
							Quit();
							return;
					}
				}
				catch (Exception ex)
				{
					Error("Error " + ex.Message);
				}

				Info(voucher);
				command = GetNextCommand();
			}
		}

		private static void Quit()
		{
			Console.WriteLine("Bye bye");
		}

		private static Voucher LoadVoucher(ISessionFactory sessionFactory)
		{
			Voucher voucher;
			Console.WriteLine("Enter a voucher code");
			string voucherCode = Console.ReadLine();
			using (var session = sessionFactory.OpenSession())
			{
				using (session.BeginTransaction())
				{
					voucher = session.Load<Voucher>(voucherCode);
				}
			}
			return voucher;
		}

		private static void SaveVoucher(Voucher voucher, ISessionFactory sessionFactory)
		{
			if (!ExistingVoucher(voucher)) return;

			using (var session = sessionFactory.OpenSession())
			{
				using (var transaction = session.BeginTransaction())
				{
					while (string.IsNullOrEmpty(voucher.VoucherCode))
					{
						Console.WriteLine("Enter Voucher code");
						voucher.VoucherCode = Console.ReadLine();
					}session.SaveOrUpdate(voucher);
					transaction.Commit();
				}
			}
		}

		private static void RedeemVoucher(Voucher voucher)
		{
			if (!ExistingVoucher(voucher)) return;

			voucher.Redeem();
		}

		private static void PreredeemVoucher(Voucher voucher)
		{
			if (!ExistingVoucher(voucher)) return;

			voucher.PreRedeem();
		}

		private static void ActivateVoucher(Voucher voucher)
		{
			if (!ExistingVoucher(voucher)) return;

			voucher.Activate();
		}

		private static bool ExistingVoucher(Voucher voucher)
		{
			if (voucher == null)
			{
				Console.WriteLine("You must create a voucher first (n for new voucher)");
				return false;
			}

			return true;
		}

		private static Voucher NewVoucher()
		{
			return new Voucher();
		}

		private static string GetNextCommand()
		{
			OutputCommandList();
			var nextCommand = Console.ReadLine();
			Console.Clear();
			return nextCommand;
		}

		private static void OutputCommandList()
		{
			Instructions("n - (n)ew voucher");
			Instructions("a - (a)ctivates voucher");
			Instructions("p - (p)re-redeem voucher");
			Instructions("r - (r)edeem voucher");
			Instructions("s - (s)ave voucher");
			Instructions("l - (l)oad voucher");
			Instructions("q - (q)uit");
		}

		private static void Info(object message)
		{
			ColourMessage(message, ConsoleColor.Green);
		}

		private static void Error(object message)
		{
			ColourMessage(message, ConsoleColor.Red);
		}

		private static void Instructions(object message)
		{
			ColourMessage(message, ConsoleColor.Cyan);
		}

		private static void ColourMessage(object message, ConsoleColor colour)
		{
			var existingColor = Console.ForegroundColor;
			Console.ForegroundColor = colour;
			Console.WriteLine(message);
			Console.ForegroundColor = existingColor;
		}
	}
}
