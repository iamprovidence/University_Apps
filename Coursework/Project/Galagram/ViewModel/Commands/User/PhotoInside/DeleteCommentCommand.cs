namespace Galagram.ViewModel.Commands.User.PhotoInside
{
    /// <summary>
    /// Deletes comments to photo
    /// </summary>
    public class DeleteCommentCommand : CommandBase
    {
        // FIELDS
        ViewModel.User.PhotoInsideViewModel photoInsideViewModel;

        // CONSTRUCTORS
        /// <summary>
        /// Initializes a new instance of <see cref="DeleteCommentCommand"/>
        /// </summary>
        /// <param name="photoInsideViewModel">
        /// An instance of <see cref="ViewModel.User.PhotoInsideViewModel"/>
        /// </param>
        public DeleteCommentCommand(ViewModel.User.PhotoInsideViewModel photoInsideViewModel)
        {
            this.photoInsideViewModel = photoInsideViewModel;
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
            Core.Logger.GetLogger.LogAsync(Core.LogMode.Debug, $"Can execute {nameof(DeleteCommentCommand)}");
            return true;
        }
        /// <summary>
        /// Executes command
        /// </summary>
        /// <param name="parameter">
        /// Command parameters
        /// <para/>
        /// 1 — command wrapper to delete
        /// </param>
        public override void Execute(object parameter)
        {
            Core.Logger.GetLogger.LogAsync(Core.LogMode.Debug, $"Execute {nameof(DeleteCommentCommand)}");

            // delete any comments on your photo
            // or
            // delete your comments on someone photo

            // delete comment
            Core.Logger.GetLogger.LogAsync(Core.LogMode.Debug, $"Convert to {nameof(DataAccess.Wrappers.CommentWrapper)}");
            DataAccess.Wrappers.CommentWrapper commentWrapperToDelete = (DataAccess.Wrappers.CommentWrapper)parameter;

            // remove from db
            Core.Logger.GetLogger.LogAsync(Core.LogMode.Debug, "Remove value from data base");
            photoInsideViewModel.UnitOfWork.CommentRepository.Delete(commentWrapperToDelete.Comment);
            
            // update view
            Core.Logger.GetLogger.LogAsync(Core.LogMode.Debug, "Update View");
            photoInsideViewModel.Comments.Remove(commentWrapperToDelete);

            // update data base
            Core.Logger.GetLogger.LogAsync(Core.LogMode.Debug, "Update Data base");
            photoInsideViewModel.UnitOfWork.UserRepository.Update(photoInsideViewModel.DataStorage.LoggedUser);
            photoInsideViewModel.UnitOfWork.PhotoRepository.Update(photoInsideViewModel.Photo);

            // save changes
            Core.Logger.GetLogger.LogAsync(Core.LogMode.Debug, "Save changes");
            photoInsideViewModel.UnitOfWork.Save();
        }
    }
}
