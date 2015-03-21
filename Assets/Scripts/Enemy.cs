using UnityEngine;

namespace Assets.Scripts
{
    public class Enemy : MonoBehaviour
    {
        public Player Player;
        public float Speed = 3f;

        void Update()
        {
            if (Player == null)
            {

                Player = FindObjectOfType<Player>();
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, Player.transform.position,
                    Speed*Time.deltaTime);
            }
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            Debug.Log(col.name);
        }
    }
}