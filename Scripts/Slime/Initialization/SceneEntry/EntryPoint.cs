using Slime.Data.IDs;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Slime.Initialization.SceneEntry
{
    public class EntryPoint : MonoBehaviour
    {
        protected virtual void Awake()
        {
            if (FindObjectOfType<Bootstrap>() == null)
            {
                SceneManager.LoadSceneAsync(SceneIDs.BOOTSTRAP, LoadSceneMode.Additive);
            }
        }
    }
}