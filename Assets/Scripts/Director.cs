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
            }
        }
    }
}