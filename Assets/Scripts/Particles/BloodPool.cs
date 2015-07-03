using UnityEngine;

namespace Assets.Scripts.Particles
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class BloodPool : MonoBehaviour
    {
        public float Speed = 3f;

        private Color _color;
        private Vector3 _spread;

        public void Setup(Vector3 spread, Color color)
        {
            _color = color;
            _spread = spread;
        }

        void Start()
        {
            var spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.color = _color;
        }

        void Update ()
        {
            transform.position = Vector3.MoveTowards(transform.position, _spread, Speed * Time.deltaTime);

            if(transform.position == _spread)
                Destroy(this);
        }
    }
}