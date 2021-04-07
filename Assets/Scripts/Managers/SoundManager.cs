using UnityEngine;

namespace Managers
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField]
        private bool musicEnabled = true, fxEnabled = true;

        [SerializeField]
        [Range(0, 1)]
        private float musicVolume = 1.0f, fxVolume = 1.0f;

        [SerializeField] private AudioClip clearRowSound, moveSound, wrongMoveSound, dropSound, uiClickSound, gameOverSound;

        [SerializeField] private AudioSource musicSource, fxSource;

        [SerializeField] private AudioClip[] musicClips;

        [SerializeField] private AudioClip[] levelUpClips;

        [SerializeField] private AudioClip holdShapeClip;

        // Start is called before the first frame update
        private void Start()
        {
            PlayBackgroundMusic(GetRandomClip(musicClips));
        }

        public void ToggleMusic()
        {
            musicEnabled = !musicEnabled;
            UpdateMusicState();
        }

        public void ToggleFX() => fxEnabled = !fxEnabled;

        public void PlayUIClickSound(float volumeModifier = 1.0f)
        {
            if (fxEnabled && uiClickSound)
                fxSource.PlayOneShot(uiClickSound, fxVolume * volumeModifier);
        }

        public void PlayHoldShapeSound(float volumeModifier = 1.0f)
        {
            if (fxEnabled && holdShapeClip)
                fxSource.PlayOneShot(holdShapeClip, fxVolume * volumeModifier);
        }

        public void PlayLevelUpSound(float volumeModifier = 1.0f)
        {
            if (fxEnabled && levelUpClips.Length > 0)
                fxSource.PlayOneShot(GetRandomClip(levelUpClips), fxVolume * volumeModifier);
        }

        public void PlayMoveSound(float volumeModifier = 1.0f)
        {
            if (fxEnabled && moveSound)
                fxSource.PlayOneShot(moveSound, fxVolume * volumeModifier);
        }

        public void PlayWrongMoveSound(float volumeModifier = 1.0f)
        {
            if (fxEnabled && wrongMoveSound)
                fxSource.PlayOneShot(wrongMoveSound, fxVolume * volumeModifier);
        }

        public void PlayDropSound(float volumeModifier = 1.0f)
        {
            if (fxEnabled && dropSound)
                fxSource.PlayOneShot(dropSound, fxVolume * volumeModifier);
        }

        public void PlayClearRowSound(float volumeModifier = 1.0f)
        {
            if (fxEnabled && clearRowSound)
                fxSource.PlayOneShot(clearRowSound, fxVolume * volumeModifier);
        }

        public void PlayGameOverSound(float volumeModifier = 1.0f)
        {
            if (fxEnabled && gameOverSound)
                fxSource.PlayOneShot(gameOverSound, fxVolume * volumeModifier);
        }

        private void UpdateMusicState()
        {
            if (musicSource.isPlaying != musicEnabled)
            {
                if (musicEnabled)
                    PlayBackgroundMusic(GetRandomClip(musicClips));
                else
                    musicSource.Stop();
            }
        }

        private void PlayBackgroundMusic(AudioClip musicClip)
        {
            if (!musicEnabled || !musicClip || !musicSource)
                return;

            musicSource.Stop();

            musicSource.clip = musicClip;

            musicSource.volume = musicVolume;

            musicSource.loop = true;

            musicSource.Play();
        }

        private static AudioClip GetRandomClip(AudioClip[] clips)
        {
            return clips[Random.Range(0, clips.Length)];
        }
    }
}