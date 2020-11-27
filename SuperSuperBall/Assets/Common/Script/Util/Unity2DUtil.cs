using UnityEngine;

namespace ssb
{
    // Unity全般で使える２Ｄ関係の計算ユーティリティクラス
    public class Unity2DUtil
    {
        // 2点間の角度を計算
        public static float CalcAngle2D(Vector2 start, Vector2 target)
        {
		    Vector2 dt      = target - start;
            float rad       = Mathf.Atan2(dt.y, dt.x);
            float degree    = rad * Mathf.Rad2Deg;
		
		    return degree;
	    }

        public static Vector3 genRandomVector2(float width)
        {
            return new Vector3(Random.Range(-width, width), Random.Range(-width, width));
        }
    }
}
