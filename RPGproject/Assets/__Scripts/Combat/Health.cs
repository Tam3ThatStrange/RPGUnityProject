
using UnityEngine;

namespace RPG.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float healthPoints = 100f;

        private bool bIsDead = false;
        public void TakeDamage(float damage)
        {
          

            healthPoints = Mathf.Max(healthPoints - damage, 0);
          
            if (healthPoints == 0)
            {
                Die();
            }
        }

        private void Die()
        {
            if (bIsDead) return;
            

            bIsDead = true;
            GetComponent<Animator>().SetTrigger("Die");
        }
    }
}
