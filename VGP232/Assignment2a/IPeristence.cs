namespace Assignment2a
{
    public interface IPeristence
    {
        bool Load(string filename);
        bool Save(string filename);
    }
}
