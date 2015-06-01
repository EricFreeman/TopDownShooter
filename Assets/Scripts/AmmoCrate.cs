using Assets.Scripts.Messages;
using UnityEngine;
using UnityEventAggregator;

namespace Assets.Scripts
{
    public class AmmoCrate : MonoBehaviour
    {
        private float _health;

        void Start()
        {
            _health = 5f;
        }

        void OnTriggerEnter(Collider col)
        {
            var bullet = col.GetComponent<Bullet>();
            if (bullet != null)
            {
                _health -= bullet.Damage;
                if (_health <= 0)
                {
                    var quantity = 200;
                    var text = string.Format("+{0} ammo", quantity);
                    EventAggregator.SendMessage(new SpawnDamageTextMessage { Position = transform.position, Text = text });
                    EventAggregator.SendMessage(new GainAmmunitionMessage { Quantity = quantity });
                    Destroy(gameObject);
                }
            }
        }
    }
}