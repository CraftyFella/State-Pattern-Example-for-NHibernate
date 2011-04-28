using System;
using VoucherCompany.Domain.Infrastructure;

namespace VoucherCompany.Domain
{
	public abstract class VoucherState : Enumeration
	{

		public enum VoucherStates
		{
			Reserved = 1,
			Activated = 2,
			Preredeemed = 3,
			Redeemed = 4,
		}

		// Required fields for GetAll method in Enumeration
		public static readonly VoucherState Activated = new ActivatedVoucherState();
		public static readonly VoucherState Reserved = new ReservedVoucherState();
		public static readonly VoucherState Preredeemed = new PreredeemedVoucherState();
		public static readonly VoucherState Redeemed = new RedeemedVoucherState();

		protected VoucherState(VoucherStates voucherState) : base((int)voucherState, voucherState.ToString())
		{	
		}

		private void InvalidTransition(VoucherState targetState)
		{
			throw new ApplicationException("Can't transition voucher from " + DisplayName + " to " + targetState.DisplayName);
		}

		public virtual void Activate(Voucher voucher)
		{
			InvalidTransition(Activated);
		}


		public virtual void PreRedeem(Voucher voucher)
		{
			InvalidTransition(Preredeemed);
		}

    
		public virtual void Redeem(Voucher voucher)
		{
			InvalidTransition(Redeemed);
		}

		private class ActivatedVoucherState : VoucherState
		{
			public ActivatedVoucherState() : base(VoucherStates.Activated)
			{
			}

			public override void PreRedeem(Voucher voucher)
			{
				voucher.SetVoucherState(Preredeemed);
			}
		}

		private class PreredeemedVoucherState : VoucherState
		{

			public PreredeemedVoucherState() : base (VoucherStates.Preredeemed)
			{
			
			}

			public override void Redeem(Voucher voucher)
			{
				voucher.SetVoucherState(Redeemed);

				// Mark As Finished
				voucher.MarkAsFinished();
			}
		}

		private class RedeemedVoucherState : VoucherState
		{

			public RedeemedVoucherState() : base (VoucherStates.Redeemed)
			{
			}
		}

		private class ReservedVoucherState : VoucherState
		{
			public ReservedVoucherState() : base(VoucherStates.Reserved)
			{
			}

			public override void Activate(Voucher voucher)
			{
				voucher.SetVoucherState(Activated);
			}
		}
	}
}