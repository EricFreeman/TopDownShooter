using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Particles
{
    public class DamageText : MonoBehaviour
    {
        private float _speedX;
        private float _speedY;
        private RectTransform _transform;
        private Text _text;

        void Start()
        {
            _speedX = Random.Range(-2f, 2f);
            _speedY = Random.Range(0f, 3f);
            _transform = GetComponent<RectTransform>();
            _text = GetComponent<Text>();
        }

        void Update()
        {
            var alpha = _text.color.a - Time.deltaTime*2;
            if(alpha <= 0)
                Destroy(gameObject);

            _transform.Translate(_speedX * Time.deltaTime, _speedY * Time.deltaTime, 0);
            _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, alpha);
        }
    }
}