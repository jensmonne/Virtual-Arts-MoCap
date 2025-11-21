using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    [SerializeField] private Transform player;
    
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    
    private Coroutine moveRoutine;  
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("PlayerStop")) return;
        
        StopMovement();
    }

    public void MovePlayer()
    {
        if (moveRoutine != null) return;

        moveRoutine = StartCoroutine(MoveForward());
    }
    
    public void StopMovement()
    {
        if (moveRoutine == null) return;

        StopCoroutine(moveRoutine);
        moveRoutine = null;
    }
    
    private System.Collections.IEnumerator MoveForward()
    {
        while (true)
        {
            player.position = Vector3.Lerp(
                player.position,
                player.position + player.forward,
                Time.deltaTime * moveSpeed
            );

            yield return null;
        }
    }
}