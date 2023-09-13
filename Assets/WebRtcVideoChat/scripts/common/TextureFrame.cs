/* 
 * Copyright (C) 2021 because-why-not.com Limited
 * 
 * Please refer to the license.txt for license information
 */
using Byn.Awrtc;
using UnityEngine;
public interface ITextureFrame : IFrame
{
    Texture2D Texture
    {
        get;
    }
}

public class TextureFrame : ITextureFrame
{
    public byte[] Buffer
    {
        get
        {
            return null;
        }
    }
    private Texture2D tex;
    public Texture2D Texture
    {
        get
        {
            return tex;
        }
    }

    public bool Buffered
    {
        get
        {
            return false;
        }
    }

    public int Height
    {
        get
        {
            return this.tex.height;
        }
    }


    public int Width
    {
        get
        {
            return this.tex.width;
        }
    }


    public int Rotation
    {
        get
        {
            return 0;
        }
    }


    public bool IsTopRowFirst
    {
        get
        {
            return true;
        }
    }


    public FramePixelFormat Format
    {
        get
        {
            return FramePixelFormat.Native;
        }
    }


    public TextureFrame(Texture2D tex)
    {
        this.tex = tex;
    }

    public void Dispose()
    {

    }
}
