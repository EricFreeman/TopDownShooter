using UnityEngine;

namespace Assets.Scripts
{
    public class LookAtPoint : MonoBehaviour
    {
        public float RotationSpeed = 8;
        public Vector3 TargetPoint;

        void FixedUpdate()
        {
            var newRotation = Quaternion.LookRotation(transform.position - TargetPoint, Vector3.forward);
            newRotation.x = 0;
            newRotation.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * RotationSpeed);
        }
    }
}
