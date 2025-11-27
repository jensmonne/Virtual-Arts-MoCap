using System.Collections;
using AscentProtocol.SceneManagement;
using UnityEngine;

public class LevelFlowController : MonoBehaviour
{
    public FlowState state = FlowState.WaitingForPlayerPose;
    
    [SerializeField] private SceneLoader sceneLoader;
    [SerializeField] private PlayerMover playerMover;
    
    //private int poseIndex = 0;
    //private int sceneIndex = 2;
    private bool poseCompleted = false;

    private void Start()
    {
        sceneLoader.Load("Level1");
        
        if (playerMover == null)
        {
            playerMover = FindAnyObjectByType<PlayerMover>();
        }
        
        PointManager.Instance.OnPoseCompleted += _ =>
        {
            poseCompleted = true;
            Debug.Log("Pose completed");
        };
        
        StartCoroutine(RunFlow());
    }

    private IEnumerator RunFlow()
    {
        while (true)
        {
            switch (state)
            {
                case FlowState.WaitingForPlayerPose:
                    poseCompleted = false;
                    yield return new WaitUntil(() => poseCompleted);
                    state = FlowState.PoseCompleted;
                    break;
                case FlowState.PoseCompleted:
                    state = FlowState.PlayingAnimations;
                    break;
                case FlowState.PlayingAnimations:
                    state = FlowState.LoadingNextScene;
                    break;
                case FlowState.LoadingNextScene:
                    /*bool sceneLoaded = false;
                    sceneLoader.Load($"Level{sceneIndex}", () =>
                    {
                        sceneLoaded = true;
                        sceneIndex++;
                    });*/
                    
                    //yield return new WaitUntil(() => sceneLoaded);
                    state = FlowState.MovingPlayer;
                    break;
                case FlowState.MovingPlayer:
                    playerMover.MovePlayer();
                    state = FlowState.WaitingForPlayerPose;
                    break;
            }

            yield return null;
        }
    }
}