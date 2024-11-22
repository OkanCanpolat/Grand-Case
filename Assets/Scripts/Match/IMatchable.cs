public interface IMatchable 
{
    public bool IsMatched { get; set; }
    public void OnMatch();
}
