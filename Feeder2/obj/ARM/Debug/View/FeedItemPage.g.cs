﻿

#pragma checksum "g:\dev\WindowsStore\Feeder\Feeder2\View\FeedItemPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "061431D7BA014BD5BE0D2326A4DB4F50"
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
    partial class FeedItemPage : global::Windows.UI.Xaml.Controls.Page, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 33 "..\..\..\View\FeedItemPage.xaml"
                ((global::Windows.UI.Xaml.Controls.WebView)(target)).NavigationStarting += this.WebView_NavigationStarting;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 43 "..\..\..\View\FeedItemPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.goToArticle;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 47 "..\..\..\View\FeedItemPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.btnSaveArticle_Click;
                 #line default
                 #line hidden
                break;
            case 4:
                #line 52 "..\..\..\View\FeedItemPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.btnShareLink_Click;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


