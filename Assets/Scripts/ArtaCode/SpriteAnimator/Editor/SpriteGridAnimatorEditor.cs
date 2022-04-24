using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(SpriteGridAnimator))]
public class SpriteGridAnimatorEditor : Editor {

    public Sprite sprite;

    SpriteGridAnimator myTarget;

    SpriteAnimation _curAnimation;

    float animTimer;
    int curFrame;

    public override void OnInspectorGUI()
    {


        base.OnInspectorGUI();

        myTarget = (SpriteGridAnimator)target;

        string s = (myTarget.curAnimation != null) ? myTarget.curAnimation.name : "NULL" ;
        EditorGUILayout.LabelField("Current animation is: " + s);

        this.serializedObject.Update();

        //EditorGUILayout.PropertyField(this.serializedObject.FindProperty("onExit"), true);

        

        if (sprite != null){


            Texture2D texture = AssetPreview.GetAssetPreview(sprite);
            
            GUILayout.Box(texture, GUILayout.Width(200), GUILayout.Height(200));

            for(int i=0; i<myTarget.animations.Length; i++)
            {
                if (GUILayout.Button(myTarget.animations[i].name))
                {
                    myTarget.curAnimation = myTarget.animations[i];
                }
            }
        }

        this.serializedObject.ApplyModifiedProperties();

        //Animate();
    }





    void Animate()
    {
        if (myTarget != null && myTarget.curAnimation != null)
        {

            if (animTimer <= 0)
            {
                curFrame = ++curFrame % myTarget.curAnimation.xFrames.Length;
                animTimer = myTarget.curAnimation.frameTime;
            }

            sprite = myTarget.sprites[myTarget.curAnimation.xFrames[curFrame] + _curAnimation.rowLength * _curAnimation.yFrames];

            animTimer -= Time.deltaTime;
        }
    }
}
