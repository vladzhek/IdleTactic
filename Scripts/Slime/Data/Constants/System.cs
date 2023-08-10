using System.Diagnostics.CodeAnalysis;

namespace Slime.Data.Constants
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static class System
    {
        private const string VERSION = "1";
        private const string PROJECT = "Tank Assault";
        public const string ASSET_PATH = PROJECT + "/Configs/";

        private const string CONFIG_VERSION = VERSION;
        public const string CONFIG_PATH = "Configs/Version" + CONFIG_VERSION + "/";
        
        private const string SAVE_VERSION = VERSION;
        public const string SAVE_PATH = "saves/v" + SAVE_VERSION + "/";
        public const string SAVE_NAME = "save";
        public const string SAVE_EXTENSION = "tnk";

        public const string RESOURCE_TOKEN = "GREEDISGOOD";
    }
}