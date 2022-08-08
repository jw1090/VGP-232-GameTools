namespace Assignment2b
{
    public interface IPeristence
    {
        bool Load(string filename);
        bool Save(string filename);
    }
}