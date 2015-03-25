using Assets.Scripts.Messages;
using UnityEngine;
using UnityEventAggregator;

namespace Assets.Scripts.Managers
{
    public class ParticleManager : MonoBehaviour, IListener<SpawnBloodMessage>
    {
        public GameObject BloodSplat;

        void Start()
        {
            this.Register<SpawnBloodMessage>();
        }

        void OnDestroy()
        {
            this.UnRegister<SpawnBloodMessage>();
        }

        public void Handle(SpawnBloodMessage message)
        {
            var b = Instantiate(BloodSplat);
            b.transform.position = message.Position;
        }
    }
}