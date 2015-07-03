using System;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Animator))]
    public class EnemyMovement : MonoBehaviour 
    {
        public float FieldOfView = 60f;
        public float ViewDistance = 10;
        public float MoveSpeed = 2f;
        public float MeleeWeaponRange = 2.5f;
        public EnemyState State = EnemyState.Patrolling;

        private GameObject _player;
        private Vector3 _lastKnownLocation;

        void Start()
        {
            _player = FindObjectOfType<Player>().gameObject;
        }

        void FixedUpdate()
        {
            switch (State)
            {
                case EnemyState.Attacking:
                    AttackingState();
                    break;
                case EnemyState.Idle:
                    if (CanSeePlayer) State = EnemyState.Detect;
                    break;
                case EnemyState.Patrolling:
                    if (CanSeePlayer) State = EnemyState.Detect;
                    break;
                case EnemyState.Searching:
                    SearchState();
                    break;
                case EnemyState.Detect:
                    AlertState();
                    break;
            }
        }

        private void AttackingState()
        {
            var animator = GetComponent<Animator>();
            var playerPosition = _player.transform.position;

            var distance = Vector3.Distance(playerPosition, transform.position);
            if (distance <= MeleeWeaponRange)
            {
                animator.SetBool("IsAttacking", true);
                animator.SetFloat("Forward", 0f, 0.1f, 0.5f);
            }
            else
            {
                animator.SetBool("IsAttacking", false);
                State = EnemyState.Detect;
            }
        }

        private bool CanSeePlayer
        {
            get
            {
                if (Vector3.Distance(transform.position, _player.transform.position) < ViewDistance)
                {
                    RaycastHit info;
                    var hit = Physics.Linecast(transform.position + new Vector3(0, .3f, 0), _player.transform.position, out info);
                    if (hit && info.collider.tag == "Player")
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        private void SearchState()
        {
            if (CanSeePlayer)
            {
                State = EnemyState.Detect;
            }

            LookTowardsPosition(_lastKnownLocation);
        }

        private void AlertState()
        {
            if (!CanSeePlayer)
            {
                transform.position = Vector3.MoveTowards(transform.position, _lastKnownLocation, MoveSpeed * Time.deltaTime);

                if (transform.position == _lastKnownLocation)
                    State = EnemyState.Searching;
            }
            else
            {
                _lastKnownLocation = _player.transform.position;

                var playerPosition = _player.transform.position;

                var distance = Vector3.Distance(playerPosition, transform.position);
                if (distance <= MeleeWeaponRange)
                {
                    State = EnemyState.Attacking;
                }
                else
                {
                    MoveCloserToDestination(playerPosition);
                }

                LookTowardsPosition(_player.transform.position);
            }
        }

        private void MoveCloserToDestination(Vector3 destination)
        {
            var speed = MoveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, destination, speed);

            var animator = GetComponent<Animator>();

            var forwardAmount = destination.z;
            animator.SetFloat("Forward", forwardAmount, 0.1f, speed);
        }

        private void LookTowardsPosition(Vector3 position)
        {
            var targetRotationRadians = Math.Atan2(position.x - transform.position.x, position.z - transform.position.z);
            var targetRotationDegrees = targetRotationRadians*(180.0/Math.PI);
            transform.localEulerAngles = new Vector3(0, (float)targetRotationDegrees, 0);
        }
    }

    public enum EnemyState
    {
        Idle,
        Patrolling,
        Searching,
        Detect,
        Dead,
        Attacking
    }
}