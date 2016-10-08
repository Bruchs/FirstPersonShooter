using UnityEngine;
using System.Collections;

namespace GameManager
{
	public class Gun_Aim : MonoBehaviour 
    {
        public Vector3 aimDownSightPosition;
        public Vector3 hipFirePosition;
        public Camera mainCamera;
        private Gun_Master gunMaster;
        public bool isAiming;

        void Start()
        {
            SetInitialReferences();
        }

		void Update () 
		{
            AimWeapon();
		}

        void SetInitialReferences()
        {
            gunMaster = GetComponentInParent<Gun_Master>();
        }

        public void AimWeapon()
        {
            if (!gunMaster.isReloading && Time.timeScale > 0 && transform.gameObject.layer == 8)
            {
                if (Input.GetMouseButton(1))
                {
                    isAiming = true;
                    mainCamera.fieldOfView = 60;
                    transform.localPosition = Vector3.Slerp(transform.localPosition, aimDownSightPosition, 10 * Time.deltaTime);
                }
                else
                {
                    isAiming = false;
                    mainCamera.fieldOfView = 80;
                    transform.localPosition = Vector3.Slerp(transform.localPosition, hipFirePosition, 10 * Time.deltaTime);
                }
            }
        }
    }
}

