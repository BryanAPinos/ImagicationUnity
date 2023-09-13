using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoController1 : MonoBehaviour
{
    public static VideoPlayer videoPlayer1;
    public static VideoPlayer videoPlayer2;
    [SerializeField]
    private string nomVideo; //name of your video file with his extension like hello.mp4
    public GameObject screen1;
    public GameObject screen2;
    void Start()
    {
        // videoPlayer = gameObject.GetComponent<VideoPlayer>(); 
        videoPlayer1 = screen1.GetComponent<VideoPlayer>(); 
        videoPlayer2 = screen2.GetComponent<VideoPlayer>(); 
        // videoPlayer1.url = System.IO.Path.Combine (Application.streamingAssetsPath,nomVideo); //turn off when running in editor
        // videoPlayer2.url = System.IO.Path.Combine (Application.streamingAssetsPath,nomVideo); //turn off when running in editor
        // videoPlayer1.url = "StreamingAssets/" + nomVideo + ".mp4";
        videoPlayer1.url = "StreamingAssets/" + nomVideo; //solution
        videoPlayer2.url = "StreamingAssets/" + nomVideo;

        // videoPlayer.SetDirectAudioMute(0,true);
        
    }

    void Update()
    {
    
    }


   
}