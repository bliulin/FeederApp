﻿

#pragma checksum "g:\dev\WindowsStore\Feeder\Feeder2\View\Search\SearchPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "2FE611E6A250D56D24706B96762BB417"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Feeder.PivotApp.View.Search
{
    partial class SearchPage : global::Windows.UI.Xaml.Controls.Page, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 60 "..\..\..\..\View\Search\SearchPage.xaml"
                ((global::Feeder.PivotApp.View.ArticlesControl)(target)).ItemClicked += this.ArticlesControl_ItemClicked;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 57 "..\..\..\..\View\Search\SearchPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.searchBtn_Click;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


