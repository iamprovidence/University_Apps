using System.Windows;
using System.Windows.Controls;

using MessageBoxButton = Galagram.Window.Enums.MessageBoxButton;

namespace Galagram.Window.Dialogs
{
    /// <summary>
    /// Interaction logic for MessageBoxOk.xaml
    /// </summary>
    public partial class MessageBox : System.Windows.Window, Interfaces.IMessageBox
    {
        // CONSTRUCTORS
        /// <summary>
        /// Initialize a new instance of <see cref="MessageBox"/>
        /// </summary>
        public MessageBox()
        {
            InitializeComponent();
        }
        // PROPERTIES
        /// <summary>
        /// Set up title for <see cref="MessageBox"/>
        /// </summary>
        public string Header
        {
            get
            {
                return HeaderLbl.Content.ToString();
            }
            set
            {
                HeaderLbl.Content = value;
            }
        }
        /// <summary>
        /// Set up message text for <see cref="MessageBox"/>
        /// </summary>
        public string Text
        {
            get
            {
                return ContentTb.Text;
            }
            set
            {
                ContentTb.Text = value;
            }
        }
        // METHODS
        private void MovingWindow(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
            {
                this.DragMove();
            }
        }
        private void Yes(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
        private void No(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
        /// <summary>
        /// Open a window and returns only when the newly opened window is closed
        /// </summary>
        /// <param name="messageBoxButton">
        ///  Specifies the buttons that are displayed on a message box. 
        /// </param>
        /// <returns>
        /// A <see cref="System.Nullable"/> value of type <see cref="System.Boolean"/> that specifies whether the activity was accepted (true) or canceled (false).
        /// <para/>
        /// The return value is the value of the <see cref="System.Windows.Window.DialogResult"/> property before a window closes.
        /// </returns>   
        public bool? ShowDialog(MessageBoxButton messageBoxButton)
        {
            // get button Resources
            Style buttonStyle = (Style)FindResource("ClickButton");

            // initialize buttons
            if (messageBoxButton == MessageBoxButton.Ok)
            {
                // ok button
                Button okButton = new Button()
                {
                    Content = "Ok",
                    Style = buttonStyle,
                    IsDefault = true
                };
                okButton.Click += Yes;
                // set position
                Grid.SetRow(okButton, 2);
                Grid.SetColumn(okButton, 0);
                // add to grid
                GridMain.Children.Add(okButton);
            }
            else if (messageBoxButton == MessageBoxButton.YesNo)
            {
                // 2 cols
                Grid grid = new Grid();
                grid.ColumnDefinitions.Add(new ColumnDefinition());
                grid.ColumnDefinitions.Add(new ColumnDefinition());

                Grid.SetColumn(grid, 0);
                Grid.SetRow(grid, 2);
                // yes and no buttons
                Button yesButton = new Button()
                {
                    Content = "Yes",
                    Style = buttonStyle,
                    IsDefault = true
                };
                yesButton.Click += Yes;

                Grid.SetRow(yesButton, 0);
                Grid.SetColumn(yesButton, 0);

                Button noButton = new Button()
                {
                    Content = "No",
                    Style = buttonStyle
                };
                noButton.Click += No;

                Grid.SetRow(noButton, 0);
                Grid.SetColumn(noButton, 1);

                // adding to the grid
                grid.Children.Add(yesButton);
                grid.Children.Add(noButton);

                GridMain.Children.Add(grid);
            }

            // show the window
            return this.ShowDialog();
        }
    }
}
