using UnityEngine;

public class OnAnimEnd : MonoBehaviour
{
    public void OnAnimationEnd()
    {
        PlayerComponent.onAnimEnd?.Invoke();
    }
}
