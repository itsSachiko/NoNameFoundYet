using UnityEngine;

public class GameObjectHolder : MonoBehaviour
{
    public GameObject nightMare;
    public GameObject dream;

    public void NightmareChange()
    {
        dream.SetActive(false);
        nightMare.SetActive(true);
    }

    public void DreamOnDayo()
    {
        nightMare.SetActive(false);
        dream.SetActive(true);
    }
}
