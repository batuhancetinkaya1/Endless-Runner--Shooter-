using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Slider volumeSlider2;

    private bool isUpdating = false; // Sonsuz d�ng�y� engellemek i�in

    private void Start()
    {
        float savedVolume = SoundManager.Instance.GetSavedVolume();
        // Her iki slider'� da kaydedilen ses seviyesine ayarla
        volumeSlider.value = savedVolume;
        volumeSlider2.value = savedVolume;

        // Her iki slider i�in listener'lar� ekle
        volumeSlider.onValueChanged.AddListener(OnFirstSliderChanged);
        volumeSlider2.onValueChanged.AddListener(OnSecondSliderChanged);
    }

    private void OnFirstSliderChanged(float value)
    {
        if (!isUpdating)
        {
            isUpdating = true;
            // �lk slider de�i�ti�inde ikinci slider'� g�ncelle
            volumeSlider2.value = value;
            SoundManager.Instance.SetVolume(value);
            isUpdating = false;
        }
    }

    private void OnSecondSliderChanged(float value)
    {
        if (!isUpdating)
        {
            isUpdating = true;
            // �kinci slider de�i�ti�inde ilk slider'� g�ncelle
            volumeSlider.value = value;
            SoundManager.Instance.SetVolume(value);
            isUpdating = false;
        }
    }
}