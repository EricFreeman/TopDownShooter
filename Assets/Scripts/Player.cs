using Assets.Scripts.Messages;
using UnityEngine;
using UnityEventAggregator;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Animator))]
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

        void FixedUpdate()
        {
            var frameSpeed = Speed * Time.deltaTime;

            var horizontalSpeed = Input.GetAxisRaw("Horizontal")*frameSpeed;
            var verticalSpeed = Input.GetAxisRaw("Vertical") * frameSpeed;
            var animator = GetComponentInChildren<Animator>();


            var move = verticalSpeed * Vector3.forward + horizontalSpeed * Vector3.right;
            if (move.magnitude > 1f)
            {
                move.Normalize();
            }
            animator.SetFloat("speed", move.magnitude * 100f);
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
            
            var bloodSplatPosition = new Vector3(transform.position.x, 0.1f, transform.position.z);
            EventAggregator.SendMessage(new SpawnBloodMessage { Position = bloodSplatPosition, Color = new Color(.67f, 0f, 0f, 1f) });
        }
    }
}