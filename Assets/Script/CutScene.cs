using UnityEngine;
using System.Collections;

public class CutScene : MonoBehaviour
{
    public GameObject botCamera;   // Camera nhìn Boss
    public GameObject playerCamera; // Camera chính của Player
    public float cutsceneDuration = 3f; // Thời gian camera nhìn Boss

    void Start()
    {
        StartCoroutine(PlayCutScene());
    }

    IEnumerator PlayCutScene()
    {
        // Bật camera Boss, tắt camera Player
        botCamera.SetActive(true);
        playerCamera.SetActive(false);

        // Chờ vài giây
        yield return new WaitForSeconds(cutsceneDuration);

        // Trả lại camera Player
        botCamera.SetActive(false);
        playerCamera.SetActive(true);
    }
}
