﻿// WarHub licenses this file to you under the MIT license.
// See LICENSE file in the project root for more information.

namespace WarHub.GodMode.Windows
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();

            LoadApplication(new GodMode.App());
        }
    }
}
