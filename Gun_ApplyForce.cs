﻿using UnityEngine;
using System.Collections;

namespace GameManager
{
	public class Gun_ApplyForce : MonoBehaviour
    {
        private Gun_Master gunMaster;
        private Transform myTransform;
        public float forceToApply = 300;

		void OnEnable()
		{
            SetInitialReferences();
            gunMaster.EventShotDefault += ApplyForce;
		}
		
		void OnDisable()
		{
            gunMaster.EventShotDefault -= ApplyForce;
		}
		
		void SetInitialReferences()
		{
            gunMaster = GetComponent<Gun_Master>();
            myTransform = transform;
		}

        void ApplyForce(Vector3 hitPosition, Transform hitTransform)
        {
            if (hitTransform.GetComponent<Rigidbody>() != null)
            {
                hitTransform.GetComponent<Rigidbody>().AddForce(myTransform.forward * forceToApply, ForceMode.Impulse);
            }
        }
	}
}

