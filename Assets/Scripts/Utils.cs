
using System;
using UnityEngine;

public class Utils {
    public static void horizontalSmoothlyMove(float speed, float target, Transform tr) {
        float distance = target - tr.position.x;

        if (distance != 0) {
            float step = Math.Clamp(speed, 0f, Math.Abs(distance)); // 0f - нижний предел,  Math.Abs(distance) - верхний предел

            if (distance < 0) {
                step = -step;
            }

            tr.position = new Vector3(tr.position.x + step * Time.deltaTime, tr.position.y, tr.position.z);
        }
    }
}
