using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCam : MonoBehaviour
{
    private GameObject player;   //プレイヤー情報格納用
    private Vector3 offset;      //相対距離取得用

    // Use this for initialization
    void Start()
    {

        player = GameObject.FindGameObjectWithTag("CubePlayer");

        // MainCamera(自分自身)とplayerとの相対距離を求める
        offset = transform.position - player.transform.position;

    }

    // Update is called once per frame
    void Update()
    {

        /* transform.position = player.transform.position + offset; */
        transform.position = Vector3.Lerp(transform.position, player.transform.position + offset, 3f * Time.deltaTime);


    }
}
