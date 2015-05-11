using UnityEngine;

namespace Assets.Scripts
{
    public class Player : MonoBehaviour
    {
        public float Speed;

        void Update()
        {
            var frameSpeed = Speed * Time.deltaTime;

            transform.Translate(Input.GetAxisRaw("Horizontal") * frameSpeed, 0, Input.GetAxisRaw("Vertical") * frameSpeed);

            if (Input.GetMouseButton(0))
            {
                var gun = GetComponent<Gun>();
                if (gun != null) gun.Fire();
            }
        }
    }
}