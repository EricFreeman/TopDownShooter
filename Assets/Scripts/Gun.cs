using Assets.Scripts.Messages;
using UnityEngine;
using UnityEventAggregator;

namespace Assets.Scripts
{
    public class Gun : MonoBehaviour, IListener<GainAmmunitionMessage>
    {
        public int Ammunition;
        public GameObject BulletGameObject;
        public GameObject TipGameObject;
        public GameObject MuzzleFlareLight;

        private Light _muzzleFlare;

        public float ShotDelay = .1f;
        private float _lastShot;

        void Start()
        {
            this.Register<GainAmmunitionMessage>();

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
                bullet.GetComponent<Bullet>().Damage = Random.Range(1, 5);

                _lastShot = Time.fixedTime;

                Ammunition--;

                if (_muzzleFlare != null)
                {
                    _muzzleFlare.intensity = 3;
                }
            }
        }

        private bool CanFire()
        {
            var ammunitionIsNotEmpty = Ammunition > 0;
            var itHasBeenLongEnoughSinceTheLastShot = Time.fixedTime - _lastShot > ShotDelay;
            
            return ammunitionIsNotEmpty && itHasBeenLongEnoughSinceTheLastShot;
        }

        public void Handle(GainAmmunitionMessage message)
        {
            Ammunition += message.Quantity;
        }
    }
}