namespace Mnemonic.Strings.General;

public sealed record StringReplacePair
{
    public string Pattern { get; set; }
    public string Replacement { get; set; }
}
