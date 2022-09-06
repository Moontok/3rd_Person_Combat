using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ForceReceiver : MonoBehaviour
{
    [SerializeField] private CharacterController controller = null;
    [SerializeField] private NavMeshAgent agent = null;
    [SerializeField] private float drag = 0.3f;

    private Vector3 dampingVelocity = Vector3.zero;
    private Vector3 impact = Vector3.zero;
    private float verticalVelocity = 0f;

    public Vector3 Movement => impact + Vector3.up * verticalVelocity;

    private void Update()
    {
        if (verticalVelocity < 0f && controller.isGrounded)
        {
            verticalVelocity = Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }

        impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dampingVelocity, drag);

        if (agent != null && impact.sqrMagnitude < 0.2f * 0.2f)
        {
            impact = Vector3.zero;
            agent.enabled = true;
        }
    }

    public void AddForce(Vector3 force)
    {
        impact += force;
        if (agent != null)
            agent.enabled = false;
    }
}
