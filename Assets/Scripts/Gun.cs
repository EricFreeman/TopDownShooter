using System.ComponentModel;
using UnityEngine;

namespace Assets.Scripts
{
    public class Gun : MonoBehaviour
    {
        public GameObject BulletGameObject;
        public GameObject TipGameObject;
        public GameObject MuzzleFlareLight;

        private Light _muzzleFlare;

        public float ShotDelay = .1f;
        private float _lastShot;

        void Start()
        {
            if (MuzzleFlareLight != null)
            {
                _muzzleFlare = MuzzleFlareLight.GetComponent<Light>();
            }
        }

        public void Fire()
        {
            if (CanFire())
            {
                var bullet = Instantiate(BulletGameObject);
                bullet.transform.position = TipGameObject.transform.position;
                bullet.transform.rotation = transform.GetComponentInChildren<SpriteRenderer>().transform.rotation;

                _lastShot = Time.fixedTime;

                if (_muzzleFlare != null)
                {
                    _muzzleFlare.intensity = 3;
                }
            }
        }

        private bool CanFire()
        {
            return Time.fixedTime - _lastShot > ShotDelay;
        }
    }
}