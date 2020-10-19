using Atomus.Page.Menu.Controllers;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Atomus.Page.Menu.ViewModel
{
    public class DefaultMenuViewModel : MVVM.ViewModel
    {
        #region Declare
        private string appName;
        private string nickName;
        private string info;
        #endregion

        #region Property
        public ICore Core { get; set; }

        public ICommand MenuItemSelectedCommand { get; set; }

        public string AppName
        {
            get
            {
                return this.appName;
            }
            set
            {
                if (this.appName != value)
                {
                    this.appName = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string NickName
        {
            get
            {
                return this.nickName;
            }
            set
            {
                if (this.nickName != value)
                {
                    this.nickName = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string Info
        {
            get
            {
                this.GetPoint();

                return this.info;
            }
            set
            {
                if (this.info != value)
                {
                    this.info = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Color BackgroundColor
        {
            get
            {
                return ((string)Config.Client.GetAttribute("BackgroundColor")).ToColor();
            }
        }

        public ObservableCollection<MenuItem> MenuItems { get; set; }
        #endregion

        #region INIT
        public DefaultMenuViewModel()
        {
            this.appName = Factory.FactoryConfig.GetAttribute("Atomus", "ProjectName");
            this.nickName = (string)Config.Client.GetAttribute("Account.NICKNAME");
            this.info = (string)Config.Client.GetAttribute("Account.INFO");

            this.MenuItems = new ObservableCollection<MenuItem>();

            //this.MenuItemSelectedCommand = new Command(async () => await this.MenuItemSelected());
        }

        public DefaultMenuViewModel(ICore core) : this()
        {
            IAction _Core;

            this.Core = core;

            _Core = (IAction)Config.Client.GetAttribute("DebugPage");

            if (_Core != null)
                this.LoadMenu(-1, -1, _Core.GetAttributeDecimal("ASSEMBLY_ID"));
            else
                this.LoadMenu(-1, -1, -1);
        }

        #endregion

        #region IO
        private async void LoadMenu(decimal START_MENU_ID, decimal ONLY_PARENT_MENU_ID, decimal ASSEMBLY_ID)
        {
            Service.IResponse result;

            try
            {
                result = await this.Core.SearchAsync(START_MENU_ID, ONLY_PARENT_MENU_ID, ASSEMBLY_ID);

                if (result.Status == Service.Status.OK)
                    foreach (DataRow dataRow in result.DataSet.Tables[1].Rows)
                    {
                        this.MenuItems.Add(new MenuItem()
                        {
                            MenuID = (decimal)dataRow["MENU_ID"],
                            AssemblyID = dataRow["ASSEMBLY_ID"] != DBNull.Value ? (decimal)dataRow["ASSEMBLY_ID"] : -1,
                            VisibleOne = dataRow["VISIBLE_ONE"] != DBNull.Value ? ((string)dataRow["VISIBLE_ONE"]) == "Y" : true,
                            Title = (string)dataRow["NAME"]
                        });
                    }
                else
                    await Application.Current.MainPage.DisplayAlert("Warning", result.Message, "OK");
            }
            finally
            {
            }
        }

        public async void GetPoint()
        {
            Service.IResponse result;

            try
            {
                result = await this.Core.SearchInfoAsync();

                if (result.Status == Service.Status.OK && result.DataSet != null && result.DataSet.Tables.Count > 0)
                {
                    foreach (DataRow dataRow in result.DataSet.Tables[0].Rows)
                        this.Info = (string)dataRow["info"];

                    if ((int)result.DataSet.Tables[0].Rows[0]["LEVEL_UP_COUNT"] > 0)
                    {
                        try
                        {
                            // Use default vibration length
                            Vibration.Vibrate();

                            // Or use specified time
                            //var duration = TimeSpan.FromSeconds(1);
                            Vibration.Vibrate();
                        }
                        catch (FeatureNotSupportedException ex)
                        {
                            // Feature not supported on device
                        }
                        catch (Exception ex)
                        {
                            // Other error has occurred.
                        }

                        await Application.Current.MainPage.DisplayAlert("Level Up !!", "포인트 차감률이 낮아 졌습니다.", "OK");
                    }
                }
                else
                    await Application.Current.MainPage.DisplayAlert("Warning", result.Message, "OK");
            }
            finally
            {
            }
        }
        #endregion

        #region ETC
        #endregion
    }
}
