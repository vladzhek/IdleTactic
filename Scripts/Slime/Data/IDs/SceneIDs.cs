using Slime.Data.IDs.Abstract;

namespace Slime.Data.IDs
{
    public abstract class SceneIDs : IDList<SceneIDs>
    {
        public new static string Default => BOOTSTRAP;
        
        public const string BOOTSTRAP = "Bootstrap";
        public const string UI = "Ui";
        public const string GAMEPLAY = "WaveTest";
        public const string PREFABS = "Prefabs";
    }
}