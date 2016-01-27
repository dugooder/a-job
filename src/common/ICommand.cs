namespace common
{
    public interface ICommand
    {
        string Name { get; set; }
        int Result { get; }
        bool Successful { get; }
        void Execute();
    }
}