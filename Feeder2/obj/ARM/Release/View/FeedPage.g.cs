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
    partial class FeedPage : global::Windows.UI.Xaml.Controls.Page, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 29 "..\..\..\View\FeedPage.xaml"
                ((global::Feeder.PivotApp.View.ArticlesControl)(target)).ItemClicked += this.ListView_ItemClick;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 35 "..\..\..\View\FeedPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.btnUpdate_Click;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 36 "..\..\..\View\FeedPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.btnClear_Click;
                 #line default
                 #line hidden
                break;
            case 4:
                #line 39 "..\..\..\View\FeedPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.btnMarkAllAsRead_Click;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


