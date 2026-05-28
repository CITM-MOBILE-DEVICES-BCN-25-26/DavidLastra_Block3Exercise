using UnityEngine;

namespace CleanRefactor.Presentation
{
    [RequireComponent(typeof(AudioSource))]
    public sealed class UnityPurchaseAudio : MonoBehaviour, IPurchaseAudio
    {
        [SerializeField] private AudioSource audioSource;

        private void Awake()
        {
            if (audioSource == null)
                audioSource = GetComponent<AudioSource>();
        }

        public void PlayPurchaseSound()
        {
            if (audioSource != null)
                audioSource.Play();
        }
    }
}
