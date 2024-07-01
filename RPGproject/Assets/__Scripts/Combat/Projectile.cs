using RPG.Core;
using UnityEngine;

public class Projectile : MonoBehaviour
{
   
    [SerializeField] float speed = 1f;

    Health target = null;
    // Update is called once per frame
    void Update()
    {
        if (target == null) return;

        transform.LookAt(target.transform.position);
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        
    }
    public void SetTarget(Health target)
    {
        this.target = target;
    }
}
