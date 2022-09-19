namespace WeaponLib
{
    public interface IPeristence
    {
        bool Load(string filename);
        bool Save(string filename);
    }
}