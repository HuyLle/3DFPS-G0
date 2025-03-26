using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MiniMapCamera : MonoBehaviour
{
    public GameObject player;
    private void LateUpdate()
    {
        transform.position = new Vector3(player.transform.position.x, 7, player.transform.position.z);
    }
}
