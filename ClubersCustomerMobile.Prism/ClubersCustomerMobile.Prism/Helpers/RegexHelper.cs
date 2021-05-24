
using ClubersCustomerMobile.Prism.Interfaces;
using System;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace ClubersCustomerMobile.Prism.Helpers
{
    public class RegexHelper : IRegexHelper
    {
        public bool IsValidEmail(string emailaddress)
        {
            try
            {
                new MailAddress(emailaddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public bool IsValidPIN(string pinnumber)
        {
            string re = @"(?x)^
            # fail if...
            (?!
                # repeating numbers
                (\d) \1+ $
                |
                # sequential ascending
                (?:0(?=1)|1(?=2)|2(?=3)|3(?=4)|4(?=5)|5(?=6)|6(?=7)|7(?=8)|8(?=9)|9(?=0)){3} \d $
                |
                # sequential descending
                (?:0(?=9)|1(?=0)|2(?=1)|3(?=2)|4(?=3)|5(?=4)|6(?=5)|7(?=6)|8(?=7)|9(?=8)){3} \d $
            )
            # match any other combinations of 4 digits
            \d{4}$";
            try
            {
                
                bool Value = Regex.IsMatch(pinnumber, re) ? true : false;
                return Value;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
