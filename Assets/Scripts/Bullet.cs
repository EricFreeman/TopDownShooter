using UnityEngine;

namespace Assets.Scripts
{
    public class Bullet : MonoBehaviour
    {
        public float Speed;

        void Update()
        {
            transform.position += (transform.up.normalized * Speed * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider col)
        {
            Destroy(gameObject);
        }
    }
}