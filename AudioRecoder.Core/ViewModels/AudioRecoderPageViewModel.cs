using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;
using AudioRecoder.Core.Services;
using Prism.Commands;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace AudioRecoder.Core.ViewModels
{
    public class AudioRecoderPageViewModel : ViewModelBase
    {
        public ICommand RecordStartCommand { get; private set; }
        public ReactiveCommand PlayRecordedSoundCommand { get; private set; }
        public ReactiveCommand PlayReferenceSoundCommand { get; private set; }
        public ICommand NavigateNextCommand { get; private set; }
        public ICommand NavigatePreviousCommand { get; private set; }

        public ReactiveProperty<string> RecordStartButtonText { get; }
        public ReactiveProperty<string> PlayRecordedSoundButtonText { get; } = new ReactiveProperty<string>("Play Rec");
        public ReactiveProperty<string> PlayReferenceSoundButtonText { get; } = new ReactiveProperty<string>("Play Ref");

        public ReactiveProperty<int> SelectedSamplingRate { get; } = new ReactiveProperty<int>(16000);
        public ReactiveProperty<int> SelectedChannel { get; } = new ReactiveProperty<int>(1);
        public ReactiveProperty<int> SelectedBitDepth { get; } = new ReactiveProperty<int>(16);

        public ReadOnlyReactiveCollection<DeviceIdPair> Devices { get; }
        public ReactiveProperty<int> SelectedDeviceIndex { get; } = new ReactiveProperty<int>(0);

        public List<int> SamplingRates { get; } = new List<int> { 16000, 44100, 48000 };
        public List<int> Channels { get; } = new List<int> { 1, 2 };
        public List<int> BitDepths { get; } = new List<int> { 8, 16, 32 };

        public AudioRecoderPageViewModel(IRecorderService recorderService)
        {
            Devices = recorderService.RecoderDevices.ToReadOnlyReactiveCollection();

            RecordStartButtonText = recorderService.ObserveProperty(x => x.IsRecording)
                                                   .Select(b => b ? "Stop" : "Record")
                                                   .ToReactiveProperty("Record")
                                                   .AddTo(Disposable);

            RecordStartCommand = new DelegateCommand(() =>
            {
                if (recorderService.IsRecording)
                {
                    recorderService.StopRecording();
                }
                else
                {
                    recorderService.InitializeRecordingDevice(Devices[SelectedDeviceIndex.Value].Id);
                    recorderService.StartRecording(SelectedSamplingRate.Value,
                                                   SelectedChannel.Value,
                                                   (SampleBit)SelectedBitDepth.Value);
                }
            });

            recorderService.GetRecorderDevices();
        }
    }
}
