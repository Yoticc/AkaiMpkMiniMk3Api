namespace AkaiMpkMiniMk3Api;
public record PadKey(byte Code, int Column, int Row)
{
    public static List<PadKey> Keys = 
    [
        new(40, 0, 0), new(38, 1, 0),  new(46, 2, 0),  new(44, 3, 0), 
        new(37, 0, 1), new(36, 1, 1),  new(42, 2, 1),  new(54, 3, 1),
        new(49, 0, 2), new(55, 1, 2),  new(51, 2, 2),  new(53, 3, 2),
        new(48, 0, 3), new(47, 1, 3),  new(45, 2, 3),  new(43, 3, 3),
    ];

    public static PadKey FromCode(byte code) => Keys.Find(k => k.Code == code);

    public override string ToString() => $"{Column}:{Row}";
}

public record PianoKey(byte Code, Note Note, byte Octave)
{
    public static readonly List<PianoKey> Keys = Enumerable.Range(0, 120).Select(i => (byte)i).Select(i => new PianoKey(i, Note.Octave[i % Note.Octave.Length], (byte)(i / (byte)Note.Octave.Length))).ToList();

    public static PianoKey FromCode(byte code) => Keys.Find(k => k.Code == code);

    public override string ToString() => $"{Note.Ru}({Octave})";
}

public record Note(string En, string Ru)
{
    public static Note 
        C = new("C", "До"),
        CDiez = new("C♯", "До♯"),
        D = new("D", "Ре"),
        DDiez = new("D♯", "Ре♯"),
        E = new("E", "Ми"),
        F = new("F", "Фа"),
        FDiez = new("F♯", "Фа♯"),
        G = new("G", "Соль"),
        GDiez = new("G♯", "Соль♯"),
        A = new("A", "Ля"),
        ADiez = new("A♯", "Ля♯"),
        B = new("B", "Си");

    public static readonly Note[] Octave = [C, CDiez, D, DDiez, E, F, FDiez, G, GDiez, A, ADiez, B];
}