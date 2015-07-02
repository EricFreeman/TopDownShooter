using Assets.Scripts.Messages;
using UnityEngine;
using UnityEventAggregator;

namespace Assets.Scripts
{
    public class Enemy : MonoBehaviour
    {
        public float Speed = 3f;
        public float Health = 5;

        void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.name == "Sprite")
            {
                var playerPosition = collision.collider.transform.position;
                EventAggregator.SendMessage(new SpawnDamageTextMessage
                {
                    Position = playerPosition,
                    Text = "Ow!"
                });
                EventAggregator.SendMessage(new SpawnDamageTextMessage
                {
                    Position = transform.position,
                    Text = "Ow!"
                });
                EventAggregator.SendMessage(new SpawnGibsMessage { Count = 1, Position = playerPosition });
                EventAggregator.SendMessage(new SpawnBloodMessage { Position = playerPosition });
                
            }
            else
            {
                var bullet = collision.gameObject.GetComponent<Bullet>();
                if (bullet != null)
                {
                    Health -= bullet.Damage;

                    EventAggregator.SendMessage(new SpawnDamageTextMessage
                        {
                            Position = transform.position,
                            Text = bullet.Damage.ToString("N0")
                        });

                    if (Health > 0)
                    {
                        EventAggregator.SendMessage(new SpawnGibsMessage {Count = 1, Position = transform.position});
                        EventAggregator.SendMessage(new SpawnBloodMessage {Position = transform.position});
                    }
                    else
                    {
                        Destroy(gameObject);
                        EventAggregator.SendMessage(new SpawnBloodMessage
                            {
                                Position = transform.position,
                                SplatterSize = SplatterSize.Medium
                            });
                        EventAggregator.SendMessage(new SpawnGibsMessage {Count = 5, Position = transform.position});
                    }
                }
            }
        }
    }
}