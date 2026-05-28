using UnityEngine;

namespace CleanRefactor.Presentation
{
    /// <summary>
    /// Unity implementation of IPurchaseAudio backed by an AudioSource.
    ///
    /// Audio is kept entirely in the presentation layer. The presenter depends
    /// on the IPurchaseAudio abstraction, so in unit tests a silent fake is
    /// injected instead of this component.
    /// </summary>
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
