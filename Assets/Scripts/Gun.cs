using UnityEngine;

namespace Assets.Scripts
{
    public class Gun : MonoBehaviour
    {
        public GameObject BulletGameObject;
        public GameObject TipGameObject;

        void Update()
        {

        }

        public void Fire()
        {
            if (CanFire())
            {
                var bullet = Instantiate(BulletGameObject);
                bullet.transform.position = TipGameObject.transform.position;
                bullet.transform.rotation = transform.GetComponentInChildren<SpriteRenderer>().transform.rotation;
            }
        }

        private bool CanFire()
        {
            return true;
        }
    }
}