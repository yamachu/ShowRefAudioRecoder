using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using AudioRecoder.Core.Models;
using ManagedBass;

namespace AudioRecoder.Core.Services
{
    public class RecorderService : BaseNotifyModel, IRecorderService
    {
        private readonly ObservableCollection<DeviceIdPair> _recorderDevices = new ObservableCollection<DeviceIdPair>();
        public ReadOnlyObservableCollection<DeviceIdPair> RecoderDevices { get; }

        private ObservableCollection<byte> _recordingBuffer = new ObservableCollection<byte>();
        public ReadOnlyObservableCollection<byte> RecordingBuffer { get; }

        private float recordingVolume = 1.0f;
        public float RecordingVolume
        {
            get => recordingVolume;
            private set
            {
                if (value < 0f || value > 1f)
                    throw new ArgumentException("Volume range is between 0 to 1");

                if (value.CompareTo(recordingVolume) != 0)
                {
                    recordingVolume = value;
                    NotifyPropertyChanged(nameof(RecordingVolume));
                }
            }
        }

        private bool isRecording = false;
        public bool IsRecording
        {
            get => isRecording;
            private set
            {
                if (value != isRecording)
                {
                    isRecording = value;
                    NotifyPropertyChanged(nameof(IsRecording));
                }
            }
        }

        private int RecorderHandle = -1;

        public RecorderService()
        {
            RecoderDevices = new ReadOnlyObservableCollection<DeviceIdPair>(_recorderDevices);
            RecordingBuffer = new ReadOnlyObservableCollection<byte>(_recordingBuffer);
        }

        public async Task GetRecorderDevices()
        {
            _recorderDevices.Clear();
            var totalDevices = Bass.RecordingDeviceCount;
            Enumerable.Range(0, totalDevices).ToList().ForEach((i) =>
            {
                _recorderDevices.Add(new DeviceIdPair { Id = i, Info = Bass.RecordGetDeviceInfo(i) });
            });
        }

        public async Task StartRecording(int samplingRate, int channel, SampleBit bit)
        {
            var recordId = Bass.RecordStart(samplingRate, channel, GetBitFlagsFromEnum(bit),
                                            (handle, buffer, length, user) =>
            {
                var tmp = new byte[length];
                Marshal.Copy(buffer, tmp, 0, length);
                foreach (var v in tmp) _recordingBuffer.Add(v);
                return true;
            }, IntPtr.Zero);

            if (recordId != 0) 
            {
                RecorderHandle = recordId;

                IsRecording = true;
            }
        }

        public async Task StopRecording()
        {
            Bass.ChannelStop(RecorderHandle);

            IsRecording = false;
        }

        public async Task InitializeRecordingDevice(int deviceId)
        {
            try
            {
                Bass.RecordFree();
            }
            catch (BassException ex)
            {
                // There are no initialized devices.
                // If this function is called for the first time, this exception will be raised.
            }

            try
            {
                Bass.RecordInit(deviceId);
                Bass.CurrentRecordingDevice = deviceId;
            }
            catch (BassException ex)
            {
                // ToDo: Error handling
                throw ex;
            }
        }

        public async Task SetRecordingDeviceVolume(float volume)
        {
            if (volume < 0f || volume > 1f)
                throw new ArgumentException("Parameter must be between 0 to 1");

            try
            {
                Bass.RecordSetInput(0, InputFlags.None, volume);
                RecordingVolume = volume;
            }
            catch(BassException ex) when (ex.ErrorCode == Errors.NotAvailable)
            {
                RecordingVolume = 1f;
            }
            catch(BassException ex)
            {
                // ToDo: Error handling
                throw ex;
            }
        }

        private static BassFlags GetBitFlagsFromEnum(SampleBit bit)
        {
            switch (bit)
            {
                case SampleBit.BIT8:
                    return BassFlags.Byte;
                case SampleBit.BIT16:
                    return BassFlags.Byte | BassFlags.Float;
                case SampleBit.BIT32F:
                    return BassFlags.Float;
                default:
                    throw new ArgumentException();
            }
        }
    }
}
