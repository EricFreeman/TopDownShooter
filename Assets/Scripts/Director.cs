using Assets.Scripts.Util;
using Dungeon.Generator;
using UnityEngine;

namespace Assets.Scripts
{
    public class Director : MonoBehaviour
    {
        public GameObject Enemy;
        public float SpawnDelay = 5;
        public float CurrentSpawnDelay;

        public GameObject Tile;
        public GameObject Wall;
        public float TileSize = 8;

        void Start()
        {
            CurrentSpawnDelay = SpawnDelay;
            CreateLevel();
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

        private void CreateLevel()
        {
            var map = Generator.Generate(MapSize.Small, 1);

            for (var y = 0; y < map.Height; y++)
            {
                for (var x = 0; x < map.Width; x++)
                {
                    if (map[x, y].MaterialType == MaterialType.Floor)
                    {
                        var tile = Instantiate(Tile);
                        tile.transform.position = new Vector3(x, y) * TileSize;
                    }
                    else if (map[x, y].MaterialType == MaterialType.Wall)
                    {
                        var wall = Instantiate(Wall);
                        wall.transform.position = new Vector3(x, y) * TileSize;
                    }
                }
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