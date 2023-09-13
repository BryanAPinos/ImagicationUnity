/* 
 * Copyright (C) 2021 because-why-not.com Limited
 * 
 * Please refer to the license.txt for license information
 */
using System;
using UnityEngine;

namespace Byn.Unity.Examples
{
    /// <summary>
    /// See IVideoInput for documentation. 
    /// Use IVideoInput whenever possible. VideoInput might get split up into several platform specific classes.
    /// </summary>
    public class VideoInput : IVideoInput
    {
        public TextureFormat Format
        {
            get
            {
#if UNITY_WEBGL && !UNITY_EDITOR
                return TextureFormat.RGBA32;
#else
                return TextureFormat.ARGB32;
#endif
            }
        }
#if UNITY_WEBGL && !UNITY_EDITOR
#else
        private Awrtc.Native.NativeVideoInput mInternal;
#endif
        public VideoInput()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
#else
            mInternal = Awrtc.Unity.UnityCallFactory.Instance.VideoInput;
#endif
        }

        public void AddDevice(string name, int width, int height, int fps)
        {

#if UNITY_WEBGL && !UNITY_EDITOR
            Byn.Awrtc.Browser.CAPI.Unity_VideoInput_AddDevice(name, width, height, fps);
#else
            mInternal.AddDevice(name, width, height, fps);
#endif
        }

        public void RemoveDevice(string name)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            Byn.Awrtc.Browser.CAPI.Unity_VideoInput_RemoveDevice(name);
#else
            mInternal.RemoveDevice(name);
#endif
        }

        public bool UpdateFrame(string name, Texture2D texture, int rotation, bool firstRowIsBottom)
        {
            if (texture.format != Format)
                throw new FormatException("Only " + Format + " supported so far. Use NativeVideoInput.UpdateFrame directly for anything else. ");
            var dataPtr = texture.GetRawTextureData();

#if UNITY_WEBGL && !UNITY_EDITOR
            return Byn.Awrtc.Browser.CAPI.Unity_VideoInput_UpdateFrame(name, dataPtr, 0, dataPtr.Length, texture.width, texture.height, rotation, firstRowIsBottom);
#else

            return mInternal.UpdateFrame(name, dataPtr, texture.width, texture.height, WebRtcCSharp.ImageFormat.kBGRA, rotation, firstRowIsBottom);
#endif
        }
        public bool UpdateFrame(string name, byte[] dataPtr, int width, int height, int rotation, bool firstRowIsBottom)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return Byn.Awrtc.Browser.CAPI.Unity_VideoInput_UpdateFrame(name, dataPtr, 0, dataPtr.Length, width, height, rotation, firstRowIsBottom);
#else

            return mInternal.UpdateFrame(name, dataPtr, width, height, WebRtcCSharp.ImageFormat.kBGRA, rotation, firstRowIsBottom);
#endif
        }
        public bool UpdateFrame(string name, IntPtr dataPtr, int length, int width, int height, int rotation, bool firstRowIsBottom)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return Byn.Awrtc.Browser.CAPI.Unity_VideoInput_UpdateFrame(name, dataPtr, 0, length, width, height, rotation, firstRowIsBottom);
#else

            return mInternal.UpdateFrame(name, dataPtr, length, width, height, WebRtcCSharp.ImageFormat.kBGRA, rotation, firstRowIsBottom);
#endif
        }

    }
}
