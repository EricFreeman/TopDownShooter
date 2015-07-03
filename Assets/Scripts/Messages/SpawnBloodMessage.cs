using UnityEngine;

namespace Assets.Scripts.Messages
{
    public class SpawnBloodMessage
    {
        public Color Color;
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