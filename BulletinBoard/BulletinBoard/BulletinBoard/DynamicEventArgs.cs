using System;

namespace BulletinBoardStyle
{
    internal class DynamicEventArgs : EventArgs
    {
        public object Data { get; set; }

        public DynamicEventArgs(object data)
        {
            Data = data;
        }

        public static DynamicEventArgs Empty
        {
            get { return new DynamicEventArgs(null);}
        }
    }
}