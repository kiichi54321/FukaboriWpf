using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;


namespace MyWpf
{
    //http://qiita.com/maenotti_99/items/4dddbc755efa74086b7c ネタ元

    /// <summary>
    /// Enum用のComboBoxのItem
    /// </summary>
    /// <typeparam name="Type"></typeparam>
    public class ComboBoxEnumItem<Type>
    {
        public Type Code;
        public string Name { get; set; }
    }
    public static class EnumLib
    {
        /// <summary>
        /// ComboBoxのためのEnumリスト作り
        /// </summary>
        /// <typeparam name="Types"></typeparam>
        /// <returns></returns>
        public static IEnumerable<ComboBoxEnumItem<Types>> MakeComboBoxEnum<Types>()
        {
            foreach (Types dow in Enum.GetValues(typeof(Types)))
            {
                yield return new ComboBoxEnumItem<Types>
                {
                    Code = dow,
                    Name = dow.ToString(),
                };
            }
        }
    }
}
