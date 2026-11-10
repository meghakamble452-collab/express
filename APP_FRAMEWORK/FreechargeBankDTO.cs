using System.Collections.Generic;

namespace APP_FRAMEWORK;

public class FreechargeBankDTO
{
	public string ClientId { get; set; }

	public string Mac { get; set; }

	public string BankCode { get; set; }

	public string Customerid { get; set; }

	public string MobileNumber { get; set; }

	public string oemdeviceid { get; set; }

	public string txnId { get; set; }

	public string ProxyStr { get; set; }

	public string? csrfRequestIdentifier { get; set; }

	public List<CookieData> cookies { get; set; }
}
