using System;
using System.Collections.Generic;
using System.Reflection;

namespace ButbleConfig
{
    public class KeyInfo : IComparable<KeyInfo>
    {
        public int Priority;
        public FieldInfo FieldInfo;

        public KeyInfo(int Priority, FieldInfo FieldInfo)
        {
            this.Priority = Priority;
            this.FieldInfo = FieldInfo;
        }

        public int CompareTo(KeyInfo other)
            => Priority.CompareTo(other.Priority);
    }

    public class RecordComparer<T> : IComparer<T> where T : class
    {
        private readonly List<KeyInfo> keyInfos = new();

        public RecordComparer()
        {
            var fieldInfos = typeof(T).GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            foreach (var fieldInfo in fieldInfos)
            {
                var attr = fieldInfo.GetCustomAttribute<RecordKeyFieldAttribute>();
                if (attr != null)
                    keyInfos.Add(new(attr.Priority, fieldInfo));
            }

            keyInfos.Sort();
        }

        public int Compare(T value, T target)
        {
            int result = 0;
            for (int i = 0; i < keyInfos.Count; i++)
            {
                object expectedValue = keyInfos[i].FieldInfo.GetValue(value);
                object actualValue = keyInfos[i].FieldInfo.GetValue(target);

                result = ((IComparable)expectedValue).CompareTo(actualValue);

                if (result != 0)
                    break;
            }

            return result;
        }

        public int Compare(T target, params object[] keyValues)
        {
            int comparison = 0;
            for (int i = 0; i < keyInfos.Count; i++)
            {
                object expectedValue = keyValues[i];
                object actualValue = keyInfos[i].FieldInfo.GetValue(target);

                comparison = ((IComparable)expectedValue).CompareTo(actualValue);

                if (comparison != 0)
                    break;
            }

            return comparison;
        }
    }
}