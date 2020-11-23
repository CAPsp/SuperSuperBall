using UnityEngine;

namespace ssb
{
    // Unity全般で使える２Ｄ関係の計算ユーティリティクラス
    public class Unity2DUtil
    {
        
        public static float GetAngle2D(Vector2 start, Vector2 target)
        {
		    Vector2 dt      = target - start;
            float rad       = Mathf.Atan2(dt.y, dt.x);
            float degree    = rad * Mathf.Rad2Deg;
		
		    return degree;
	    }

    }
}
