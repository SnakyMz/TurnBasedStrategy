using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    [SerializeField] Animator animator;
    Rigidbody[] ragdollBodies;
    HealthSystem healthSystem;

    void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        ragdollBodies = GetComponentsInChildren<Rigidbody>();

        // Start with ragdoll disabled
        SetRagdollActive(false);
    }

    void Start()
    {
        healthSystem.OnDeath += Die;
    }

    public void SetRagdollActive(bool active)
    {
        animator.enabled = !active; // disable Animator when ragdoll is active

        foreach (Rigidbody rb in ragdollBodies)
        {
            rb.isKinematic = !active; // physics takes over when active
        }
    }

    // Example trigger: call this when character dies
    public void Die()
    {
        SetRagdollActive(true);
    }

    void OnDestroy()
    {
        healthSystem.OnDeath -= Die;
    }
}
