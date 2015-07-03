using System;
using Assets.Scripts.Messages;
using UnityEngine;
using UnityEventAggregator;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Collider))]
    public class MeleeWeapon : MonoBehaviour
    {
        void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.name == "Player")
            {
                Debug.Log("Melee collision!");
                EventAggregator.SendMessage(new AcceptMeleeHitMessage {Collision = collision});
            }
        }
    }
}