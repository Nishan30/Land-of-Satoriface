using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    internal int m_damage = 0;
    [SerializeField] Rigidbody m_rigidBody;
    private void Awake()
    {
        StartCoroutine(destroyDelay());
        transform.rotation = Quaternion.LookRotation(transform.forward, Player.CurrentPlayer.position);
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && m_rigidBody.velocity.magnitude > 10)
        {
            Player.instance.DealDamage(m_damage);
            Destroy(gameObject);
        }
    }

    IEnumerator destroyDelay()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
