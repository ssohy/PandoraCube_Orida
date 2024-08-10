using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public AudioSource bgmSource; // 배경 음악 오디오 소스
    public Slider bgmSlider; // 배경 음악 슬라이더

    void Start()
    {
        // 슬라이더 초기값 설정 (이전 세션의 볼륨 값이 있으면 불러옴)
        bgmSlider.value = PlayerPrefs.GetFloat("BGMVolume", 1f);

        // 초기 볼륨 설정
        bgmSource.volume = bgmSlider.value;

        // 슬라이더 변경 시 호출될 메서드 등록
        bgmSlider.onValueChanged.AddListener(SetBGMVolume);
    }

    // 배경 음악 볼륨 조절
    public void SetBGMVolume(float volume)
    {
        bgmSource.volume = volume;
        PlayerPrefs.SetFloat("BGMVolume", volume); // 볼륨 값 저장
    }

}
