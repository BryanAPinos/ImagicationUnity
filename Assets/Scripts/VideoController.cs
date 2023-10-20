using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    public static VideoPlayer videoPlayer1;
    public static VideoPlayer videoPlayer2;
    [SerializeField]
    private string nomVideo; //name of your video file with extension like hello.mp4
    public GameObject screen1;
    public GameObject screen2;
    void Start()
    {
        videoPlayer1 = screen1.GetComponent<VideoPlayer>();
        videoPlayer2 = screen2.GetComponent<VideoPlayer>();
        videoPlayer1.url = "StreamingAssets/" + nomVideo;
        videoPlayer2.url = "StreamingAssets/" + nomVideo;
    }

    void Update()
    {

    }



}
