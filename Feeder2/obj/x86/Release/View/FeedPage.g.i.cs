﻿

#pragma checksum "G:\dev\WindowsStore\Feeder\Feeder2\View\FeedPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "0D19E2C0318D37000AC4AB70AFE42902"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Feeder.PivotApp.View
{
    partial class FeedPage : global::Windows.UI.Xaml.Controls.Page
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Feeder.PivotApp.View.ArticlesControl articlesControl; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.AppBarButton btnUpdate; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.AppBarButton btnClear; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.AppBarButton btnMarkAllAsRead; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private bool _contentLoaded;

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent()
        {
            if (_contentLoaded)
                return;

            _contentLoaded = true;
            global::Windows.UI.Xaml.Application.LoadComponent(this, new global::System.Uri("ms-appx:///View/FeedPage.xaml"), global::Windows.UI.Xaml.Controls.Primitives.ComponentResourceLocation.Application);
 
            articlesControl = (global::Feeder.PivotApp.View.ArticlesControl)this.FindName("articlesControl");
            btnUpdate = (global::Windows.UI.Xaml.Controls.AppBarButton)this.FindName("btnUpdate");
            btnClear = (global::Windows.UI.Xaml.Controls.AppBarButton)this.FindName("btnClear");
            btnMarkAllAsRead = (global::Windows.UI.Xaml.Controls.AppBarButton)this.FindName("btnMarkAllAsRead");
        }
    }
}



