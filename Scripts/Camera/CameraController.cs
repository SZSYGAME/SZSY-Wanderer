using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    [Header("绑定玩家")]
    public GameObject player;
    private Transform playerTransform;
    private Transform camTransform;

    private Vector3 offset = new Vector3(0, 5, -10);  //偏移量

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = player.transform;
        camTransform = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        FollowPlayer();
    }

    /// <summary>
    /// 跟随
    /// </summary>
    void FollowPlayer()
    {
        // 相机位置 = 玩家位置 + 偏移量
        camTransform.position = playerTransform.position + offset;

        camTransform.LookAt(player.transform);
    }
}
