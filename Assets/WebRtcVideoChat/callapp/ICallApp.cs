/* 
 * Copyright (C) 2021 because-why-not.com Limited
 * 
 * Please refer to the license.txt for license information
 */
using Byn.Awrtc;

//interface is used by CallAppUi to allow reusing
//the same UI for multiple different versions of the
//CallApp
public interface ICallApp
{
    bool CanSelectVideoDevice();
    bool GetLoudspeakerStatus();
    string[] GetVideoDevices();
    bool IsMute();
    void Join(string address);
    void ResetCall();
    void Send(string msg);
    void SetAudio(bool value);
    void SetAutoRejoin(bool rejoin, float rejoinTime = 4);
    void SetFormat(FramePixelFormat format);
    void SetIdealFps(int fps);
    void SetIdealResolution(int width, int height);
    void SetLoudspeakerStatus(bool state);
    void SetMute(bool state);
    void SetRemoteVolume(float volume);
    void SetShowLocalVideo(bool showLocalVideo);
    void SetupCall();
    void SetVideo(bool value);
    void SetVideoDevice(string deviceName);
}
