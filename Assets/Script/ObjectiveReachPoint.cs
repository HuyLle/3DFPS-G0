using UnityEngine;

public class ObjectiveReachPoint : MonoBehaviour
{
    public Animator anim; // Animator của NextFloor
    public bool isFirstStage = false;
    //public SceneLoader loader;
    private AudioSource audioSource;
    public AudioClip winSound;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (anim == null && isFirstStage)
            anim = GetComponentInParent<Animator>(); // Tìm Animator trên NextFloor
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player đã chạm vào ObjectiveReachPoint!");

            if (isFirstStage)
            {
                if(anim != null)
                    anim.SetTrigger("isActive");
            }
            else 
            {
                audioSource.PlayOneShot(winSound);
                if(SceneLoader.Instance != null)
                {
                    SceneLoader.Instance.LoadSceneByIndex(3);
                }
            }
        }
    }
}
