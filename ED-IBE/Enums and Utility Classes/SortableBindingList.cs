using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace RegulatedNoise
{
    public class SortableBindingList<T> : BindingList<T>
    {
        private ArrayList sortedList;
        private ArrayList unsortedItems = null;
        private bool isSortedValue;

        public SortableBindingList()
        {
        }

        public SortableBindingList(IList<T> list)
        {
            foreach (object o in list)
            {
                this.Add((T)o);
            }
        }

        protected override bool SupportsSearchingCore
        {
            get
            {
                return true;
            }
        }

        protected override int FindCore(PropertyDescriptor prop, object key)
        {
            PropertyInfo propInfo = typeof(T).GetProperty(prop.Name);
            T item;

            if (key != null)
            {
                for (int i = 0; i < Count; ++i)
                {
                    item = (T)Items[i];
                    if (propInfo.GetValue(item, null).Equals(key))
                        return i;
                }
            }
            return -1;
        }

        public int Find(string property, object key)
        {
            PropertyDescriptorCollection properties =
                TypeDescriptor.GetProperties(typeof(T));
            PropertyDescriptor prop = properties.Find(property, true);

            if (prop == null)
                return -1;
            else
                return FindCore(prop, key);
        }

        protected override bool SupportsSortingCore
        {
            get { return true; }
        }


        protected override bool IsSortedCore
        {
            get { return isSortedValue; }
        }

        ListSortDirection sortDirectionValue;
        PropertyDescriptor sortPropertyValue;

        protected override void ApplySortCore(PropertyDescriptor prop,
            ListSortDirection direction)
        {

            sortedList = new ArrayList();

            Type interfaceType = prop.PropertyType.GetInterface("IComparable");

            if (interfaceType == null && prop.PropertyType.IsValueType)
            {
                Type underlyingType = Nullable.GetUnderlyingType(prop.PropertyType);

                if (underlyingType != null)
                {
                    interfaceType = underlyingType.GetInterface("IComparable");
                }
            }

            if (interfaceType != null)
            {
                sortPropertyValue = prop;
                sortDirectionValue = direction;

                IEnumerable<T> query = base.Items;
                if (direction == ListSortDirection.Ascending)
                {
                    query = query.OrderBy(i => prop.GetValue(i));
                }
                else
                {
                    query = query.OrderByDescending(i => prop.GetValue(i));
                }
                int newIndex = 0;
                foreach (object item in query)
                {
                    this.Items[newIndex] = (T)item;
                    newIndex++;
                }
                isSortedValue = true;
                this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));

            }
            else
            {
                throw new NotSupportedException("Cannot sort by " + prop.Name +
                    ". This" + prop.PropertyType.ToString() +
                    " does not implement IComparable");
            }
        }

        protected override void RemoveSortCore()
        {
            int position;
            object temp;

            if (unsortedItems != null)
            {
                for (int i = 0; i < unsortedItems.Count; )
                {
                    position = this.Find("LastName",
                        unsortedItems[i].GetType().
                        GetProperty("LastName").GetValue(unsortedItems[i], null));
                    if (position > 0 && position != i)
                    {
                        temp = this[i];
                        this[i] = this[position];
                        this[position] = (T)temp;
                        i++;
                    }
                    else if (position == i)
                        i++;
                    else
                        unsortedItems.RemoveAt(i);
                }
                isSortedValue = false;
                OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
            }
        }

        public void RemoveSort()
        {
            RemoveSortCore();
        }
        protected override PropertyDescriptor SortPropertyCore
        {
            get { return sortPropertyValue; }
        }

        protected override ListSortDirection SortDirectionCore
        {
            get { return sortDirectionValue; }
        }

    }
}
