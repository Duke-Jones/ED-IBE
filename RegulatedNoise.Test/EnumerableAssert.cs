using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RegulatedNoise.Test
{
    public static class EnumerableAssert
    {
        public static void Contains(IEnumerable enumeration, object element)
        {
            Contains(enumeration, element, string.Empty, (object[])null);
        }

        public static void Contains(IEnumerable enumeration, object element, string message)
        {
            Contains(enumeration, element, message, (object[])null);
        }

        public static void Contains(IEnumerable enumeration, object element, string message, params object[] parameters)
        {
            Assert.IsNotNull(enumeration, "enumeration is null");
            foreach (object objA in enumeration)
            {
                if (Equals(objA, element))
                    return;
            }
            Assert.Fail("enumeration does not contain " + element + DisplayMessage(message, parameters));
        }

        private static string DisplayMessage(string message, object[] parameters)
        {
            if (String.IsNullOrEmpty(message))
            {
                return String.Empty;
            }
            else
            {
                return " : " + String.Format(message, parameters);
            }
        }

        public static void DoesNotContain(IEnumerable enumeration, object element)
        {
            DoesNotContain(enumeration, element, string.Empty, (object[])null);
        }

        public static void DoesNotContain(IEnumerable enumeration, object element, string message)
        {
            DoesNotContain(enumeration, element, message, (object[])null);
        }

        public static void DoesNotContain(IEnumerable enumeration, object element, string message, params object[] parameters)
        {
            Assert.IsNotNull(enumeration, "enumeration is null");
            foreach (object objA in enumeration)
            {
                if (Equals(objA, element))
                    Assert.Fail("enumeration should not contains " + element + DisplayMessage(message, parameters));
            }
        }

        public static void AllItemsAreNotNull(IEnumerable enumeration)
        {
            AllItemsAreNotNull(enumeration, string.Empty, (object[])null);
        }

        public static void AllItemsAreNotNull(IEnumerable enumeration, string message)
        {
            AllItemsAreNotNull(enumeration, message, (object[])null);
        }

        public static void AllItemsAreNotNull(IEnumerable enumeration, string message, params object[] parameters)
        {
            Assert.IsNotNull(enumeration, "enumeration is null");
            foreach (object obj in enumeration)
            {
                if (obj == null)
                    Assert.Fail("null item found" + DisplayMessage(message, parameters));
            }
        }

        public static void AllItemsAreUnique(IEnumerable enumeration)
        {
            AllItemsAreUnique(enumeration, string.Empty, (object[])null);
        }

        public static void AllItemsAreUnique(IEnumerable enumeration, string message)
        {
            AllItemsAreUnique(enumeration, message, (object[])null);
        }

        public static void AllItemsAreUnique(IEnumerable enumeration, string message, params object[] parameters)
        {
            Assert.CheckParameterNotNull((object)enumeration, "enumerationAssert.AllItemsAreUnique", "enumeration", string.Empty);
            message = Assert.ReplaceNulls((object)message);
            bool flag = false;
            Hashtable hashtable = new Hashtable();
            foreach (object index in (IEnumerable)enumeration)
            {
                if (index == null)
                {
                    if (!flag)
                        flag = true;
                    else
                        Assert.HandleFail("enumerationAssert.AllItemsAreUnique", (string)FrameworkMessages.AllItemsAreUniqueFailMsg(message == null ? (object)string.Empty : (object)message, (object)FrameworkMessages.Common_NullInMessages), parameters);
                }
                else if (hashtable[index] != null)
                    Assert.HandleFail("enumerationAssert.AllItemsAreUnique", (string)FrameworkMessages.AllItemsAreUniqueFailMsg(message == null ? (object)string.Empty : (object)message, (object)Assert.ReplaceNulls(index)), parameters);
                else
                    hashtable.Add(index, (object)true);
            }
        }

        public static void IsSubsetOf(IEnumerable subset, IEnumerable superset)
        {
            IsSubsetOf(subset, superset, string.Empty, (object[])null);
        }

        public static void IsSubsetOf(IEnumerable subset, IEnumerable superset, string message)
        {
            IsSubsetOf(subset, superset, message, (object[])null);
        }

        public static void IsSubsetOf(IEnumerable subset, IEnumerable superset, string message, params object[] parameters)
        {
            Assert.CheckParameterNotNull((object)subset, "enumerationAssert.IsSubsetOf", "subset", string.Empty);
            Assert.CheckParameterNotNull((object)superset, "enumerationAssert.IsSubsetOf", "superset", string.Empty);
            if (IsSubsetOfHelper(subset, superset))
                return;
            Assert.HandleFail("enumerationAssert.IsSubsetOf", message, parameters);
        }

        public static void IsNotSubsetOf(IEnumerable subset, IEnumerable superset)
        {
            IsNotSubsetOf(subset, superset, string.Empty, (object[])null);
        }

        public static void IsNotSubsetOf(IEnumerable subset, IEnumerable superset, string message)
        {
            IsNotSubsetOf(subset, superset, message, (object[])null);
        }

        public static void IsNotSubsetOf(IEnumerable subset, IEnumerable superset, string message, params object[] parameters)
        {
            Assert.CheckParameterNotNull((object)subset, "enumerationAssert.IsNotSubsetOf", "subset", string.Empty);
            Assert.CheckParameterNotNull((object)superset, "enumerationAssert.IsNotSubsetOf", "superset", string.Empty);
            if (!IsSubsetOfHelper(subset, superset))
                return;
            Assert.HandleFail("enumerationAssert.IsNotSubsetOf", message, parameters);
        }

        public static void AreEquivalent(IEnumerable expected, IEnumerable actual)
        {
            AreEquivalent(expected, actual, string.Empty, (object[])null);
        }

        public static void AreEquivalent(IEnumerable expected, IEnumerable actual, string message)
        {
            AreEquivalent(expected, actual, message, (object[])null);
        }

        public static void AreNotEquivalent(IEnumerable expected, IEnumerable actual)
        {
            AreNotEquivalent(expected, actual, string.Empty, (object[])null);
        }

        public static void AreNotEquivalent(IEnumerable expected, IEnumerable actual, string message)
        {
            AreNotEquivalent(expected, actual, message, (object[])null);
        }

        public static void AreNotEquivalent(IEnumerable expected, IEnumerable actual, string message, params object[] parameters)
        {
            if (expected == null != (actual == null))
                return;
            if (object.ReferenceEquals((object)expected, (object)actual))
                Assert.HandleFail("enumerationAssert.AreNotEquivalent", (string)FrameworkMessages.BothenumerationsSameReference(message == null ? (object)string.Empty : (object)Assert.ReplaceNulls((object)message)), parameters);
            if (expected.Count != actual.Count)
                return;
            if (expected.Count == 0)
                Assert.HandleFail("enumerationAssert.AreNotEquivalent", (string)FrameworkMessages.BothenumerationsEmpty(message == null ? (object)string.Empty : (object)Assert.ReplaceNulls((object)message)), parameters);
            int expectedCount;
            int actualCount;
            object mismatchedElement;
            if (FindMismatchedElement(expected, actual, out expectedCount, out actualCount, out mismatchedElement))
                return;
            Assert.HandleFail("enumerationAssert.AreNotEquivalent", (string)FrameworkMessages.BothSameElements(message == null ? (object)string.Empty : (object)Assert.ReplaceNulls((object)message)), parameters);
        }

        public static void AllItemsAreInstancesOfType(IEnumerable enumeration, Type expectedType)
        {
            AllItemsAreInstancesOfType(enumeration, expectedType, string.Empty, (object[])null);
        }

        public static void AllItemsAreInstancesOfType(IEnumerable enumeration, Type expectedType, string message)
        {
            AllItemsAreInstancesOfType(enumeration, expectedType, message, (object[])null);
        }

        public static void AllItemsAreInstancesOfType(IEnumerable enumeration, Type expectedType, string message, params object[] parameters)
        {
            Assert.CheckParameterNotNull((object)enumeration, "enumerationAssert.AllItemsAreInstancesOfType", "enumeration", string.Empty);
            Assert.CheckParameterNotNull((object)expectedType, "enumerationAssert.AllItemsAreInstancesOfType", "expectedType", string.Empty);
            int num = 0;
            foreach (object o in (IEnumerable)enumeration)
            {
                if (!expectedType.IsInstanceOfType(o))
                    Assert.HandleFail("enumerationAssert.AllItemsAreInstancesOfType", o == null ? (string)FrameworkMessages.ElementTypesAtIndexDontMatch2(message == null ? (object)string.Empty : (object)Assert.ReplaceNulls((object)message), (object)num, (object)expectedType.ToString()) : (string)FrameworkMessages.ElementTypesAtIndexDontMatch(message == null ? (object)string.Empty : (object)Assert.ReplaceNulls((object)message), (object)num, (object)expectedType.ToString(), (object)o.GetType().ToString()), parameters);
                ++num;
            }
        }

        public static void AreEqual(IEnumerable expected, IEnumerable actual)
        {
            AreEqual(expected, actual, string.Empty, (object[])null);
        }

        public static void AreEqual(IEnumerable expected, IEnumerable actual, string message)
        {
            AreEqual(expected, actual, message, (object[])null);
        }

        public static void AreEqual(IEnumerable expected, IEnumerable actual, string message, params object[] parameters)
        {
            string reason = string.Empty;
            if (AreEnumerationsEqual(expected, actual, (IComparer)new ObjectComparer(), ref reason))
                return;
            Assert.HandleFail("enumerationAssert.AreEqual", (string)FrameworkMessages.enumerationEqualReason((object)message, (object)reason), parameters);
        }

        public static void AreNotEqual(IEnumerable notExpected, IEnumerable actual)
        {
            AreNotEqual(notExpected, actual, string.Empty, (object[])null);
        }

        public static void AreNotEqual(IEnumerable notExpected, IEnumerable actual, string message)
        {
            AreNotEqual(notExpected, actual, message, (object[])null);
        }

        public static void AreNotEqual(IEnumerable notExpected, IEnumerable actual, string message, params object[] parameters)
        {
            string reason = string.Empty;
            if (!AreEnumerationsEqual(notExpected, actual, (IComparer)new ObjectComparer(), ref reason))
                return;
            Assert.HandleFail("enumerationAssert.AreNotEqual", (string)FrameworkMessages.enumerationEqualReason((object)message, (object)reason), parameters);
        }

        public static void AreEqual(IEnumerable expected, IEnumerable actual, IComparer comparer)
        {
            AreEqual(expected, actual, comparer, string.Empty, (object[])null);
        }

        public static void AreEqual(IEnumerable expected, IEnumerable actual, IComparer comparer, string message)
        {
            AreEqual(expected, actual, comparer, message, (object[])null);
        }

        public static void AreEqual(IEnumerable expected, IEnumerable actual, IComparer comparer, string message, params object[] parameters)
        {
            string reason = string.Empty;
            if (AreEnumerationsEqual(expected, actual, comparer, ref reason))
                return;
            Assert.HandleFail("enumerationAssert.AreEqual", (string)FrameworkMessages.enumerationEqualReason((object)message, (object)reason), parameters);
        }

        public static void AreNotEqual(IEnumerable notExpected, IEnumerable actual, IComparer comparer)
        {
            AreNotEqual(notExpected, actual, comparer, string.Empty, (object[])null);
        }

        public static void AreNotEqual(IEnumerable notExpected, IEnumerable actual, IComparer comparer, string message)
        {
            AreNotEqual(notExpected, actual, comparer, message, (object[])null);
        }

        public static void AreNotEqual(IEnumerable notExpected, IEnumerable actual, IComparer comparer, string message, params object[] parameters)
        {
            string reason = string.Empty;
            if (!AreEnumerationsEqual(notExpected, actual, comparer, ref reason))
                return;
            Assert.HandleFail("enumerationAssert.AreNotEqual", (string)FrameworkMessages.enumerationEqualReason((object)message, (object)reason), parameters);
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
            Assert.Fail("enumerations are not equivalent" + DisplayMessage(message, parameters));
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

        internal static bool IsSubsetOfHelper(IEnumerable subset, IEnumerable superset)
        {
            int nullCount1;
            Dictionary<object, int> elementCounts1 = GetElementCounts(subset, out nullCount1);
            int nullCount2;
            Dictionary<object, int> elementCounts2 = GetElementCounts(superset, out nullCount2);
            if (nullCount1 > nullCount2)
                return false;
            foreach (object key in elementCounts1.Keys)
            {
                int num1;
                elementCounts1.TryGetValue(key, out num1);
                int num2;
                elementCounts2.TryGetValue(key, out num2);
                if (num1 > num2)
                    return false;
            }
            return true;
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

        private static bool AreEnumerationsEqual(IEnumerable expected, IEnumerable actual, IComparer comparer, ref string reason)
        {
            Assert.CheckParameterNotNull((object)comparer, "Assert.AreenumerationsEqual", "comparer", string.Empty);
            if (!object.ReferenceEquals((object)expected, (object)actual))
            {
                if (expected == null || actual == null)
                    return false;
                if (expected.Count != actual.Count)
                {
                    reason = (string)FrameworkMessages.NumberOfElementsDiff;
                    return false;
                }
                IEnumerator enumerator1 = expected.GetEnumerator();
                IEnumerator enumerator2 = actual.GetEnumerator();
                int num = 0;
                while (enumerator1.MoveNext() && enumerator2.MoveNext())
                {
                    if (0 != comparer.Compare(enumerator1.Current, enumerator2.Current))
                    {
                        reason = (string)FrameworkMessages.ElementsAtIndexDontMatch((object)num);
                        return false;
                    }
                    ++num;
                }
                reason = (string)FrameworkMessages.BothenumerationsSameElements;
                return true;
            }
            reason = (string)FrameworkMessages.BothenumerationsSameReference((object)string.Empty);
            return true;
        }

        private class ObjectComparer : IComparer
        {
            int IComparer.Compare(object x, object y)
            {
                return !object.Equals(x, y) ? -1 : 0;
            }
        }
    }

}