using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AICar : MonoBehaviour
{
    [SerializeField] NavMeshAgent m_navMeshAgent;
    [SerializeField] Transform front, back, right, left, target;
    void Update()
    {
        if (target != null)
        {
            m_navMeshAgent.SetDestination(target.position);
        }
    }
}
