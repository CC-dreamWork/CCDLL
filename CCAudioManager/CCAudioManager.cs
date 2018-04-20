
using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CC.AudioManager
{
    /// <summary>
    /// 音频管理器
    /// </summary>
    public static class CCAudioManager
    {
        /// <summary>
        /// 播放顶点音乐
        /// </summary>
        /// <param name="audiopos">播放位置</param>
        /// <param name="url">地址</param>
        /// <param name="rangeMax">最大范围</param>
        /// <param name="volume">音量</param>
        /// <param name="pitchmin">最小音调</param>
        /// <param name="pitchmax">最大音调</param>
        /// <param name="loop">循环</param>
        /// <returns></returns>
        public static CCAudioController PlayPosition(Vector3 audiopos, string url, float rangeMax = 10,
            float volume = 1, float pitchmin = 0.95f, float pitchmax = 1.05f,bool loop=false)
        {
            if (string.IsNullOrEmpty(url)) throw new Exception("错误：您输入的音频路径是空的");
            AudioClip audioClip = Resources.Load<AudioClip>(url);
            if (!audioClip) throw new Exception("错误:找不到路径 " + url + " 请检查路径是否存在!");
            return PlayPosition(audiopos, audioClip, rangeMax, volume, pitchmin, pitchmax);
        }

        /// <summary>
        /// 播放顶点音乐
        /// </summary>
        /// <param name="audiopos">播放位置</param>
        /// <param name="audioClip">音频剪辑</param>
        /// <param name="rangeMax">最大范围</param>
        /// <param name="volume">音量</param>
        /// <param name="pitchmin">最小音调</param>
        /// <param name="pitchmax">最大音调</param>
        /// <param name="loop">循环</param>
        /// <returns></returns>
        public static CCAudioController PlayPosition(Vector3 audiopos, AudioClip audioClip, float rangeMax = 10,
            float volume = 1, float pitchmin = 0.95f, float pitchmax = 1.05f, bool loop = false)
        {
            GameObject audiosourcepos = new GameObject("定点3D音源") {hideFlags = HideFlags.HideInHierarchy};
            audiosourcepos.transform.position = audiopos;
            AudioSource audioSource = Get3DAudioSource(audiosourcepos, rangeMax, volume, loop);
            audioSource.clip = audioClip;
            audioSource.pitch = UnityEngine.Random.Range(pitchmin,pitchmax);
            audioSource.Play();
            Object.Destroy(audiosourcepos, audioClip.length / audioSource.pitch);
            return new CCAudioController(audioSource);
        }
        /// <summary>
        /// 获取3d音效
        /// </summary>
        /// <param name="audiogo">声音挂载物体</param>
        /// <param name="rangeMax">最大范围</param>
        /// <param name="volume">音量</param>
        /// <param name="loop">循环</param>
        /// <returns></returns>
        public static AudioSource Get3DAudioSource(GameObject audiogo, float rangeMax, float volume, bool loop)
        {
            AudioSource audioSource = audiogo.GetComponent<AudioSource>();
            if (audioSource) return audioSource;
            audioSource = audiogo.AddComponent<AudioSource>();
            audioSource.volume = volume;
            audioSource.loop = loop;
            audioSource.rolloffMode = AudioRolloffMode.Linear;
            audioSource.minDistance = 0;
            audioSource.maxDistance = rangeMax;
            audioSource.dopplerLevel = 0;
            audioSource.spatialBlend = 1;
            audioSource.spatialize = true;
            return audioSource;
        }

        /// <summary>
        /// 播放3d音乐
        /// </summary>
        /// <param name="audiogo">播放音频挂载体</param>
        /// <param name="url">地址</param>
        /// <param name="rangeMax">最大范围</param>
        /// <param name="volume">音量</param>
        /// <param name="pitchmin">最小音调</param>
        /// <param name="pitchmax">最大音调</param>
        /// <param name="loop">循环</param>
        /// <returns></returns>
        public static CCAudioController Play3DAudio(GameObject audiogo, string url, float rangeMax = 10,
            float volume = 1, float pitchmin = 0.95f, float pitchmax = 1.05f, bool loop = false)
        {
            if (string.IsNullOrEmpty(url)) throw new Exception("错误：您输入的音频路径是空的");
            AudioClip audioClip = Resources.Load<AudioClip>(url);
            if (!audioClip) throw new Exception("错误:找不到路径 " + url + " 请检查路径是否存在!");
            return Play3DAudio(audiogo, audioClip, rangeMax, volume, pitchmin, pitchmax);
        }
        /// <summary>
        /// 播放3d音乐
        /// </summary>
        /// <param name="audiogo">播放音频挂载体</param>
        /// <param name="audioClip">音频剪辑</param>
        /// <param name="rangeMax">最大范围</param>
        /// <param name="volume">音量</param>
        /// <param name="pitchmin">最小音调</param>
        /// <param name="pitchmax">最大音调</param>
        /// <param name="loop">循环</param>
        /// <returns></returns>
        public static CCAudioController Play3DAudio(GameObject audiogo, AudioClip audioClip, float rangeMax = 10,
            float volume = 1, float pitchmin = 0.95f, float pitchmax = 1.05f, bool loop = false)
        {
            AudioSource audioSource = Get3DAudioSource(audiogo, rangeMax, volume, loop);
            audioSource.clip = audioClip;
            audioSource.pitch = UnityEngine.Random.Range(pitchmin, pitchmax);
            if(loop) audioSource.Play();
            else audioSource.PlayOneShot(audioClip);
            return new CCAudioController(audioSource);
        }
    }
}
