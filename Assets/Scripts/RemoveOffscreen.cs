using UnityEngine;

namespace Assets.Scripts
{
    public class RemoveOffscreen : MonoBehaviour
    {
        void Update()
        {
            // 1 sec check is a hack to fix it removing all npcs on load
            if (!GetComponent<Renderer>().isVisible && Time.fixedTime > 1)
                DestroyImmediate(gameObject);
        }
    }
}