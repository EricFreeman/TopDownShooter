using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class EnemyMovement : MonoBehaviour 
    {
        private GameObject _player;
        public float FieldOfView = 60f;
        public float ViewDistance = 10;
        public float MoveSpeed = 2f;
        public float TurnSpeed = 6f;

        public EnemyState State = EnemyState.Patrolling;
        private Vector3 _lastKnownLocation;

        private bool CanSeePlayer
        {
            get
            {
                if (Vector3.Distance(transform.position, _player.transform.position) < ViewDistance)
                {
                    RaycastHit info;
                    var hit = Physics.Linecast(transform.position + new Vector3(0, -.3f, 0), _player.transform.position, out info);
                    if (hit && info.collider.tag == "Player")
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        void Start()
        {
            _player = FindObjectOfType<Player>().gameObject;
        }

        void FixedUpdate()
        {
            switch (State)
            {
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
                transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, MoveSpeed * Time.deltaTime);

                LookTowardsPosition(_player.transform.position);
            }
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
        Dead
    }
}