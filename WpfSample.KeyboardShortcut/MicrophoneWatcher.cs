using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Runtime.InteropServices;

namespace WpfSample.KeyboardShortcut
{
    public class MicrophoneWatcher
    {
        private ManagementEventWatcher _insertWatcher;
        private ManagementEventWatcher _removeWatcher;

        public void StartListening()
        {
            // 监听设备插入
            var insertQuery = new WqlEventQuery(
                "SELECT * FROM Win32_DeviceChangeEvent WHERE EventType = 2"
            );
            _insertWatcher = new ManagementEventWatcher(insertQuery);
            _insertWatcher.EventArrived += OnDeviceInserted;
            _insertWatcher.Start();

            // 监听设备拔出
            var removeQuery = new WqlEventQuery(
                "SELECT * FROM Win32_DeviceChangeEvent WHERE EventType = 3"
            );
            _removeWatcher = new ManagementEventWatcher(removeQuery);
            _removeWatcher.EventArrived += OnDeviceRemoved;
            _removeWatcher.Start();
        }

        private void OnDeviceInserted(object sender, EventArrivedEventArgs e)
        {
            // 检测是否为麦克风
            if (IsMicrophoneConnected())
                Console.WriteLine("麦克风已插入");
        }

        private void OnDeviceRemoved(object sender, EventArrivedEventArgs e)
        {
            if (!IsMicrophoneConnected())
                Console.WriteLine("麦克风已拔出");
        }

        public void StopListening()
        {
            _insertWatcher?.Stop();
            _removeWatcher?.Stop();
        }


        private static bool IsMicrophoneConnected()
        {
            using var searcher = new ManagementObjectSearcher(
                "SELECT * FROM Win32_PnPEntity WHERE ClassGuid = '{e0cbf06c-cd8b-4647-bb8a-263b43f0f974}'"
            );
            return searcher.Get().Count > 0;
        }

    }

}
