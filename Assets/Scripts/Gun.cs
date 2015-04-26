using UnityEngine;

namespace Assets.Scripts
{
    public class Gun : MonoBehaviour
    {
        public GameObject BulletGameObject;
        public GameObject TipGameObject;

        public float ShotDelay = .1f;
        private float _lastShot;

        public void Fire()
        {
            if (CanFire())
            {
                var bullet = Instantiate(BulletGameObject);
                bullet.transform.position = TipGameObject.transform.position;
                bullet.transform.rotation = transform.GetComponentInChildren<SpriteRenderer>().transform.rotation;

                _lastShot = Time.fixedTime;
            }
        }

        private bool CanFire()
        {
            return Time.fixedTime - _lastShot > ShotDelay;
        }
    }
}