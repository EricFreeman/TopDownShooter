using UnityEngine;

namespace Assets.Scripts.Particles
{
    public class BloodPool : MonoBehaviour
    {
        public float Speed = 3f;
        private Vector3 _spread;

        public void Setup(Vector3 spread)
        {
            _spread = spread;
        }

        void Update ()
        {
            transform.position = Vector3.MoveTowards(transform.position, _spread, Speed * Time.deltaTime);

            if(transform.position == _spread)
                Destroy(this);
        }
    }
}