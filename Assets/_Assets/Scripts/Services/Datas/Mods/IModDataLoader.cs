using Cysharp.Threading.Tasks;

namespace _Assets.Scripts.Services.Datas.Mods
{
    public interface IModDataLoader
    {
        UniTask Load();
        void Save();
        ModData ModData { get; }
        void SetModName(string modName);
    }
}