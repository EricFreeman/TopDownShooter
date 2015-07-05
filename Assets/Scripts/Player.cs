using Assets.Scripts.Messages;
using UnityEngine;
using UnityEventAggregator;

namespace Assets.Scripts
{
    public class Player : MonoBehaviour, IListener<AcceptMeleeHitMessage>
    {
        public float Speed;

        void Start()
        {
            this.Register<AcceptMeleeHitMessage>();
        }

        void OnDestroy()
        {
            this.UnRegister<AcceptMeleeHitMessage>();
        }

        void Update()
        {
            var frameSpeed = Speed * Time.deltaTime;

            var horizontalSpeed = Input.GetAxisRaw("Horizontal")*frameSpeed;
            var verticalSpeed = Input.GetAxisRaw("Vertical")*frameSpeed;
            if (horizontalSpeed < 0.1f && verticalSpeed < 0.1f)
            {
                //var animator = GetComponentInChildren<Animator>();
                //animator.SetFloat("speed", 0f);
            }
            else
            {
                var animator = GetComponentInChildren<Animator>();
                animator.SetFloat("speed", 2f);
            }
            transform.Translate(horizontalSpeed, 0, verticalSpeed);

            if (Input.GetMouseButton(0))
            {
                var gun = GetComponent<Gun>();
                if (gun != null) gun.Fire();
            }
        }

        public void Handle(AcceptMeleeHitMessage message)
        {
            EventAggregator.SendMessage(new SpawnDamageTextMessage
            {
                Position = transform.position,
                Text = message.Collision.relativeVelocity.ToString("N0")
            });


            EventAggregator.SendMessage(new SpawnGibsMessage { Count = 1, Position = transform.position });
            EventAggregator.SendMessage(new SpawnBloodMessage { Position = transform.position, Color = new Color(.67f, 0f, 0f, 1f) });
        }
    }
}