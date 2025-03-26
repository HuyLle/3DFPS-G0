using UnityEngine;

public class NextStage : MonoBehaviour
{
    public GameObject nextFloor;
    public GameObject turretBot;

    private MissionUI missionUI;
    private void Start()
    {
        turretBot.SetActive(false);
        missionUI = FindObjectOfType<MissionUI>();
    }
    void InActive()
    {
        nextFloor.SetActive(false);
    }

    void CreateEnemy()
    {
        turretBot.SetActive(true);

        if (missionUI != null)
        {
            missionUI.SetMission(2);
        }
    }
}
