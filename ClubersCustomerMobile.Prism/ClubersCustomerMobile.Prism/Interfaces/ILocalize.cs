using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace ClubersCustomerMobile.Prism.Interfaces
{
	public interface ILocalize
	{
		CultureInfo GetCurrentCultureInfo();

		void SetLocale(CultureInfo ci);
	}
}