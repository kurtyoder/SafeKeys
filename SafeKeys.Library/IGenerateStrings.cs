using SafeKeys.Library.Models;

namespace SafeKeys.Library
{
    public interface IGenerateStrings
    {
        string Generate(StringGenerationModel gen);
    }
}