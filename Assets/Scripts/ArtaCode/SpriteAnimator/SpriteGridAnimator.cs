using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteGridAnimator : MonoBehaviour {

    public Sprite[] sprites;

    public SpriteAnimation[] animations;

    public bool hideIFNull;

    public new SpriteRenderer renderer;

    SpriteAnimation _curAnimation;
    int curFrame = 0;
    float animTimer;


    string curAtlasPath;
    //
    

    void Awake () {
        renderer = GetComponent<SpriteRenderer>();

    }

	
	void Update () {

        if (curAnimation == null){
            if(hideIFNull)
                renderer.sprite = null;
            return;
        }

        if (animTimer <= 0)
        {
            if (!curAnimation.loop)
                curFrame = (isLastFrame) ? curFrame : curFrame + 1;
            else
                curFrame = ++curFrame % _curAnimation.xFrames.Length;
            animTimer = _curAnimation.frameTime;
        }

        int i = _curAnimation.xFrames[curFrame] + _curAnimation.rowLength * _curAnimation.yFrames;
        if(i < sprites.Length)
            renderer.sprite = sprites[i];

        animTimer -= Time.deltaTime;
	}



    public bool isLastFrame
    {
        get { return curFrame + 1 == _curAnimation.xFrames.Length; }
    }

    
    public void SetCurrentAnimation(string name)
    {
        SpriteAnimation anim = GetAnimation(name);
        //if (anim != null)
            curAnimation = anim;
    }

    public SpriteAnimation curAnimation
    {
        get { return _curAnimation; }
        set{
            if (_curAnimation == value){
                if (_curAnimation != null && _curAnimation.loop && isLastFrame) { }
                else return;
            }
            
            _curAnimation = value;

            if (_curAnimation == null) return;
            curFrame = curFrame % _curAnimation.xFrames.Length;
        }
    }


    public SpriteAnimation GetAnimation(string name){
        for(int i=0; i<animations.Length; i++){
            if (animations[i].name == name)
                return animations[i];
        }
        return null;
    }



    public void LoadAtlas(string path)
    {
        curAtlasPath = path;

        sprites = Resources.LoadAll<Sprite>(path);
    }
}


[System.Serializable]
public class SpriteAnimation
{
    public string name;
    public float frameTime;
    public int[] xFrames;
    public int yFrames;
    public int rowLength;
    public bool loop = true;

    public SpriteAnimation() {
        frameTime = 0.1f;
        rowLength = 5;
    }

    public SpriteAnimation(int[] xFrames, int yFrames, int rowLength = 0, float t = 0.1f)
    {
        this.xFrames = xFrames;
        this.yFrames = yFrames;
        this.frameTime = t;
        this.rowLength = (rowLength == 0) ? xFrames.Length : rowLength;
    }
}
