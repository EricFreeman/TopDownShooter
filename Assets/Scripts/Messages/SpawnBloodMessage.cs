using UnityEngine;

namespace Assets.Scripts.Messages
{
    public class SpawnBloodMessage
    {
        public Vector3 Position;
        public SplatterSize SplatterSize;
    }

    public enum SplatterSize
    {
        Small,
        Medium,
        Large
    }
}