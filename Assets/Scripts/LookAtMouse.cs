using UnityEngine;

namespace Assets.Scripts
{
    public class LookAtMouse : MonoBehaviour
    {
        void FixedUpdate()
        {
            var playerPlane = new Plane(Vector3.forward, transform.position);
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float hitdist;
            if (playerPlane.Raycast(ray, out hitdist))
            {
                var targetPoint = ray.GetPoint(hitdist);
                
                var newRotation = Quaternion.LookRotation(transform.position - targetPoint, Vector3.forward);
                newRotation.x = 0;
                newRotation.y = 0;
                transform.rotation = newRotation;
            }
        }
    }
}
