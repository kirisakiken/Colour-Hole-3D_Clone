using System.Text;
using UnityEngine;

namespace BezmicanZehir.Core
{
    public static class HapticFeedback
    {
        #if UNITY_ANDROID && !UNITY_EDITOR
                public static readonly AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                public static AndroidJavaObject CurrentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                public static AndroidJavaObject vibrator = CurrentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
        #else
                public static readonly AndroidJavaClass UnityPlayer;
                public static AndroidJavaObject CurrentActivity;
                public static AndroidJavaObject vibrator;
        #endif

        public static void Vibrate(long ms)
        {
            if (IsAndroid())
            {
                vibrator.Call("vibrate", ms);
            }
            else
            {
                Handheld.Vibrate();
            }
        }

        public static void Cancel()
        {
            if (IsAndroid())
            {
                vibrator.Call("cancel");
            }
        }

        public static bool IsAndroid()
        {
        #if UNITY_ANDROID
                    return true;
        #else
                    return false;
        #endif
        }
    }
}
