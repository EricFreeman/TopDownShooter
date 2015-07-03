using Assets.Scripts.Messages;
using Assets.Scripts.Particles;
using UnityEngine;
using UnityEngine.UI;
using UnityEventAggregator;

namespace Assets.Scripts.Managers
{
    public class ParticleManager : MonoBehaviour, IListener<SpawnBloodMessage>, IListener<SpawnGibsMessage>, IListener<SpawnDamageTextMessage>, IListener<SpawnExplosionMessage>
    {
        public GameObject BloodSplat;
        public GameObject Gib;
        public GameObject DamageText;
        public GameObject UiCanvas;
        public GameObject ExplosionGameObject;

        void Start()
        {
            this.Register<SpawnBloodMessage>();
            this.Register<SpawnGibsMessage>();
            this.Register<SpawnDamageTextMessage>();
            this.Register<SpawnExplosionMessage>();
        }

        void OnDestroy()
        {
            this.UnRegister<SpawnBloodMessage>();
            this.UnRegister<SpawnGibsMessage>();
            this.UnRegister<SpawnDamageTextMessage>();
            this.UnRegister<SpawnExplosionMessage>();
        }

        public void Handle(SpawnBloodMessage message)
        {
            var b = Instantiate(BloodSplat);
            b.transform.position = message.Position;
            b.GetComponent<BloodSplat>().Spawn(message.SplatterSize, message.Color);
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

        public void Handle(SpawnExplosionMessage message)
        {
            var explosion = Instantiate(ExplosionGameObject);
            explosion.transform.position = message.Position;
        }
    }
}