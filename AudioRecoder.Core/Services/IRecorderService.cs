using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;

namespace AudioRecoder.Core.Services
{
    public interface IRecorderService : INotifyPropertyChanged
    {
        ReadOnlyObservableCollection<DeviceIdPair> RecoderDevices { get; }
        ReadOnlyObservableCollection<byte> RecordingBuffer { get; }

        bool IsRecording { get; }
        float RecordingVolume { get; }

        Task GetRecorderDevices();
        Task StartRecording(int samplingRate, int channel, SampleBit bit);
        Task StopRecording();
        Task InitializeRecordingDevice(int deviceId);
        Task SetRecordingDeviceVolume(float volume);
    }

    public enum SampleBit
    {
        BIT8 = 1,
        BIT16 = 16,
        BIT32F = 32,
    }

    public class DeviceIdPair
    {
        public int Id { get; set; }
        public object Info { get; set; }
    }
}
