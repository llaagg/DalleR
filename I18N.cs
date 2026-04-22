using Revuo.Chat.Base.I18N;

namespace DalleR;

public static class I18N
{
    public static TranslationSet set = new TranslationSet()
    {
        Translations =
        {
            ["en-US"] = new Translation()
            {
                Entries =
                {
                    ["HELLO_0"] = "Hello from DalleR"
                }
            }
        }
    };
}
