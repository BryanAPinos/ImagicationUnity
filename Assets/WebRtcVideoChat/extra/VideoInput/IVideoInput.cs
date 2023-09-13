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
    /// This interface provides a cross-platform API (WebGL+native) to forward custom image data to WebRTC. 
    /// 
    /// Make sure UnityCallFactory is initialized before using this.
    /// TODO: This should be moved into UnityCallFactory once stable.
    /// </summary>
    public interface IVideoInput
    {
        /// <summary>
        /// Returns a Unity texture format which is supported on the current platform.
        /// 
        /// Native platforms:
        /// Unity's ARGB32 corresponds to native WebRTC's BGRA which is the only image / texture format that both Unity & WebRTC support.
        /// Use NativeVideoInput directly for other formats.
        /// 
        /// WebGL: 
        /// Unity's RGBA32 is the only format that is supported in the browser's HTMLCanvasElement. 
        /// 
        /// </summary>
        TextureFormat Format { get; }

        /// <summary>
        /// Adds a new device. It will be treated by other parts of the asset as a camera device that
        /// has only one supported format using width, height and FPS you set via this method. 
        /// 
        /// Note that actual frame updates do not need to be done in the exact FPS but it is recommended to do so.
        /// </summary>
        /// <param name="name">name of the device. Can be used via MediaConfig.VideoDeviceName and will be returned by UnityCallFactory.GetVideoDevices()</param>
        /// <param name="width">Width of the images you want to deliver</param>
        /// <param name="height">Height of the images you want to deliver</param>
        /// <param name="fps">expected framerate you want to deliver</param>
        void AddDevice(string name, int width, int height, int fps);

        /// <summary>
        /// Removes the device from the internal device list.
        /// </summary>
        /// <param name="name"></param>
        void RemoveDevice(string name);


        /// <summary>
        /// Updates the frame of the video device using the given Texture2D. Note that using Textures might not be
        /// the fastest method.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="texture"></param>
        /// <param name="rotation"></param>
        /// <param name="firstRowIsBottom"></param>
        /// <returns></returns>
        bool UpdateFrame(string name, byte[] dataPtr, int width, int height, int rotation, bool firstRowIsBottom);


        /// <summary>
        /// Updates frames via a byte[].  
        /// Width & Height should be the size you used in AddDevice. It is possible the lower layers will scale your image
        /// to fit the values from AddDevice though.
        /// </summary>
        /// <param name="name">Device name. Must be added before this call via AddDevice</param>
        /// <param name="dataPtr">raw data of the image. Must be in the expected Format</param>
        /// <param name="width">width of the image you supply</param>
        /// <param name="height">height of the image you supply</param>
        /// <param name="rotation">Attaches a rotation value to the image. (might not work on all platforms)</param>
        /// <param name="firstRowIsBottom">Unity often has the frames up-side-down in memory. Set this to true to flip it</param>
        /// <returns>
        /// Returns true if the image was updated. 
        /// False if the device wasn't updated (e.g. invalid username, other problems).Consider logging a warning if this happens
        /// </returns>
        bool UpdateFrame(string name, IntPtr dataPtr, int length, int width, int height, int rotation, bool firstRowIsBottom);


        /// <summary>
        /// Updates frames via a IntPtr. Less safe but allows interaction with other native plugins. 
        /// 
        /// See UpdateFrame for other docs
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dataPtr"></param>
        /// <param name="length"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="rotation"></param>
        /// <param name="firstRowIsBottom"></param>
        /// <returns></returns>
        bool UpdateFrame(string name, Texture2D texture, int rotation, bool firstRowIsBottom);
    }
}