using UnityEngine;
using System.Collections;

namespace GameManager
{
	public class Gun_Ammo : MonoBehaviour
    {
        private Player_Master playerMaster;
        private Gun_Master gunMaster;
        private Player_AmmoBox playerAmmoBox;
        private Gun_Sounds gunSounds;
        private Gun_Animation gunAnimation;
        private Gun_Aim gunAim;

        public int clipSize;
        public int currentAmmo;
        public string ammoName;
        public float reloadTime;

		void OnEnable()
		{
            SetInitialReferences();
            StartingSanityCheck();
            CheckAmmoStatus();

            gunMaster.EventPlayerInput += DeductAmmo;
            gunMaster.EventPlayerInput += CheckAmmoStatus;
            gunMaster.EventRequestReload += TryToReload;
            gunMaster.EventGunNotUsable += TryToReload;
            gunMaster.EventRequestGunReset += ResetGunReloading;

            if (playerMaster != null)
            {
                playerMaster.EventAmmoChanged += UiAmmoUpdateRequest;
            }

            if (playerAmmoBox != null)
            {
                StartCoroutine(UpdateAmmoUiWhenEnabling());
            }
        }
		
		void OnDisable()
		{
            gunMaster.EventPlayerInput -= DeductAmmo;
            gunMaster.EventPlayerInput -= CheckAmmoStatus;
            gunMaster.EventRequestReload -= TryToReload;
            gunMaster.EventGunNotUsable -= TryToReload;
            gunMaster.EventRequestGunReset -= ResetGunReloading;

            if (playerMaster != null)
            {
                playerMaster.EventAmmoChanged -= UiAmmoUpdateRequest;
            }
        }

		void Start () 
		{
            SetInitialReferences();
            StartCoroutine(UpdateAmmoUiWhenEnabling());

            if (playerMaster != null)
            {
                playerMaster.EventAmmoChanged += UiAmmoUpdateRequest;
            }
        }
	
		
		void SetInitialReferences()
		{
            gunMaster = GetComponentInParent<Gun_Master>();
            gunAnimation = GetComponentInParent<Gun_Animation>();
            gunSounds = GetComponentInParent<Gun_Sounds>();
            gunAim = GetComponent<Gun_Aim>();

            if (GameManager_References._player != null)
            {
                playerMaster = GameManager_References._player.GetComponent<Player_Master>();
                playerAmmoBox = GameManager_References._player.GetComponent<Player_AmmoBox>();
            }
		}

        void DeductAmmo()
        {
            currentAmmo--;
            UiAmmoUpdateRequest();
        }

        void TryToReload()
        {
            for (int i = 0; i < playerAmmoBox.typesOfAmmunition.Count; i++)
            {
                if (playerAmmoBox.typesOfAmmunition[i].ammoName == ammoName)
                {
                    if (playerAmmoBox.typesOfAmmunition[i].ammoCurrentCarried > 0 && currentAmmo != clipSize && !gunMaster.isReloading && !gunAim.isAiming)
                    {
                        gunMaster.isReloading = true;
                        gunMaster.isGunLoaded = false;

                        if (gunAnimation != null)
                        {
                            gunAnimation.ReloadWeapon();
                            StartCoroutine(ReloadWeaponTime());
                            
                        }
                        else
                        {
                            StartCoroutine(ReloadWithoutAnimation());
                        }
                    }
                    break;
                }
            }
        }

        void CheckAmmoStatus()
        {
            if (currentAmmo <= 0)
            {
                currentAmmo = 0;
                gunMaster.isGunLoaded = false;
            }
            else
            {
                gunMaster.isGunLoaded = true;
            }
        }

        void StartingSanityCheck()
        {
            if (currentAmmo > clipSize)
            {
                currentAmmo = clipSize;
            }
        }

        void UiAmmoUpdateRequest()
        {
            for (int i = 0; i < playerAmmoBox.typesOfAmmunition.Count; i++)
            {
                if (playerAmmoBox.typesOfAmmunition[i].ammoName == ammoName)
                {
                    gunMaster.CallEventAmmoChanged(currentAmmo, playerAmmoBox.typesOfAmmunition[i].ammoCurrentCarried);
                    break;
                }
            }
        }

        void ResetGunReloading()
        {
            gunMaster.isReloading = false;
            CheckAmmoStatus();
            UiAmmoUpdateRequest();
        }

        public void OnReloadComplete()
        {
            // Attemp to add ammo to current ammo
            for (int i = 0; i < playerAmmoBox.typesOfAmmunition.Count; i++)
            {
                if (playerAmmoBox.typesOfAmmunition[i].ammoName == ammoName)
                {
                    int ammoTopUp = clipSize - currentAmmo;

                    if (playerAmmoBox.typesOfAmmunition[i].ammoCurrentCarried >= ammoTopUp)
                    {
                        currentAmmo += ammoTopUp;
                        playerAmmoBox.typesOfAmmunition[i].ammoCurrentCarried -= ammoTopUp;
                    }

                    else if (playerAmmoBox.typesOfAmmunition[i].ammoCurrentCarried < ammoTopUp && playerAmmoBox.typesOfAmmunition[i].ammoCurrentCarried != 0)
                    {
                        currentAmmo += playerAmmoBox.typesOfAmmunition[i].ammoCurrentCarried;
                        playerAmmoBox.typesOfAmmunition[i].ammoCurrentCarried = 0;
                    }

                    break;
                }
            }

            ResetGunReloading();
        }

        IEnumerator ReloadWithoutAnimation()
        {
            yield return new WaitForSeconds(reloadTime);
            OnReloadComplete();
        }

        IEnumerator UpdateAmmoUiWhenEnabling()
        {
            yield return new WaitForSeconds(0.05f);
            UiAmmoUpdateRequest();
        }

        IEnumerator ReloadWeaponTime()
        {
            gunSounds.PlayReloadSound();
            yield return new WaitForSeconds(reloadTime);
            OnReloadComplete();
        }
	}
}

