using NAudio.Midi;
using System.Collections;

namespace AkaiMpkMiniMk3Api;

public abstract record Input()
{
    public int RawMessage;
    public MidiEvent Event;

    public static Input Parse(int message)
    {
        var bits = new BitArray((int[])[message]).Cast<bool>().ToArray();
        var n0 = (byte)((message & 0xFF) &~(1<<7));
        var n1 = (byte)((message >> 8) & 0xFF);
        var n2 = (byte)((message >> 16) & 0xFF);
        var n3 = (byte)((message >> 24) & 0xFF);

        if (n0 == 0b0000000)
            return new PianoKeyUp(PianoKey.FromCode(n1));
        if (n0 == 0b0010000)
            return new PianoKeyDown(PianoKey.FromCode(n1));
        if (n0 == 0b0011001)
            return new PadKeyDown(PadKey.FromCode(n1));
        if (n0 == 0b0001001)
            return new PadKeyUp(PadKey.FromCode(n1));

        return new UnknownInput();
    }

    public static string SerializeRawMessage(int rawMessage)
    {
        var bits = new BitArray((int[])[rawMessage]).Cast<bool?>().ToArray();
        bits[7] = null;

        var message = string.Join('.', bits.Select(b => b is null ? ' ' : (b.Value ? '1' : '0')).Chunk(8).Select(a => new string(a)));
        return message;
    }
}

public record UnknownInput : Input { public override string ToString() => $"UnknownInput[{SerializeRawMessage(RawMessage)}]"; }
public record PianoKeyDown(PianoKey Key) : Input { public override string ToString() => $"PianoKeyDown[{Key}]"; }
public record PianoKeyUp(PianoKey Key) : Input { public override string ToString() => $"PianoKeyUp[{Key}]"; }
public record PadKeyDown(PadKey Key) : Input { public override string ToString() => $"PadKeyDown[{Key}]"; }
public record PadKeyUp(PadKey Key) : Input { public override string ToString() => $"PadKeyUp[{Key}]"; }