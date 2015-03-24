using UnityEngine;

namespace Assets.Scripts.Util
{
    public static class Extensions
    {
        public static Vector3 ToXY(this Vector3 v)
        {
            return new Vector3(v.x, v.y, 0);
        }
    }
}
