using Assets.Scripts.Util;
using UnityEngine;

namespace Assets.Scripts.Particles
{
    public class BloodSplat : MonoBehaviour
    {
        public GameObject BloodGameObject;

        void Start()
        {
            var amount = Random.Range(1, 5);

            for (var i = 0; i < amount; i++)
            {
                var spreadX = Random.Range(-1f, 1f);
                var spreadY = Random.Range(-1f, 1f);
                var size = Random.Range(.5f, 2f);
                var opacity = Random.Range(.25f, 1f);

                var b = Instantiate(BloodGameObject);
                b.transform.position = transform.position + new Vector3(spreadX, spreadY, 0);
                b.transform.localScale = new Vector3(size, size, size);
                b.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, opacity);
                b.GetComponent<SpriteRenderer>().sortingOrder = Constants.BloodLayer;
            }
        }
    }
}