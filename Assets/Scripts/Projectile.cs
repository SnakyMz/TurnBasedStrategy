using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float moveSpeed = 200f;
    [SerializeField] GameObject bulletHitVfx;

    Vector3 targetPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetDirection = (targetPosition - transform.position).normalized;

        float distanceBeforeMoving = Vector3.Distance(transform.position, targetPosition);

        transform.position += targetDirection * moveSpeed * Time.deltaTime;

        float distanceAfterMoving = Vector3.Distance(transform.position, targetPosition);

        if (distanceBeforeMoving < distanceAfterMoving)
        {
            Instantiate(bulletHitVfx, targetPosition, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    public void Setup(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }
}
