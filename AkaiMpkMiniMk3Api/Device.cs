using System.IO.Ports;
using System.Management;
using NAudio.Midi;

namespace AkaiMpkMiniMk3Api;
public class Device : IDisposable
{
    public Device()
    {
        for (int device = 0; device < MidiIn.NumberOfDevices; device++)
        {
            var deviceInfo = MidiIn.DeviceInfo(device);
            if (deviceInfo.ProductName != "MPK mini 3")
                continue;

            midiIn = new MidiIn(device);
            break;
        }

        if (midiIn is null)
            return;

        InitDevice();
    }

    public readonly MidiIn midiIn;

    public delegate void InputDelegate(Input input);
    public InputDelegate? OnInput;

    void InitDevice()
    {
        midiIn.MessageReceived += (s, a) =>
        {
            var input = Input.Parse(a.RawMessage);
            input.Event = a.MidiEvent;
            input.RawMessage = a.RawMessage;

            if (input is UnknownInput)
            {
                var e = a.MidiEvent;
                var msg = $"[{a.Timestamp}]\t{a.RawMessage}\t{Input.SerializeRawMessage(a.RawMessage)}: {{ delta: {e.DeltaTime}, absolute: {e.AbsoluteTime}, channel: {e.Channel}, code: {e.CommandCode} }}";
                Console.WriteLine(msg);
            }
            else OnInput?.Invoke(input);
        };

        midiIn.Start();
    }

    public void Dispose()
    {
        if (midiIn is null)
            return;

        midiIn.Stop();
        midiIn.Dispose();
    }
}