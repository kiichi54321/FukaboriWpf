﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace FukaboriWpf.View
{
    public partial class SimpleNeural : UserControl
    {
        public SimpleNeural()
        {
            InitializeComponent();
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            inputQuestionListBox.SelectAll();
        }
    }
}
