namespace Galagram.ViewModel.ViewModel.Admin
{
    /// <summary>
    /// A logic class for <see cref="Window.Admin.AdminWindow"/>
    /// <para/>
    /// Left menu logic
    /// </summary>
    public class AdminWindowViewModel : ViewModelBase
    {
        // FIELDS
        readonly string[] menuItems;
        int menuItemIndex;
        Services.MenuItemViewModelFactory menuItemViewModelFactory;

        System.Windows.Input.ICommand changeContentCommand;

        // CONSTRUCTORS
        /// <summary>
        /// Initialize a new instance of <see cref="AdminWindowViewModel"/>
        /// </summary>
        public AdminWindowViewModel()
        {
            menuItemViewModelFactory = new Services.MenuItemViewModelFactory();
            menuItemIndex = Core.Configuration.Constants.WRONG_INDEX;

            // sets menu items 
            menuItems = Core.Configuration.AdminConfig.ADMIN_ITEMS;

            // registrate ViewModel by menu items
            menuItemViewModelFactory.Registrate(menuItems[0], typeof(Message.AllViewModel));
            menuItemViewModelFactory.Registrate(menuItems[1], typeof(User.AllViewModel));
            menuItemViewModelFactory.Registrate(menuItems[2], typeof(Comments.AllViewModel));
            menuItemViewModelFactory.Registrate(menuItems[3], typeof(Subject.AllViewModel));

            // command
            changeContentCommand = new Commands.Admin.MainAdminWindowControl.SelectItemCommand(this);

            Logger.LogAsync(Core.LogMode.Debug, $"Initialized {nameof(AdminWindowViewModel)}");
        }

        // PROPERTIES
        /// <summary>
        /// Gets menu item - VM factory
        /// </summary>
        public Services.MenuItemViewModelFactory MenuItemViewModelFactory
        {
            get
            {
                Logger.LogAsync(Core.LogMode.Debug, $"Gets {nameof(MenuItemViewModelFactory)}");

                return menuItemViewModelFactory;
            }
        }
        /// <summary>
        /// Gets menu items
        /// </summary>
        public string[] MenuItems
        {
            get
            {
                Logger.LogAsync(Core.LogMode.Debug, $"Gets {nameof(MenuItems)}");

                return menuItems;
            }
        }
        /// <summary>
        /// Gets or sets menu item index
        /// </summary>
        public int MenuItemIndex
        {
            get
            {
                Logger.LogAsync(Core.LogMode.Debug, $"Gets {nameof(MenuItemIndex)}");
                Logger.LogAsync(Core.LogMode.Info, $"Value of {nameof(MenuItemIndex)} is {menuItemIndex}");

                return menuItemIndex;
            }
            set
            {
                Logger.LogAsync(Core.LogMode.Debug, $"Sets {nameof(MenuItemIndex)}");
                Logger.LogAsync(Core.LogMode.Info, $"Old value of {nameof(MenuItemIndex)} is {menuItemIndex}, new = {value}");

                SetProperty(ref menuItemIndex, value);

                changeContentCommand.Execute(null);
            }
        }
        /// <summary>
        /// Gets exit menu item index
        /// </summary>
        public int ExitIndex => Core.Configuration.AdminConfig.EXIT_ITEM_INDEX;

        // COMMANDS
        /// <summary>
        /// Gets command to change content
        /// </summary>
        public System.Windows.Input.ICommand ChangeContentCommand
        {
            get
            {
                Logger.LogAsync(Core.LogMode.Debug, $"Gets {nameof(ChangeContentCommand)}");

                return changeContentCommand;
            }
        }
    }
}
