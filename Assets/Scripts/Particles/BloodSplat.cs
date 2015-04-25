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
                var spreadX = Random.Range(-1f * extraBloodModifier, 1f * extraBloodModifier);
                var spreadY = Random.Range(-1f * extraBloodModifier, 1f * extraBloodModifier);
                var size = Random.Range(.5f * extraBloodModifier, 2f);
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