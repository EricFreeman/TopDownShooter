using UnityEngine;

namespace Assets.Scripts
{
    public class Player : MonoBehaviour
    {
        public float Speed;

        void Update()
        {
            var frameSpeed = Speed*Time.deltaTime;

            transform.Translate(Input.GetAxisRaw("Horizontal") * frameSpeed, Input.GetAxisRaw("Vertical") * frameSpeed, 0);      
        }
    }
}
