using AscentProtocol.SceneManagement;
using UnityEngine;

public class NextPose : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject button;

    private void Start()
    {
        if (player == null)
        {
            player = SceneObjectRegistry.Instance.Get("Player").gameObject;
        }
    }
    
    public void Next()
    {
        button.SetActive(false);
        player.GetComponentInChildren<PlayerMover>().MovePlayer();
    }
}
