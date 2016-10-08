using UnityEngine;
using System.Collections;

namespace GameManager
{
	public class Gun_DynamicCrosshair : MonoBehaviour
    {
        private Gun_Master gunMaster;
        private Gun_Aim gunAim;
        public Transform canvasDynamicCrosshair;
        private Transform playerTransform;
        private Transform weaponCamera;
        
        private Vector3 myTransform;
        private float playerSpeed;
        private float nextCaptureTime;
        private float captureInterval = 0.5f;
        private Vector3 lastPosition;
        public Animator crosshairAnimator;
        public Animator gunAnimator;
        public string weaponCameraName;

		void Start () 
		{
            SetInitialReferences();
		}
	
		void Update () 
		{
            CapturePlayerSpeed();
            ApplySpeedToAnimation();
		}
		
		void SetInitialReferences()
		{
            gunMaster = GetComponent<Gun_Master>();
            gunAim = GetComponentInChildren<Gun_Aim>();
            myTransform = new Vector3(0f, 0f, 0f);
            gunAnimator = GetComponent<Animator>();
            playerTransform = GameManager_References._player.transform;
            FindWeaponCamera(playerTransform);
            SetCameraOnDynamicCrosshairCanvas();
            SetPlaneDistanceOnDynamicCrosshairCanvas();
		}

        void CapturePlayerSpeed()
        {
            if (Time.time > nextCaptureTime)
            {
                nextCaptureTime = Time.time + captureInterval;
                playerSpeed = (playerTransform.position - lastPosition).magnitude / captureInterval;
                lastPosition = playerTransform.position;
                gunMaster.CallEventSpeedCaptured(playerSpeed);
            }
        }

        void ApplySpeedToAnimation()
        {
            if (crosshairAnimator != null)
            {
                crosshairAnimator.SetFloat("Speed", playerSpeed);
            }

            if (gunAnimator != null) 
            {
                gunAnimator.SetFloat("Speed", playerSpeed);
                if (gunAim.isAiming)
                {
                    this.transform.localPosition = myTransform;
                    gunAnimator.enabled = false;
                }
                else
                {
                    gunAnimator.enabled = true;
                }
            }
        }

        void FindWeaponCamera(Transform transformToSearchTrough)
        {
            if (transformToSearchTrough != null)
            {
                if (transformToSearchTrough.name == weaponCameraName)
                {
                    weaponCamera = transformToSearchTrough;
                    return;
                }

                foreach (Transform child in transformToSearchTrough)
                {
                    FindWeaponCamera(child);
                }
            }
        }

        void SetCameraOnDynamicCrosshairCanvas()
        {
            if (canvasDynamicCrosshair != null && weaponCamera != null)
            {
                canvasDynamicCrosshair.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
                canvasDynamicCrosshair.GetComponent<Canvas>().worldCamera = weaponCamera.GetComponent<Camera>();
            }
        }

        void SetPlaneDistanceOnDynamicCrosshairCanvas()
        {
            if (canvasDynamicCrosshair != null)
            {
                canvasDynamicCrosshair.GetComponent<Canvas>().planeDistance = 1;
            }
        }
	}
}

