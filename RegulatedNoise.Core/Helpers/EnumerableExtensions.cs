#region file header
// ////////////////////////////////////////////////////////////////////
// ///
// ///  
// /// 16.05.2015
// ///
// ///
// ////////////////////////////////////////////////////////////////////
#endregion

using System.Collections.Generic;
using System.Linq;

namespace RegulatedNoise.Core.Helpers
{
	public static class EnumerableExtensions
	{
		public static TItem[] CloneN<TItem>(this IEnumerable<TItem> array)
		{
			if (array == null)
				return null;
			else
				return array.ToArray();
		}		 
	}
}