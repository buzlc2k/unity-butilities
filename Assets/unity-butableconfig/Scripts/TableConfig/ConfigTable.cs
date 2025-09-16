using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace ButbleConfig
{
    public abstract class ConfigTable<T> : ScriptableObject where T : class
    {
        private RecordComparer<T> recordComparer;
        [SerializeField, SerializeReference] protected List<T> records = new();

        protected virtual void OnEnable()
        {
            InitComparer();
        }

        protected virtual void InitComparer()
            => recordComparer ??= new RecordComparer<T>();

        public virtual void Add(object record)
        {
            records.Add(record as T);
            records.Sort(recordComparer);
        }

        public List<T> GetAllRecord()
            => records;

        public virtual T Get(params object[] keyValues)
        {
            int resultIndex = records.R_BinarySearch(recordComparer, keyValues);

            if (resultIndex >= 0)
                return records[resultIndex];
            else
            {
                Debug.Log("Can't find element by key");
                return null;
            }
        }
    }
}