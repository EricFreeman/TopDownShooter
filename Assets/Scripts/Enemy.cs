﻿using Assets.Scripts.Messages;
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
            var bullet = collision.gameObject.GetComponent<Bullet>();
            if (bullet != null)
            {
                Health -= bullet.Damage;

                EventAggregator.SendMessage(new SpawnDamageTextMessage
                    {
                        Position = transform.position,
                        Text = bullet.Damage.ToString("N0")
                    });

                var zombieBloodColor = new Color(0.25f, 0.31f, 0.18f, 1f);

                if (Health > 0)
                {
                    EventAggregator.SendMessage(new SpawnGibsMessage {Count = 1, Position = transform.position});
                    EventAggregator.SendMessage(new SpawnBloodMessage { Position = transform.position, Color = zombieBloodColor });
                }
                else
                {
                    Destroy(gameObject);
                    EventAggregator.SendMessage(new SpawnBloodMessage
                        {
                            Position = transform.position,
                            SplatterSize = SplatterSize.Medium,
                            Color = zombieBloodColor
                        });
                    EventAggregator.SendMessage(new SpawnGibsMessage {Count = 5, Position = transform.position});
                }
            }
        }
    }
}