using System;
using System.Collections.Generic;
using Assets.Scripts.Util;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Particles
{
    public class Gib : MonoBehaviour
    {
        private float _speedX;
        private float _speedY;
        private float _speedRotation;

        public float SpeedMax = 10;
        public float RotationMax = 15;

        public List<Sprite> ImageList; 

        void Start()
        {
            _speedX = Random.Range(-SpeedMax, SpeedMax);
            _speedY = Random.Range(-SpeedMax, SpeedMax);
            _speedRotation = Random.Range(-RotationMax, RotationMax);

            var spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = ImageList.Random();
        }

        void Update()
        {
            _speedX = UpdateSpeed(_speedX);
            _speedY = UpdateSpeed(_speedY);
            _speedRotation = UpdateSpeed(_speedRotation);

            transform.Translate(new Vector3(_speedX, _speedY) * Time.deltaTime);
            transform.Rotate(0, 0, _speedRotation);
        }

        private float UpdateSpeed(float speed)
        {
            speed *= .9f;
            if (Math.Abs(speed) < .1f) speed = 0;

            return speed;
        }
    }
}