using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //Text, Image 등의UI관련 변수 등을 사용할수 있게됩니다

public class Attandance : MonoBehaviour
{
   
    public Image TestImage; //기존에 존제하는 이미지
    public Sprite TestSprite; //바뀌어질 이미지

    public void ChangeImage()
    {
        TestImage.sprite = TestSprite; //TestImage에 SourceImage를 TestSprite에 존제하는 이미지로 바꾸어줍니다
    }
    

}

