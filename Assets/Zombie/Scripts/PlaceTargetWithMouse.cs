using UnityEngine;

namespace Assets.Zombie.Scripts
{
    public class PlaceTargetWithMouse : MonoBehaviour
    {
        public float surfaceOffset = 1.5f;
        public GameObject setTargetOn;

        // Update is called once per frame
        private void Update()
        {
            if (!Input.GetMouseButtonDown(0))
            {
                return;
            }
            
            var playerPlane = new Plane(Vector3.up, transform.position);
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float hitdist;
            if (playerPlane.Raycast(ray, out hitdist))
            {
                var targetPoint = ray.GetPoint(hitdist);
                transform.position = targetPoint;
                setTargetOn.SendMessage("SetTarget", transform);
            }





            /*Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (!Physics.Raycast(ray, out hit))
            {
                Debug.Log("No ray cast!");
                return;
            }
            transform.position = hit.point + hit.normal * surfaceOffset;
            if (setTargetOn != null)
            {
                Debug.Log(string.Format("Set target to {0},{1},{2}", transform.position.x, transform.position.y, transform.position.z));
                setTargetOn.SendMessage("SetTarget", transform);
            }*/
        }
    }
}
