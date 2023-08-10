using UnityEngine;

namespace Slime.AbstractLayer.Models
{
    public interface ISceneModel
    {
        public bool IsReady { get; }
        public Transform Root { get; }
        public Transform PlayerPosition { get; }
        public Transform[] CompanionsPositions { get; }
        public Transform[] WavesPositions { get; }

        public void SetRoot(Transform root);
        public void SetPlayerPosition(Transform playerPoint);
        public void SetCompanionsPositions(Transform[] companionsPoints);
        public void SetWavesPositions(Transform[] wavesPoints);
        public void SetReady(bool isReady);
    }
}