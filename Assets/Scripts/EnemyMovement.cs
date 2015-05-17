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
        private Seeker _seeker;

        private bool CanSeePlayer
        {
            get
            {
                if (Vector3.Distance(transform.position, _player.transform.position) < ViewDistance)
                {
                    RaycastHit info;
                    var hit = Physics.Linecast(transform.position + new Vector3(0, 0, -.3f), _player.transform.position, out info);
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
            _seeker = GetComponent<Seeker>();
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

        private bool onlyOnce;
        private void AlertState()
        {
            if (!CanSeePlayer)
            {
                //transform.position = Vector3.MoveTowards(transform.position, _lastKnownLocation, MoveSpeed * Time.deltaTime);
                _seeker.StartPath(transform.position, _lastKnownLocation);

                if (transform.position == _lastKnownLocation)
                    State = EnemyState.Searching;
            }
            else
            {
                _lastKnownLocation = _player.transform.position;
//                transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, MoveSpeed * Time.deltaTime);
                if(!onlyOnce)
                _seeker.StartPath(transform.position, _player.transform.position);
                onlyOnce = true;

                LookTowardsPosition(_player.transform.position);
            }
        }

        private void LookTowardsPosition(Vector3 position)
        {
            var targetRotationRadians = Math.Atan2(position.y - transform.position.y, position.x - transform.position.x);
            var targetRotationDegrees = targetRotationRadians*(180.0/Math.PI) - 90;
            transform.localEulerAngles = new Vector3(0, 0, (float) targetRotationDegrees);
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