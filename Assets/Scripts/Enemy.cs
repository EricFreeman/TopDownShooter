using Assets.Scripts.Messages;
using UnityEngine;
using UnityEventAggregator;

namespace Assets.Scripts
{
    public class Enemy : MonoBehaviour
    {
        public Player Player;
        public float Speed = 3f;
        public int Health = 5;

        void OnTriggerEnter2D(Collider2D col)
        {
            if (col.GetComponent<Bullet>() != null)
            {
                Health--;
                Destroy(col.gameObject);

                if (Health > 0)
                {
                    EventAggregator.SendMessage(new SpawnGibsMessage { Count = 1, Position = transform.position });
                    EventAggregator.SendMessage(new SpawnBloodMessage { Position = transform.position });
                }
                else
                {
                    Destroy(gameObject);
                    EventAggregator.SendMessage(new SpawnBloodMessage { Position = transform.position, SplatterSize = SplatterSize.Medium });
                    EventAggregator.SendMessage(new SpawnGibsMessage { Count = 5, Position = transform.position });
                }
            }
        }
    }
}