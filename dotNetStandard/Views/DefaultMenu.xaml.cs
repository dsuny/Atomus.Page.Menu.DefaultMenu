using Atomus.Page.Menu.Controllers;
using Atomus.Page.Menu.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Atomus.Page.Menu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DefaultMenu : ContentPage, IAction
    {
        private AtomusPageEventHandler beforeActionEventHandler;
        private AtomusPageEventHandler afterActionEventHandler;

        #region Init
        public DefaultMenu()
        {
            this.BindingContext = new DefaultMenuViewModel(this);

            InitializeComponent();

            this.BackgroundColor = ((string)Config.Client.GetAttribute("BarBackgroundColor")).ToColor();
        }
        #endregion

        #region IO
        object IAction.ControlAction(ICore sender, AtomusPageArgs e)
        {
            try
            {
                this.beforeActionEventHandler?.Invoke(this, e);

                switch (e.Action)
                {
                    case "Menu.AddHome":
                        //((DefaultBrowserStandardMasterViewModel)this.BindingContext).MenuItems.Insert(0, new MenuItem() { Id = 0, Title = "Home", Page = (Xamarin.Forms.Page)e.Value });
                        return true;

                    case "Menu.LoadInfo":
                        (this.BindingContext as DefaultMenuViewModel).GetPoint();
                        return true;

                    default:
                        throw new AtomusException("'{0}'은 처리할 수 없는 Action 입니다.".Translate(e.Action));
                }
            }
            finally
            {
                this.afterActionEventHandler?.Invoke(this, e);
            }
        }
        #endregion

        #region Event
        event AtomusPageEventHandler IAction.BeforeActionEventHandler
        {
            add
            {
                this.beforeActionEventHandler += value;
            }
            remove
            {
                this.beforeActionEventHandler -= value;
            }
        }
        event AtomusPageEventHandler IAction.AfterActionEventHandler
        {
            add
            {
                this.afterActionEventHandler += value;
            }
            remove
            {
                this.afterActionEventHandler -= value;
            }
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            AtomusPageEventArgs atomusPageEventArgs;
            try
            {
                var item = e.SelectedItem as MenuItem;

                if (item == null)
                    return;

                (sender as ListView).SelectedItem = null;

                if (item.AssemblyID < 0)
                    return;

                if (item.Page == null)
                {
                    atomusPageEventArgs = new AtomusPageEventArgs("Menu.OpenControl", new object[] { item.MenuID, item.AssemblyID, item.VisibleOne });

                    this.afterActionEventHandler?.Invoke(this, atomusPageEventArgs);

                    if (atomusPageEventArgs.Value != null && atomusPageEventArgs.Value is Xamarin.Forms.Page)
                    {
                        item.Page = (Xamarin.Forms.Page)atomusPageEventArgs.Value;
                        item.Page.Title = item.Title;
                    }
                }

                this.afterActionEventHandler?.Invoke(this, new AtomusPageEventArgs("Menu.Select", item.Page));
            }
            catch (Exception ex)
            {
                Diagnostics.DiagnosticsTool.MyTrace(ex);
            }
        }
        #endregion

        #region Etc
        #endregion
    }
}