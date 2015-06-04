using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimationMessenger : MonoBehaviour {

        // Use this for initialization
        void Start () {
            var animator = GetComponent<Animator>();
            animator.SetBool("grounded", true);
        }
	
        // Update is called once per frame
        void Update () {
	
        }
    }
}
