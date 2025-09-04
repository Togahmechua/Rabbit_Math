using UnityEngine;

public class Level : MonoBehaviour
{
    public Rabbit rabbit;

    private void Start()
    {
        // Khi Level spawn xong thì set camera follow rabbit
        if (rabbit != null)
        {
            CameraManager.Ins.ResetCamera();
            CameraManager.Ins.SetTarget(rabbit.transform);
            UIManager.Ins.mainCanvas.ResetUI();
        }
    }
}
