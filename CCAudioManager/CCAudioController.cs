
using UnityEngine;

namespace CC.AudioManager
{
    /// <summary>
    /// 音频控制器
    /// </summary>
    public class CCAudioController
    {
        /// <summary>
        /// 播放点
        /// </summary>
        public float Time
        {
            get { return _audioSoure.time; }
            set { _audioSoure.time = value; }
        }

        /// <summary>
        /// 音量
        /// </summary>
        public float Volume
        {
            get { return _audioSoure.volume; }
            set { _audioSoure.volume = value; }
        }

        /// <summary>
        /// 音调
        /// </summary>
        public float Pitch
        {
            get { return _audioSoure.pitch; }
            set { _audioSoure.pitch = value; }
        }

        /// <summary>
        /// 声音源
        /// </summary>
        private readonly AudioSource _audioSoure;

        public CCAudioController(AudioSource audiosoure)
        {
            this._audioSoure = audiosoure;
        }

        /// <summary>
        /// 停止播放
        /// </summary>
        public void CCAudioSourceStop()
        {
            _audioSoure.Stop();
        }

        /// <summary>
        /// 开始播放（如果之前是停止就从头播，是暂停就继续播）
        /// </summary>
        public void CCAudioSourcePlay()
        {
            _audioSoure.Play();
        }

        /// <summary>
        /// 暂停音乐
        /// </summary>
        public void CCAudioSourcePause()
        {
            _audioSoure.Pause();
        }

        /// <summary>
        /// 隐式转换，用来外部直接判断或有没有
        /// </summary>
        /// <param name="ccAudioController"></param>
        public static implicit operator bool(CCAudioController ccAudioController)
        {
            return ccAudioController != null;
        }
    }
}
