using UnityEngine;
using System.Collections;

namespace GameManager
{
	public class Gun_HitEffects : MonoBehaviour
    {
        private Gun_Master gunMaster;
        private Gun_Shoot gunShoot;
        public GameObject defaultHitEffect;
        public GameObject enemyHitEffect;



        public void SpawnDefaultHitEffect(Vector3 hitPosition, Transform hitTransform)
        {
                if (defaultHitEffect != null)
                {
                    Instantiate(defaultHitEffect, hitPosition, Quaternion.identity);

                }

        }

        public void SpawnEnemyHitEffect(Vector3 hitPosition, Transform hitTransform)
        {
            if (enemyHitEffect != null)
            {
                Instantiate(enemyHitEffect, hitPosition, Quaternion.identity);
            }
        }
	}
}

