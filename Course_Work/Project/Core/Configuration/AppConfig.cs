using System.IO;

namespace Core.Configuration
{
    /// <summary>
    /// Consists of all important configuration to application.
    /// </summary>
    public static class AppConfig
    {
        /// <summary>
        /// Name of the application.
        /// </summary>
        public static readonly string APP_NAME = "Galagram";
        /// <summary>
        /// Determines if need to close application on fatal error
        /// </summary>
        public static readonly bool DO_CLOSE_APP_ON_FATAL_ERROR = false;

        // FOLDER CONFIG
        #region FOLDER CONFIG
        /// <summary>
        /// A directory separator depending on environment
        /// </summary>
        public static readonly string DIRECTORY_SEPARATOR_STR = Path.DirectorySeparatorChar.ToString();
        /// <summary>
        /// A path to executing file.
        /// </summary>
        public static readonly string DIRECTORY_EXE_PATH = System.AppDomain.CurrentDomain.BaseDirectory;
        #endregion

        // LOG
        #region LOG
        internal static readonly string LOG_DIRECTORY = string.Join(DIRECTORY_SEPARATOR_STR, DIRECTORY_EXE_PATH, "Log");
        /// <summary>
        /// A path to a file with logs.
        /// </summary>
        public static readonly string LOG_FILE = string.Join(DIRECTORY_SEPARATOR_STR, LOG_DIRECTORY, "log.log");
        /// <summary>
        /// The size limit of log file in bytes.
        /// </summary>
        public static readonly long LOG_FILE_SIZE_LIMIT = Converters.FileSizeConverter.GetSize(10, Enums.FileSizeMode.MB); // 10 MB in bytes
        /// <summary>
        /// The log modes that have to be off.
        /// </summary>
        public static readonly LogMode OFF_LOG_MODES = 0;
        /// <summary>
        /// Template for logs.
        /// <para/>
        /// Date, logMode, message.
        /// </summary>
        public static readonly string LOG_TEMPLATE_FORMAT = "-{0}- [{1}] \t {2}" + System.Environment.NewLine;
        #endregion

        // AVATAR
        #region AVATAR
        /// <summary>
        /// Folder with avatars
        /// </summary>
        public static readonly string AVATAR_FOLDER = string.Join(DIRECTORY_SEPARATOR_STR, DIRECTORY_EXE_PATH, "Avatars");
        /// <summary>
        /// A path to avatar image
        /// <para/>
        /// {0} � avatar name, user id <para/>
        /// {1} � extension with . <para/>
        /// </summary>
        public static readonly string AVATAR_FORMAT = string.Join(DIRECTORY_SEPARATOR_STR, AVATAR_FOLDER, "{0}{1}");
        /// <summary>
        /// Determines attributes for avatars folder
        /// </summary>
        public static readonly FileAttributes AVATAR_FOLDER_ATTRIBUTES = FileAttributes.Directory | FileAttributes.Hidden;
        #endregion

        // PHOTOS
        #region PHOTOS
        /// <summary>
        /// Folder with photos
        /// </summary>
        public static readonly string PHOTOS_SAVE_FOLDER = string.Join(DIRECTORY_SEPARATOR_STR, DIRECTORY_EXE_PATH, "Images");
        /// <summary>
        /// A path to saved photos.
        /// <para/>
        /// ImageFolder/UserIdFolder/image.jpg
        /// <para/>
        /// {0} � user id   <para/>
        /// {1} � photo name  <para/>
        /// {2} � extension with . <para/>
        /// </summary>
        public static readonly string PHOTOS_SAVE_PATH_FORMAT = string.Join(DIRECTORY_SEPARATOR_STR, PHOTOS_SAVE_FOLDER, "{0}", "{1}{2}");
        /// <summary>
        /// Determines attributes for photos folder
        /// </summary>
        public static readonly FileAttributes PHOTOS_FOLDER_ATTRIBUTES = FileAttributes.Directory | FileAttributes.Hidden;
        #endregion

        // TEMP
        #region TEMP
        /// <summary>
        /// Folder with temporary files
        /// </summary>
        public static readonly string TEMP_FOLDER = string.Join(DIRECTORY_SEPARATOR_STR, DIRECTORY_EXE_PATH, "Temp");
        /// <summary>
        /// A path to temporary file
        /// <para/>
        /// {0} � temp file name <para/>
        /// {1} � extension with . <para/>
        /// </summary>
        public static readonly string TEMP_FILE_FORMAT = string.Join(DIRECTORY_SEPARATOR_STR, TEMP_FOLDER, "{0}{1}");
        #endregion
    }
}
