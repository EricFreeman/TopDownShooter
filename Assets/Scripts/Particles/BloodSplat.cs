using Assets.Scripts.Messages;
using Assets.Scripts.Util;
using UnityEngine;

namespace Assets.Scripts.Particles
{
    public class BloodSplat : MonoBehaviour
    {
        public GameObject BloodGameObject;

        public void Spawn(SplatterSize splatterSize)
        {
            var extraBloodModifier = (int)splatterSize + 1;
            var amount = Random.Range(7 * extraBloodModifier, 15 * extraBloodModifier);

            for (var i = 0; i < amount; i++)
            {
                var spreadX = Random.Range(-.5f * extraBloodModifier, .5f * extraBloodModifier);
                var spreadZ = Random.Range(-.5f * extraBloodModifier, .5f * extraBloodModifier);
                var spead = new Vector3(spreadX, 0, spreadZ) + transform.position;
                var size = Random.Range(.5f * extraBloodModifier, 2f);
                var opacity = Random.Range(.25f, 1f);

                var b = Instantiate(BloodGameObject);
                b.GetComponent<BloodPool>().Setup(spead);
                b.transform.position = transform.position;
                b.transform.localScale = new Vector3(size, size, size);
                b.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, opacity);
                b.GetComponent<SpriteRenderer>().sortingOrder = Constants.BloodLayer;
            }
        }
    }
}