﻿

#pragma checksum "G:\dev\WindowsStore\Feeder\Feeder2\View\PivotPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "A44A566D7F3DB26FC5B6FB884ED803BE"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Feeder.PivotApp
{
    partial class PivotPage : global::Windows.UI.Xaml.Controls.Page, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 33 "..\..\..\View\PivotPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Pivot)(target)).SelectionChanged += this.pivot_SelectionChanged;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 99 "..\..\..\View\PivotPage.xaml"
                ((global::Feeder.PivotApp.View.ArticlesControl)(target)).ItemClicked += this.ArticlesControl_ItemClicked;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 96 "..\..\..\View\PivotPage.xaml"
                ((global::Feeder.PivotApp.View.ArticlesControl)(target)).ItemClicked += this.ArticlesControl_ItemClicked;
                 #line default
                 #line hidden
                break;
            case 4:
                #line 93 "..\..\..\View\PivotPage.xaml"
                ((global::Feeder.PivotApp.View.ArticlesControl)(target)).ItemClicked += this.ArticlesControl_ItemClicked;
                 #line default
                 #line hidden
                break;
            case 5:
                #line 45 "..\..\..\View\PivotPage.xaml"
                ((global::Windows.UI.Xaml.Controls.ListViewBase)(target)).ItemClick += this.ItemView_ItemClick;
                 #line default
                 #line hidden
                break;
            case 6:
                #line 88 "..\..\..\View\PivotPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.AddAppBarButton_Click;
                 #line default
                 #line hidden
                break;
            case 7:
                #line 55 "..\..\..\View\PivotPage.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).Holding += this.StackPanel_Holding;
                 #line default
                 #line hidden
                break;
            case 8:
                #line 57 "..\..\..\View\PivotPage.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).LostFocus += this.TextBox_LostFocus;
                 #line default
                 #line hidden
                break;
            case 9:
                #line 75 "..\..\..\View\PivotPage.xaml"
                ((global::Windows.UI.Xaml.Controls.MenuFlyoutItem)(target)).Click += this.MenuFlyoutItemDelete_Click;
                 #line default
                 #line hidden
                break;
            case 10:
                #line 76 "..\..\..\View\PivotPage.xaml"
                ((global::Windows.UI.Xaml.Controls.MenuFlyoutItem)(target)).Click += this.MenuFlyoutItemRename_Click;
                 #line default
                 #line hidden
                break;
            case 11:
                #line 107 "..\..\..\View\PivotPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.AddAppBarButton_Click;
                 #line default
                 #line hidden
                break;
            case 12:
                #line 108 "..\..\..\View\PivotPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.searchAppBarButton_Click;
                 #line default
                 #line hidden
                break;
            case 13:
                #line 109 "..\..\..\View\PivotPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.SettingsAppBarButton_Click;
                 #line default
                 #line hidden
                break;
            case 14:
                #line 112 "..\..\..\View\PivotPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.aboutButton_Click;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}

