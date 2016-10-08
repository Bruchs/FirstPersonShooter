using UnityEngine;
using System.Collections;

namespace GameManager
{
	public class Gun_Sounds : MonoBehaviour {

        private Gun_Master gunMaster;
        public float shootVolume = 0.4f;
        public float reloadVolume = 0.5f;
        public AudioClip[] shootSounds;
        public AudioClip reloadSound;
        public AudioSource myAudioSource;

		void OnEnable()
		{
            SetInitialReferences();
            gunMaster.EventPlayerInput += PlayShootSound;
		}
		
		void OnDisable()
		{
            gunMaster.EventPlayerInput -= PlayShootSound;
        }
		
		void SetInitialReferences()
		{
            gunMaster = transform.GetComponentInParent<Gun_Master>();
		}

        void PlayShootSound()
        {
            if (shootSounds.Length > 0)
            {
                int index = Random.Range(0, shootSounds.Length);
                myAudioSource.PlayOneShot(shootSounds[index]);
            }
        }

        public void PlayReloadSound()
        {
            if (reloadSound != null)
            {
                myAudioSource.PlayOneShot(reloadSound);
            }
        }
	}
}

