using System;
using Assets.Scripts.Messages;
using UnityEngine;
using UnityEventAggregator;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Collider))]
    public class MeleeWeapon : MonoBehaviour
    {
        public bool IsActive = true;

        void OnCollisionEnter(Collision collision)
        {
            if (IsActive && collision.collider.name == "Player")
            {
                EventAggregator.SendMessage(new AcceptMeleeHitMessage {Collision = collision});
            }
        }
    }
}