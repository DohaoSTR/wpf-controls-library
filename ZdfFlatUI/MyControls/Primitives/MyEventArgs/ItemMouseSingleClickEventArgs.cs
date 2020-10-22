using System;

namespace ZdfFlatUI.MyControls.Primitives
{
    public class ItemMouseSingleClickEventArgs<T> : EventArgs
    {
        public ItemMouseSingleClickEventArgs() { }

        public T NewValue { get; private set; }

        public static ItemMouseSingleClickEventArgs<T> ItemSingleClick(T newValue)
        {
            return new ItemMouseSingleClickEventArgs<T>() { NewValue = newValue };
        }

        public static ItemMouseSingleClickEventArgs<T> ShowContextMenu()
        {
            return new ItemMouseSingleClickEventArgs<T>() { };
        }
    }
}
