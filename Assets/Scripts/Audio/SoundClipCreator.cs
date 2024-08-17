using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundClipCreator : MonoBehaviour
{
    public static SoundClipCreator Instance;
    public float pulseFrequency = 2f;

    void Awake()
    {
        // �V���O���g���p�^�[���̎���
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public AudioClip CreateClip(float initialFrequency, float finalFrequency, float duration, bool includePulse)
    {
        int sampleRate = 44100;
        int sampleCount = (int)(sampleRate * duration);
        AudioClip clip = AudioClip.Create("Clip", sampleCount, 1, sampleRate, false);
        float[] samples = new float[sampleCount];

        for (int i = 0; i < sampleCount; i++)
        {
            float t = (float)i / sampleCount;
            float frequency = Mathf.Lerp(initialFrequency, finalFrequency, t);
            float sample = Mathf.Sign(Mathf.Sin(2 * Mathf.PI * frequency * i / sampleRate));

            if (includePulse)
            {
                // �p���X����������
                float pulse = Mathf.Sin(2 * Mathf.PI * pulseFrequency * i / sampleRate) > 0 ? 1f : -1f;
                sample = sample * 0.5f + pulse * 0.5f; // �T�C���g�ƃp���X���u�����h
            }

            samples[i] = sample;
        }

        clip.SetData(samples, 0);
        return clip;
    }
}
