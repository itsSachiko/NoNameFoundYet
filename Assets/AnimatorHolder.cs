using UnityEngine;

public class AnimatorHolder : MonoBehaviour
{
    public Animator dreamAnim;
    public Animator NightmareAnim;
    [SerializeField] private Animator current;

    private void Start()
    {
        PlayerHp.Nightmare += NightMare;
    }

    public void NightMare()
    {
        current = NightmareAnim;
    }
}
