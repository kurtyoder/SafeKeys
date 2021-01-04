namespace SafeKeys.Library.API
{
    public interface ISettingHandler
    {
        string GetPath();
        void SavePath(string path);
    }
}