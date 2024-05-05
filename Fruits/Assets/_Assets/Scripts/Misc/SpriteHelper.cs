using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace _Assets.Scripts.Misc
{
    public static class SpriteHelper
    {
        public static async UniTask<Sprite> CreateSprite(string relativePath, bool compress = true, bool highQuality = false)
        {

            var imageBytes = await File.ReadAllBytesAsync(relativePath);
            var texture = new Texture2D(1, 1);
            texture.LoadImage(imageBytes);

            if (compress)
            {
                texture.Compress(highQuality);
            }

            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 256);
        }


        public static async UniTask<Sprite> CreateSpriteFromStreamingAssests(string relativePath, bool compress = true, bool highQuality = false)
        {

            using (var webRequest = UnityWebRequestTexture.GetTexture(relativePath))
            {

                await webRequest.SendWebRequest();
                var texture = DownloadHandlerTexture.GetContent(webRequest);

                if (compress)
                {
                    texture.Compress(highQuality);
                }

                return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 256);
            }
        }
    }
}