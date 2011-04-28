using System;
using VoucherCompany.Domain.Infrastructure;

namespace VoucherCompany.Domain
{
	public class Voucher
    {

		public Voucher()
        {
			// Default State
        	voucherState = VoucherState.Reserved.DisplayName;
        }

    	protected string voucherState;
    	public VoucherState VoucherState
    	{
    		get { return Enumeration.FromDisplayName<VoucherState>(voucherState); }
    	}
		public string VoucherCode { get; set; }

		internal void SetVoucherState(VoucherState state)
		{
			this.voucherState = state.DisplayName;
		}

        public void Activate()
        {
            VoucherState.Activate(this);
        }

        public void PreRedeem()
        {
            VoucherState.PreRedeem(this);
        }

        public void Redeem()
        {
            VoucherState.Redeem(this);
        }

        public void MarkAsFinished()
        {
            Console.WriteLine("Finished");
        }

		public override string ToString()
        {
        	return VoucherState.ToString();
        }
        
    }
}
