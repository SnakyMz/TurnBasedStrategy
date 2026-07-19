using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] Vector3 targetPosition;
    [SerializeField] float moveSpeed = 4f;
    [SerializeField] float stoppingDistance = 0.1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }
    }

    void Move(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }
}
