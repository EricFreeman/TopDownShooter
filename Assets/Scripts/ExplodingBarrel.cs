using Assets.Scripts.Messages;
using UnityEngine;
using UnityEventAggregator;

namespace Assets.Scripts
{
    public class ExplodingBarrel : MonoBehaviour
    {
        public float Health;

        public GameObject ExplosionGameObject;

        void Start()
        {
            Health = 10f;
        }

        void OnCollisionEnter(Collision collision)
        {
            var bullet = collision.gameObject.GetComponent<Bullet>();
            if (bullet != null)
            {
                Health -= bullet.Damage;
                if (Health <= 0)
                {
                    var position = transform.position;
                    EventAggregator.SendMessage(new SpawnExplosionMessage { Position = position });
                    Destroy(gameObject);
                }
            }
        }
    }
}