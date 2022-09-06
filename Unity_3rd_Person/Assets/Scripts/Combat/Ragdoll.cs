using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    [SerializeField] private Animator animator = null;
    [SerializeField] private CharacterController controller = null;

    private Collider[] allColliders = null;
    public Rigidbody[] allRigidbodies = null;

    private void Start()
    {
        allColliders = GetComponentsInChildren<Collider>(true);
        allRigidbodies = GetComponentsInChildren<Rigidbody>(true);

        ToggleRagdoll(false);
    }

    public void ToggleRagdoll(bool isRagdoll)
    {
        foreach (Collider collider in allColliders)
        {
            if(collider.gameObject.CompareTag("Ragdoll"))
                collider.enabled = isRagdoll;
        }

        foreach (Rigidbody rigidbody in allRigidbodies)
        {
            if (rigidbody.gameObject.CompareTag("Ragdoll"))
            {
                rigidbody.isKinematic = !isRagdoll;
                rigidbody.useGravity = isRagdoll;
            }
        }

        controller.enabled = !isRagdoll;
        animator.enabled = !isRagdoll;
    }
}
