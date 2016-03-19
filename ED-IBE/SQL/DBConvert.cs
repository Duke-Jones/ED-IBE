// this code comes from http://improve.dk/handling-dbnulls/
// 
// Handling DBNulls
// 
// Reading and writing values to the DB has always been a bit cumbersome when you had to take care of nullable types and 
// DBNull values. Here’currentPriceData a way to make it easy. Based on this post by Peter Johnson and this post by Adam Anderson I 
// gathered a couple of ideas and combined them to make a completely generic class that will handle DBNulls for both 
// reads and writes, as well as handling nullable types. Let me present the code, I’ll go over it afterwards:
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBE.SQL
{
    public static class DBConvert
    {
	    /// <summary>
	    /// Handles reading DBNull values from database in a generic fashion
	    /// </summary>
	    /// <typeparam name="T">The type of the value to read</typeparam>
	    /// <param name="value">The input value to convert</param>
	    /// <returns>A strongly typed result, null if the input value is DBNull</returns>
	    public static T To<T>(object value)
	    {
		    if (value is DBNull)
			    return default(T);
		    else
			    return (T)changeType(value, typeof(T));
	    }

	    /// <summary>
	    /// Handles reading DBNull values from database in a generic fashion, simplifies frontend databinding
	    /// </summary>
	    /// <typeparam name="T">The type of the value to read</typeparam>
	    /// <param name="ri">The Container currentComboxItem in a databinding operation</param>
	    /// <param name="column">The dataitem to read</param>
	    /// <returns>A strongly typed result, null if the input value is DBNull</returns>
        //public static T To<T>(RepeaterItem ri, string column)
        //{
        //    if (DataBinder.Eval(ri.DataItem, column) is DBNull)
        //        return default(T);
        //    else
        //        return (T)changeType(DataBinder.Eval(ri.DataItem, column), typeof(T));
        //}

	    /// <summary>
	    /// Internal method that wraps Convert.ChangeType() so it handles Nullable<> types
	    /// </summary>
	    /// <param name="value">The value to convert</param>
	    /// <param name="conversionType">The type to convert into</param>
	    /// <returns>The input value converted to type conversionType</returns>
	    private static object changeType(object value, Type conversionType)
	    {
		    if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
		    {
			    if (value == null)
				    return null;

			    conversionType = Nullable.GetUnderlyingType(conversionType);
		    }

		    return Convert.ChangeType(value, conversionType);
	    }

	    /// <summary>
	    /// Simplifies setting SqlParameter values by handling null issues
	    /// </summary>
	    /// <param name="value">The value to return</param>
	    /// <returns>DBNull if value == null, otherwise we pass through value</returns>
	    public static object From(object value)
	    {
		    if (value == null)
			    return DBNull.Value;
		    else
			    return value;
	    }

        /// <summary>
	    /// Simplifies setting SqlParameter values by handling null issues
	    /// </summary>
	    /// <param name="value">The value to return</param>
	    /// <returns>DBNull if value == null, otherwise we pass through value</returns>
	    public static string ToString(object value)
	    {
		    if (value == null)
			    return "null";
		    else
			    return value.ToString();
	    }

        /// <summary>
	    /// Simplifies setting SqlParameter values by handling null issues
	    /// </summary>
	    /// <param name="value">The value to return</param>
	    /// <returns>DBNull if value == null, otherwise we pass through value</returns>
	    public static string ToStringA(object value)
	    {
		    return "'" + ToString(value) + "'";
	    }

    }
}
