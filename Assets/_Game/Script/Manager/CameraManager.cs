using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    [SerializeField] private Transform target;       // Đối tượng cần follow
    [SerializeField] private float followSpeed = 5f; // Tốc độ mượt khi follow

    private Vector3 startPos = new Vector3(0f, 1f, -7f);
    private Quaternion startRot = Quaternion.Euler(Vector3.zero);

    private void LateUpdate()
    {
        if (target == null) return;

        // Follow chỉ khi target cao hơn
        Vector3 camPos = transform.position;
        if (target.position.y > camPos.y)
        {
            Vector3 targetPos = new Vector3(camPos.x, target.position.y, camPos.z);
            transform.position = Vector3.Lerp(camPos, targetPos, followSpeed * Time.deltaTime);
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void ResetCamera()
    {
        target = null;
        transform.position = startPos;
        transform.rotation = startRot;
    }
}
