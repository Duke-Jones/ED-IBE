using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RegulatedNoise.Test
{
    internal static class TestHelpers
    {
        public static void AssertAllPropertiesEqual<TObject>(TObject expected, TObject actual)
        {
            foreach (PropertyInfo propertyInfo in typeof(TObject).GetProperties())
            {
                if (typeof(IEnumerable).IsAssignableFrom(propertyInfo.PropertyType))
                {
                    var expectedEnumeration = (IEnumerable)propertyInfo.GetValue(expected);
                    var actualEnumeration = (IEnumerable)propertyInfo.GetValue(actual);
                    if (expectedEnumeration == null || actualEnumeration == null)
                    {
                        Assert.AreEqual(propertyInfo.GetValue(expected), propertyInfo.GetValue(actual),
                            propertyInfo.Name + ": does not match");
                    }
                    else // both are not null
                    {
                        AreEquivalent(expectedEnumeration, actualEnumeration, propertyInfo.Name + ": does not match");
                    }
                }
                else
                {
                    Assert.AreEqual(propertyInfo.GetValue(expected), propertyInfo.GetValue(actual),
                        propertyInfo.Name + ": does not match");

                }
            }
        }

        public static void AreEquivalent(IEnumerable expected, IEnumerable actual, string message, params object[] parameters)
        {
            if (expected == null != (actual == null))
                Assert.AreEqual(expected, actual, message, parameters);
            if (object.ReferenceEquals((object)expected, (object)actual) || expected == null)
                return;
            int expectedCount;
            int actualCount;
            object mismatchedElement;
            if (!FindMismatchedElement(expected, actual, out expectedCount, out actualCount, out mismatchedElement))
                return;
            Assert.Fail("enumerations are not equivalent" + message, parameters);
        }

        private static Dictionary<object, int> GetElementCounts(IEnumerable enumeration, out int nullCount)
        {
            Dictionary<object, int> dictionary = new Dictionary<object, int>();
            nullCount = 0;
            foreach (object key in enumeration)
            {
                if (key == null)
                {
                    ++nullCount;
                }
                else
                {
                    int num;
                    dictionary.TryGetValue(key, out num);
                    ++num;
                    dictionary[key] = num;
                }
            }
            return dictionary;
        }

        private static bool FindMismatchedElement(IEnumerable expected, IEnumerable actual, out int expectedCount, out int actualCount, out object mismatchedElement)
        {
            int nullCount1;
            Dictionary<object, int> elementCounts1 = GetElementCounts(expected, out nullCount1);
            int nullCount2;
            Dictionary<object, int> elementCounts2 = GetElementCounts(actual, out nullCount2);
            if (nullCount2 != nullCount1)
            {
                expectedCount = nullCount1;
                actualCount = nullCount2;
                mismatchedElement = (object)null;
                return true;
            }
            foreach (object key in elementCounts1.Keys)
            {
                elementCounts1.TryGetValue(key, out expectedCount);
                elementCounts2.TryGetValue(key, out actualCount);
                if (expectedCount != actualCount)
                {
                    mismatchedElement = key;
                    return true;
                }
            }
            expectedCount = 0;
            actualCount = 0;
            mismatchedElement = (object)null;
            return false;
        }
    }
}