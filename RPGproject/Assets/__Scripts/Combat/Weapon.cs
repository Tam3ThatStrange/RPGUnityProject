using RPG.Core;
using Unity.VisualScripting;
using UnityEngine;

namespace RPG.Combat
{

    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] AnimatorOverrideController animatorOvverride = null;
        [SerializeField] GameObject equippedPrefab = null;
        [SerializeField] float weaponDamage = 5f;
        [SerializeField] float weaponRange = 2f;
        [SerializeField] bool isRightHanded = true;
        [SerializeField] Projectile projectile = null;


        public void Spawn(Transform righthand, Transform leftHand, Animator animator)
        {
            if (equippedPrefab != null)
            {
                Transform handTransform = GetTransform(righthand, leftHand);
                Instantiate(equippedPrefab, handTransform);
            }
            if (animatorOvverride != null)
            {
                animator.runtimeAnimatorController = animatorOvverride;
            }
        }

        private Transform GetTransform(Transform righthand, Transform leftHand)
        {
            Transform handTransform;
            if (isRightHanded) handTransform = righthand;
            else handTransform = leftHand;
            return handTransform;
        }

        public bool HasProjectile()
        {
            return projectile != null;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target)
        {
           Projectile projectileInstance = Instantiate(projectile,
               GetTransform(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target);
        }
        public float GetDamage()
        {
            return weaponDamage;
        }
        public float GatRange()
        {
            return weaponRange;
        }
    }
}
