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

        private bool _isDead;

        void Update()
        {
            if (_isDead) return;

            if (Player == null)
            {

                Player = FindObjectOfType<Player>();
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, Speed * Time.deltaTime);
                GetComponent<LookAtPoint>().TargetPoint = Player.transform.position;
            }
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            if (col.GetComponent<Bullet>() != null)
            {
                if (Health > 0)
                {
                    Destroy(col.gameObject);
                    EventAggregator.SendMessage(new SpawnBloodMessage { Position = transform.position });
                }

                Health--;
                
                if (Health <= 0) Die();
            }
        }

        private void Die()
        {
            _isDead = true;
        }
    }
}