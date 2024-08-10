using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public AudioSource bgmSource; // ��� ���� ����� �ҽ�
    public Slider bgmSlider; // ��� ���� �����̴�

    void Start()
    {
        // �����̴� �ʱⰪ ���� (���� ������ ���� ���� ������ �ҷ���)
        bgmSlider.value = PlayerPrefs.GetFloat("BGMVolume", 1f);

        // �ʱ� ���� ����
        bgmSource.volume = bgmSlider.value;

        // �����̴� ���� �� ȣ��� �޼��� ���
        bgmSlider.onValueChanged.AddListener(SetBGMVolume);
    }

    // ��� ���� ���� ����
    public void SetBGMVolume(float volume)
    {
        bgmSource.volume = volume;
        PlayerPrefs.SetFloat("BGMVolume", volume); // ���� �� ����
    }

}
