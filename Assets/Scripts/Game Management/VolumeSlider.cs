using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Slider volumeSlider2;

    private bool isUpdating = false; // Sonsuz döngüyü engellemek için

    private void Start()
    {
        float savedVolume = SoundManager.Instance.GetSavedVolume();
        // Her iki slider'ý da kaydedilen ses seviyesine ayarla
        volumeSlider.value = savedVolume;
        volumeSlider2.value = savedVolume;

        // Her iki slider için listener'larý ekle
        volumeSlider.onValueChanged.AddListener(OnFirstSliderChanged);
        volumeSlider2.onValueChanged.AddListener(OnSecondSliderChanged);
    }

    private void OnFirstSliderChanged(float value)
    {
        if (!isUpdating)
        {
            isUpdating = true;
            // Ýlk slider deðiþtiðinde ikinci slider'ý güncelle
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
            // Ýkinci slider deðiþtiðinde ilk slider'ý güncelle
            volumeSlider.value = value;
            SoundManager.Instance.SetVolume(value);
            isUpdating = false;
        }
    }
}