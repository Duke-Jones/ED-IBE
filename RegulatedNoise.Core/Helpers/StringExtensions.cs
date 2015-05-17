#region file header
// ////////////////////////////////////////////////////////////////////
// ///
// ///  
// /// 16.05.2015
// ///
// ///
// ////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Globalization;

namespace RegulatedNoise.Core.Helpers
{
	public static class StringExtensions
	{
		private static readonly TextInfo _textInfo = new CultureInfo("en-US", false).TextInfo;

		public static string ToCleanUpperCase(this string value)
		{
			return String.IsNullOrWhiteSpace(value) ? String.Empty : value.ToUpper().Trim();
		}		 

		public static string ToCleanTitleCase(this string value)
		{
			return String.IsNullOrWhiteSpace(value) ? String.Empty : _textInfo.ToTitleCase(value.Trim());
		}		 
	}
}