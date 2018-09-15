using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Reflection;

namespace MySilverlightLibrary
{
    public class Utility
    {
        public static Version GetVersion()
        {
            string name = Assembly.GetExecutingAssembly().FullName;
            AssemblyName asmName = new AssemblyName(name);

            return asmName.Version;
        }
    }
}
