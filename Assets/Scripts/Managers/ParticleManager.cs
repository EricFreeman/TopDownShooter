using Assets.Scripts.Messages;
using Assets.Scripts.Particles;
using UnityEngine;
using UnityEventAggregator;

namespace Assets.Scripts.Managers
{
    public class ParticleManager : MonoBehaviour, IListener<SpawnBloodMessage>, IListener<SpawnGibsMessage>
    {
        public GameObject BloodSplat;
        public GameObject Gib;

        void Start()
        {
            this.Register<SpawnBloodMessage>();
            this.Register<SpawnGibsMessage>();
        }

        void OnDestroy()
        {
            this.UnRegister<SpawnBloodMessage>();
            this.UnRegister<SpawnGibsMessage>();
        }

        public void Handle(SpawnBloodMessage message)
        {
            var b = Instantiate(BloodSplat);
            b.transform.position = message.Position;
            b.GetComponent<BloodSplat>().Spawn(message.SplatterSize);
        }

        public void Handle(SpawnGibsMessage message)
        {
            for (var i = 0; i < message.Count; i++)
            {
                var b = Instantiate(Gib);
                b.transform.position = message.Position;
            }
        }
    }
}