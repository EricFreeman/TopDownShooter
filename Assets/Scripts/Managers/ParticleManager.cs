using Assets.Scripts.Messages;
using Assets.Scripts.Particles;
using UnityEngine;
using UnityEngine.UI;
using UnityEventAggregator;

namespace Assets.Scripts.Managers
{
    public class ParticleManager : MonoBehaviour, IListener<SpawnBloodMessage>, IListener<SpawnGibsMessage>, IListener<SpawnDamageTextMessage>
    {
        public GameObject BloodSplat;
        public GameObject Gib;
        public GameObject DamageText;
        public GameObject UiCanvas;

        void Start()
        {
            this.Register<SpawnBloodMessage>();
            this.Register<SpawnGibsMessage>();
            this.Register<SpawnDamageTextMessage>();
        }

        void OnDestroy()
        {
            this.UnRegister<SpawnBloodMessage>();
            this.UnRegister<SpawnGibsMessage>();
            this.UnRegister<SpawnDamageTextMessage>();
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

        public void Handle(SpawnDamageTextMessage message)
        {
            var b = Instantiate(DamageText);
            b.GetComponent<Text>().text = message.Text;
            b.transform.SetParent(UiCanvas.transform, false);
            b.transform.position = message.Position;
        }
    }
}