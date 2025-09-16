using System;

namespace ButbleConfig
{
    [AttributeUsage(AttributeTargets.Field)]
    public class RecordKeyFieldAttribute : Attribute
    {
        public int Priority { get; private set; }

        public RecordKeyFieldAttribute(int Priority)
        {        
            this.Priority = Priority;
        }
    }
}