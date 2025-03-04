using NAudio.CoreAudioApi;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfSample.KeyboardShortcut
{
    public static class MicrophoneController
    {
        private static readonly MMDeviceEnumerator _deviceEnumerator = new MMDeviceEnumerator();
        private static MMDevice _defaultDevice = _deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Capture, Role.Communications);
        public static void Mute()
        {
            if (_defaultDevice is null)
            {
                _defaultDevice = _deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Capture, Role.Communications);
            }
            _defaultDevice.AudioEndpointVolume.Mute = true;
        }

        public static void Unmute()
        {
            if (_defaultDevice is null)
            {
                _defaultDevice = _deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Capture, Role.Communications);
            }
            _defaultDevice.AudioEndpointVolume.Mute = false;
        }
    }
}
