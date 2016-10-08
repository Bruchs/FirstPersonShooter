using UnityEngine;
using System.Collections;

namespace GameManager
{
	public class Gun_StandarInput : MonoBehaviour
    {
        private Gun_Master gunMaster;
        private Gun_Animation gunAnimation;
        private float nextAttack;
        public float attackRate = 0.5f;
        private Transform myTransform;
        public bool isAutomatic;
        public bool hasBurstFire;
        private bool isBurstFireActive;
        private bool isAimActive;
        public bool isReloading;
        public bool isShooting;
        public string attackButtonName;
        public string reloadButtonName;
        public string burstFireButtonName;
        public string aimButtonName;

		void Start () 
		{
            SetInitialReferences();
		}       
	
		void Update () 
		{
            CheckIfWeaponShouldAttack();
            CheckForReloadRequest();
            CheckForBurstFireToggle();
            CheckForAimToggle();
		}
		
		void SetInitialReferences()
		{
            gunMaster = GetComponent<Gun_Master>();
            gunAnimation = GetComponent<Gun_Animation>();
            myTransform = transform;
            gunMaster.isGunLoaded = true;
		}

        void CheckIfWeaponShouldAttack()
        {
            if (Time.time > nextAttack && Time.timeScale > 0 && myTransform.root.CompareTag("Player"))
            {
                if (isAutomatic && !isBurstFireActive)
                {
                    if (Input.GetButton(attackButtonName))
                    {
                        Debug.Log("Full Auto");
                        AttemptAttack();
                    }
                }
                else if (isAutomatic && isBurstFireActive)
                {
                    if (Input.GetButtonDown(attackButtonName))
                    {
                        Debug.Log("Burst");
                        StartCoroutine(RunBurstFire());
                    }
                }
                else if (!isAutomatic)
                {
                    if (Input.GetButtonDown(attackButtonName))
                    {
                        AttemptAttack();
                    }
                }
            }
        }

        void AttemptAttack()
        {
            nextAttack = Time.time + attackRate;
            if (gunMaster.isGunLoaded)
            {
                gunAnimation.FireWeapon();
                Debug.Log("Shooting");
                gunMaster.CallEventPlayerInput();
                isShooting = true;
            }
            else
            {
                gunMaster.CallEventGunNotUsable();
                isShooting = false;
            }
        }

        void CheckForReloadRequest()
        {
            if (Input.GetButtonDown(reloadButtonName) && Time.timeScale > 0 && myTransform.root.CompareTag("Player"))
            {
                gunMaster.CallEventRequestReload();
            }
        }

        void CheckForBurstFireToggle()
        {
            if (Input.GetButtonDown(burstFireButtonName) && Time.timeScale > 0 && myTransform.root.CompareTag("Player"))
            {
                Debug.Log("Burst Fire Toggle");
                isBurstFireActive = !isBurstFireActive;
                gunMaster.CallEventToggleBurstFire();
            }
        }

        void CheckForAimToggle()
        {
            if (Input.GetButton(aimButtonName) && Time.timeScale > 0 && myTransform.root.CompareTag("Player"))
            {
                isAimActive = !isAimActive;
                gunMaster.CallEventToggleAim();
            }
        }

        IEnumerator RunBurstFire()
        {
            AttemptAttack();
            yield return new WaitForSeconds(attackRate);
            AttemptAttack();
            yield return new WaitForSeconds(attackRate);
            AttemptAttack();
        }


	}
}

