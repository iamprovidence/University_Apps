namespace Galagram.ViewModel.Commands.Registration
{
    /// <summary>
    /// Checks if name and password are correct
    /// <para/>
    /// Checks if nickname is available
    /// <para/>
    /// If everything is correct, log in a user
    /// </summary>
    public class SignUpCommand : CommandBase
    {
        // FIELDS
        ViewModel.RegistrationViewModel registrationViewModel;

        // CONSTRUCTORS
        /// <summary>
        /// Initializes a new instance of <see cref="SignUpCommand"/>
        /// </summary>
        /// <param name="registrationViewModel">
        /// An instance of <see cref="ViewModel.RegistrationViewModel"/>
        /// </param>
        public SignUpCommand(ViewModel.RegistrationViewModel registrationViewModel)
        {
            this.registrationViewModel = registrationViewModel;
        }

        // METHODS
        /// <summary>
        /// Checks if command can be executed
        /// </summary>
        /// <param name="parameter">
        /// Additionals parameters
        /// </param>
        /// <returns>
        /// True if command can be executed, otherwise — false
        /// </returns>
        public override bool CanExecute(object parameter)
        {
            Core.Logger.GetLogger.LogAsync(Core.LogMode.Debug, $"Can Exetute {nameof(SignUpCommand)}");

            return true;
        }
        /// <summary>
        /// Executes the command
        /// </summary>
        /// <param name="parameter">
        /// Command parameter
        /// </param>
        public override void Execute(object parameter)
        {
            Core.Logger.GetLogger.LogAsync(Core.LogMode.Debug, $"Exetute {nameof(SignUpCommand)}");
            // checking
            if (!registrationViewModel.IsDataValid()) return;

            if (!registrationViewModel.UnitOfWork.UserRepository.IsNicknameFree(registrationViewModel.Nickname))
            {
                registrationViewModel.WindowManager.ShowMessageWindow(Core.Messages.Info.ViewModel.NICKNAME_IS_NOT_AVAILABLE);
                Core.Logger.GetLogger.LogAsync(Core.LogMode.Debug, $"User can not sign up, because his nickname is not available");
                return;
            }


            // create user
            Core.Logger.GetLogger.LogAsync(Core.LogMode.Debug, "The validation has been passed. Create new user.");

            DataAccess.Entities.User user = new DataAccess.Entities.User()
            {
                NickName = registrationViewModel.Nickname,
                Password = registrationViewModel.Password
            };
            Core.Logger.GetLogger.LogAsync(Core.LogMode.Info, $"User with nickname {user.NickName} and password {user.Password} has been created");

            // add the user
            Core.Logger.GetLogger.LogAsync(Core.LogMode.Debug, "Add new user.");
            registrationViewModel.UnitOfWork.UserRepository.Insert(user);

            // save new user
            Core.Logger.GetLogger.LogAsync(Core.LogMode.Debug, "Save the user.");
            registrationViewModel.UnitOfWork.Save();

            // sets data
            registrationViewModel.DataStorage.LoggedUser = user;
            registrationViewModel.DataStorage.ShownUser = user;

            // open new window with current user
            Core.Logger.GetLogger.LogAsync(Core.LogMode.Debug, "User signed up. Registration window close. Main window opens.");
            registrationViewModel.WindowManager.SwitchMainWindow(
                key: nameof(Galagram.Window.User.MainWindow),
                viewModel: new ViewModel.User.MainWindowViewModel());
        }
    }
}
