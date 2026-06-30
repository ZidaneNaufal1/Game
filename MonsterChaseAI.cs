using UnityEngine;

public class MonsterChaseAI : MonoBehaviour
{
    [Header("Target")]
    public Transform player;

    [Header("Movement")]
    public float moveSpeed = 3.5f;
    public float rotationSpeed = 6f;

    [Header("Detection")]
    public float chaseRange = 12f;
    public float stopChaseRange = 18f;
    public float attackRange = 1.5f;

    [Header("State")]
    public bool isChasing = false;

    private bool hasCaughtPlayer = false;

    void Update()
    {
        if (player == null) return;
        if (hasCaughtPlayer) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (!isChasing && distance <= chaseRange)
        {
            isChasing = true;
            Debug.Log("Monster melihat player dan mulai mengejar!");
        }

        if (isChasing && distance >= stopChaseRange)
        {
            isChasing = false;
            Debug.Log("Player berhasil menjauh dari monster.");
        }

        if (isChasing)
        {
            ChasePlayer(distance);
        }
    }

    void ChasePlayer(float distance)
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        if (direction.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            player.position,
            moveSpeed * Time.deltaTime
        );

        if (distance <= attackRange)
        {
            CatchPlayer();
        }
    }

    void CatchPlayer()
    {
        hasCaughtPlayer = true;
        isChasing = false;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameOver();
        }
        else
        {
            Debug.Log("GAME OVER! Player tertangkap monster.");
        }
    }
}