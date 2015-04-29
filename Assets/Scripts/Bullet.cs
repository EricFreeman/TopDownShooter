using UnityEngine;

namespace Assets.Scripts
{
    public class Bullet : MonoBehaviour
    {
        public float Speed;
        public float Damage = 1;

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