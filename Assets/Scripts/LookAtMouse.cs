using UnityEngine;

namespace Assets.Scripts
{
    public class LookAtMouse : MonoBehaviour
    {
        void Update()
        {
            var mousePos = Input.mousePosition;
            var objectPos = Camera.main.WorldToScreenPoint(transform.position);
            mousePos.x = mousePos.x - objectPos.x;
            mousePos.y = mousePos.y - objectPos.y;
            var angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
            transform.localEulerAngles = new Vector3(-angle + 180, 0, 0);
        }
    }
}