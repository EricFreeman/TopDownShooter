using UnityEngine;

namespace Assets.Scripts
{
    public class MuzzleFlare : MonoBehaviour
    {
        private Light _muzzleFlareLight;

        void Start()
        {
            _muzzleFlareLight = GetComponent<Light>();
        }

        void Update()
        {
            if (_muzzleFlareLight.intensity > 0)
            {
                _muzzleFlareLight.intensity -= Time.deltaTime * 100;
            }
        }
    }
}