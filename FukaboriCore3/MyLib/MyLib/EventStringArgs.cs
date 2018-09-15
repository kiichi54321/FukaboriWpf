using System;
using System.Collections.Generic;
using System.Text;

namespace MyLib.Event
{
    public class EventStringArgs : EventArgs
    {
        string text;

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public EventStringArgs(string str)
            : base()
        {
            this.text = str;
        }

    }

    public class EventGenericArgs<T> : EventArgs
    {
        T value;

        public T Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        public EventGenericArgs(T val)
        {

            value = val;
        }
    }

    public class Args<T> : EventArgs
    {
        T value;

        public T Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        public Args(T val)
        {

            value = val;
        }
    }
}

