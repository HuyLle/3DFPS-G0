using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    public Animator transition;
    public float transitionTime = 1f;
    private bool isLoading = false; // Chặn spam chuyển cảnh

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;  // Giải phóng con trỏ chuột
        Cursor.visible = true;                   // Hiển thị chuột
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Chuyển scene theo index
    public void LoadSceneByIndex(int sceneIndex)
    {
        if (!isLoading && sceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            StartCoroutine(LoadScene(sceneIndex));
        }
    }

    // Chuyển scene theo tên
    public void LoadSceneByName(string sceneName)
    {
        if (!isLoading)
        {
            StartCoroutine(LoadScene(sceneName));
        }
    }
    public void LoadMainScene()
    {
        if (!isLoading)
        {
            StartCoroutine(LoadScene("MainScene"));
        }
    }
    public void LoadMenuScene()
    {
        if (!isLoading)
        {
            StartCoroutine(LoadScene("IntroMenu"));
        }
    }
    IEnumerator LoadScene(int sceneIndex)
    {
        isLoading = true;
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneIndex);
        isLoading = false;
    }

    IEnumerator LoadScene(string sceneName)
    {
        isLoading = true;
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneName);
        isLoading = false;
    }
}

