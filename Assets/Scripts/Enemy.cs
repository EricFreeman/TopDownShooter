using Assets.Scripts.Messages;
using UnityEngine;
using UnityEventAggregator;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(EnemyMovement))]
    [RequireComponent(typeof(Rigidbody))]
    public class Enemy : MonoBehaviour
    {
        public float Speed = 3f;
        public float Health = 5;

        void OnCollisionEnter(Collision collision)
        {
            var enemyMovement = GetComponent<EnemyMovement>();

            if (enemyMovement.State != EnemyState.Dead)
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

                    var bloodSplatPosition = new Vector3(transform.position.x, 0.1f, transform.position.z);
                    if (Health > 0)
                    {
                        EventAggregator.SendMessage(new SpawnGibsMessage {Count = 1, Position = transform.position});

                        EventAggregator.SendMessage(new SpawnBloodMessage
                            {
                                Position = bloodSplatPosition,
                                Color = zombieBloodColor
                            });
                    }
                    else
                    {
                        var animator = GetComponent<Animator>();
                        animator.SetBool("IsDead", true);

                        enemyMovement.State = EnemyState.Dead;

                        var enemyRigidbody = GetComponent<Rigidbody>();
                        enemyRigidbody.isKinematic = true;

                        var meleeWeapons = GetComponentsInChildren<MeleeWeapon>();
                        foreach (var meleeWeapon in meleeWeapons)
                        {
                            meleeWeapon.IsActive = false;
                        }

                        EventAggregator.SendMessage(new SpawnBloodMessage
                            {
                                Position = bloodSplatPosition,
                                SplatterSize = SplatterSize.Medium,
                                Color = zombieBloodColor
                            });
                        EventAggregator.SendMessage(new SpawnGibsMessage {Count = 5, Position = transform.position});
                    }
                }
            }
        }
    }
}