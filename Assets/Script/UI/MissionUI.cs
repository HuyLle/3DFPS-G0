using System.Collections;
using UnityEngine;
using TMPro;

public class MissionUI : MonoBehaviour
{
    public TextMeshProUGUI missionText; // UI hiển thị nhiệm vụ
    private int totalEnemies;
    private int enemiesDefeated = 0;

    private string currentMission;
    private int missionStep = 1; // Theo dõi bước nhiệm vụ

    public GameObject objectiveReachPoint1;
    public GameObject objectiveReachPoint2;

    public Color defaultColor = Color.white;
    public Color completedColor = Color.green;
    public float delayBeforeNextMission = 2f; // Thời gian chờ trước khi chuyển nhiệm vụ

    private void Start()
    {
        SetMission(1);
    }

    public void SetMission(int step)
    {
        missionStep = step;
        enemiesDefeated = 0;

        if (missionStep == 1)
        {
            currentMission = "Kill HoverBot";
            totalEnemies = CountEnemiesWithTag("Enemy"); // Đếm chính xác số enemy trong tầng
        }
        else if (missionStep == 2)
        {
            currentMission = "Kill TurretBot";
            totalEnemies = CountEnemiesWithTag("TurretBot");
        }
        missionText.color = defaultColor; // Reset lại màu chữ
        UpdateMissionText();
    }

    public void EnemyDefeated(string enemyTag)
    {
        if((missionStep == 1 && enemyTag == "Enemy") || (missionStep == 2 && enemyTag == "TurretBot"))
        {
            enemiesDefeated++;
            UpdateMissionText();

            if (enemiesDefeated >= totalEnemies)
            {
                StartCoroutine(HandleMissionCompletion());
            }
        }
    }

    private IEnumerator HandleMissionCompletion()
    {
        missionText.color = completedColor; // Đổi màu xanh lá khi hoàn thành
        yield return new WaitForSeconds(delayBeforeNextMission);

        if (missionStep == 1)
        {
            ActivateObjective(objectiveReachPoint1);
            //SetMission(2);
        }
        else if (missionStep == 2)
        {
            ActivateObjective(objectiveReachPoint2);
            missionText.text = "Mission Complete!";
        }

        //missionText.color = defaultColor; // Reset lại màu chữ
    }

    private void ActivateObjective(GameObject objective)
    {
        if (objective != null)
        {
            objective.SetActive(true);
        }
    }

    private void UpdateMissionText()
    {
        missionText.text = $"Mission: {currentMission} {enemiesDefeated}/{totalEnemies}";
    }

    private int CountEnemiesWithTag(string tag)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(tag);
        int count = 0;
        foreach (GameObject enemy in enemies)
        {
            if (enemy.activeInHierarchy) // Chỉ đếm enemy đang hoạt động
                count++;
        }
        return count;
    }
}

