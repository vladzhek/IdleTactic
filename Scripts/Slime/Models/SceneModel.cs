using Slime.AbstractLayer.Models;
using UnityEngine;
using Logger = Utils.Logger;

namespace Slime.Models
{
    public class SceneModel : ISceneModel
    {
        public bool IsReady { get; private set; }
        public Transform Root { get; private set; }
        public Transform PlayerPosition { get; private set; }
        public Transform[] CompanionsPositions { get; private set; }
        public Transform[] WavesPositions { get; private set; }

        public void SetRoot(Transform root)
        {
            Root = root;
        }

        public void SetPlayerPosition(Transform playerPoint)
        {
            PlayerPosition = playerPoint;
        }

        public void SetCompanionsPositions(Transform[] companionsPoints)
        {
            CompanionsPositions = companionsPoints;
        }

        public void SetWavesPositions(Transform[] wavesPoints)
        {
            WavesPositions = wavesPoints;
        }

        public void SetReady(bool isReady)
        {
            IsReady = isReady && Root != null
                              && PlayerPosition != null
                              && CompanionsPositions != null
                              && WavesPositions != null;

            if (IsReady != isReady)
            {
                Logger.Error($"Some of scene components are not set properly\n" +
                                  $"Root {Root}\n" +
                                  $"PlayerPosition {PlayerPosition}\n" +
                                  $"CompanionsPositions {CompanionsPositions}\n" +
                                  $"WavesPositions {WavesPositions}\n");
            }
        }
    }
}