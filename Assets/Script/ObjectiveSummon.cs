using UnityEngine;
using System.Collections;

public class ObjectiveSummon : MonoBehaviour
{
    public GameObject botPrefab;  // Prefab bot sẽ spawn
    public Transform spawnPoint;  // Vị trí spawn bot
    public float summonDuration = 2.5f; // Thời gian tồn tại vòng sáng

    void Start()
    {
   
        StartCoroutine(SummonRoutine());
    }

    IEnumerator SummonRoutine()
    {
        yield return new WaitForSeconds(1.5f);
        // Triệu hồi bot tại vị trí spawn
        if (botPrefab != null && spawnPoint != null)
        {
            Instantiate(botPrefab, spawnPoint.position, spawnPoint.rotation);
        }

        // Chờ vòng sáng tồn tại trong khoảng thời gian
        yield return new WaitForSeconds(summonDuration);

        // Xóa vòng sáng sau khi triệu hồi
        Destroy(gameObject);
    }
}
