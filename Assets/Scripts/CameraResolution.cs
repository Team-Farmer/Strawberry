using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResolution : MonoBehaviour
{
    //9:16 고정해상도 만들기

    void Awake()
    {
        Camera camera = GetComponent<Camera>(); //카메라 컴포넌트 가져옴
        Rect rect = camera.rect; //카메라 Viewport rect 부분 가져옴

        float scaleheight = ((float)Screen.width / Screen.height) / ((float)9 / 16); // 가로, 세로 고정하고 싶은 비율
        float scalewidth = 1f / scaleheight; //구한 값을 1로 나누었을 때

        if (scaleheight < 1) //10:16이면 1보다 크고 8:16이면 1보다 작다.
        {
            //1보다 작으면 8:16 위 아래 공간이 남는다. y의 값을 조정
            rect.height = scaleheight;
            rect.y = (1f - scaleheight) / 2f;
        }
        else
        {
            //반대로 1보다 크면 9:16. x 값 조정
            rect.width = scalewidth;
            rect.x = (1f - scalewidth) / 2f;
        }
        camera.rect = rect; //수정값 대입
    }
}
