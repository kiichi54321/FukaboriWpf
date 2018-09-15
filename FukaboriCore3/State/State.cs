using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reactive.Bindings;

namespace FukaboriCore
{
    public static class State
    {
        public static ReactiveProperty<double> MaxImageViewNum { get; } = new ReactiveProperty<double>(10);
        public static ReactiveProperty<string> NameText { get; set; } = new ReactiveProperty<string>("");
        public static ReactiveProperty<int> WordLenghtNum { get; set; } = new ReactiveProperty<int>(5);
    }
}
