using UnityEngine;

namespace Utility
{
    public class ParticlePlayer : MonoBehaviour
    {
        private ParticleSystem[] _allParticles;

        // Start is called before the first frame update
        private void Start()
        {
            _allParticles = GetComponentsInChildren<ParticleSystem>();
        }

        public void PlayParticles()
        {
            foreach (ParticleSystem particle in _allParticles)
            {
                particle.Stop();
                particle.Play();
            }
        }
    }
}