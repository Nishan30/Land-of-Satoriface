using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AI_Navigation : MonoBehaviour
{
    [SerializeField] Mesh GizmoMesh;
    [SerializeField] float maxViewAngle, maxViewDistance, AttackRange;
    [SerializeField] NavMeshAgent m_NavMeshAgent;
    [SerializeField] Animator m_Animator;

    bool PlayerInRange;



    private void Update()
    {
        Vector3 TargetDirection = Player.CurrentPlayer.position - transform.position;
        float angle = Vector3.Angle(transform.forward, TargetDirection);
        PlayerInRange = (angle <= maxViewAngle && Vector3.Distance(transform.position, Player.CurrentPlayer.position) < maxViewDistance);
        MovementCheck();
    }

    void MovementCheck()
    {
        if (PlayerInRange)
            m_NavMeshAgent.SetDestination(Player.CurrentPlayer.position);
        m_NavMeshAgent.isStopped = (Vector3.Distance(transform.position, Player.CurrentPlayer.position) < AttackRange);
        if (m_NavMeshAgent.isStopped == false && PlayerInRange == false)
        {
            m_NavMeshAgent.SetDestination(transform.position);
        }

        if (PlayerInRange && m_NavMeshAgent.isStopped)
        {
            shoot();
        }
        // else
        // {
        //     Run(PlayerInRange);
        // }

        if (PlayerInRange)
        {
            if (m_NavMeshAgent.isStopped)
            {
                Run(false);
            }
            else
            {
                Run(true);
            }
        }
        else
        {
            Run(false);
        }
    }
    void shoot()
    {
        transform.LookAt(Player.CurrentPlayer,Vector3.up);
        if (Player.IsAlive)
        {
            m_Animator.SetTrigger("Shoot");
        }
    }
    void Run(bool t)
    {
        m_Animator.SetBool("Run", t);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)(Quaternion.Euler(0, transform.rotation.eulerAngles.y + maxViewAngle, 0) * Vector3.forward * maxViewDistance));
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)(Quaternion.Euler(0, transform.rotation.eulerAngles.y - maxViewAngle, 0) * Vector3.forward * maxViewDistance));
        Gizmos.DrawMesh(GizmoMesh, transform.position, Quaternion.identity, Vector3.one * maxViewDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawMesh(GizmoMesh, transform.position, Quaternion.identity, Vector3.one * AttackRange);
    }
}