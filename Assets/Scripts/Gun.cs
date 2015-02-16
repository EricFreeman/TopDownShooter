using UnityEngine;

namespace Assets.Scripts
{
    public class Gun : MonoBehaviour
    {
        public GameObject BulletGameObject;

        void Update()
        {

        }

        public void Fire()
        {
            if (CanFire())
            {
                var bullet = (GameObject)Instantiate(BulletGameObject);
                bullet.transform.position = transform.position;
            }
        }

        private bool CanFire()
        {
            return true;
        }
    }
}