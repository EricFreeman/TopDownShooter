using Assets.Scripts.Util;
using UnityEngine;

namespace Assets.Scripts
{
    public class Director : MonoBehaviour
    {
        public GameObject Enemy;
        public float SpawnDelay = 5;
        public float CurrentSpawnDelay;

        void Start()
        {
            CurrentSpawnDelay = SpawnDelay;
        }

        void Update()
        {
            CurrentSpawnDelay -= Time.deltaTime;
            if (CurrentSpawnDelay <= 0)
            {
                CurrentSpawnDelay += SpawnDelay;
                var newEnemy = Instantiate(Enemy);
                newEnemy.transform.Translate(GetOffscreenPosition());
            }
        }

        private Vector3 GetOffscreenPosition()
        {
            var spawnSide = Random.Range(0, 4);
            const float width = 19f;
            const float height = 11f;

            Vector3 offset;

            if (spawnSide == 0) offset = new Vector3(-width, Random.Range(-height, height), 0);
            else if (spawnSide == 1) offset = new Vector3(Random.Range(-width, width), height, 0);
            else if (spawnSide == 2) offset = new Vector3(width, Random.Range(-height, height), 0);
            else offset = new Vector3(Random.Range(-width, width), -height, 0);

            return Camera.main.transform.position.ToXY() + offset;
        }
    }
}