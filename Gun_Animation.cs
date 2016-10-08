using UnityEngine;
using System.Collections;

namespace GameManager
{
	public class Gun_Animation : MonoBehaviour
    {
        public string drawAnimation;
        public string fireAnimation;
        public string reloadAnimation;

        public GameObject animatedGun;

        private bool drawWeapon = false;
        //private bool reloadWeapon = false;


		void Start () 
		{
            DrawWeapon();
		}

        IEnumerator DrawAnimation()
        {
            yield return new WaitForSeconds(0.6f);
        }

        public void FireWeapon()
        {
             animatedGun.GetComponent<Animation>().CrossFadeQueued(fireAnimation, 0.08f, QueueMode.PlayNow);
        }

        public void ReloadWeapon()
        {
            animatedGun.GetComponent<Animation>().Play(reloadAnimation);
        }

        void DrawWeapon()
        {
            if (drawWeapon)
            {
                animatedGun.GetComponent<Animation>().Play(drawAnimation);
                drawWeapon = true;
                StartCoroutine(DrawAnimation());
                drawWeapon = false;
            } 
        }

    }
}

