using Revuo.Chat.Abstraction.Client;
using Revuo.Chat.Abstraction.Server;
namespace DalleR;

public static class StorageHelper
{
    public static IUserStorage GetStorage(IThinClientContext context)
    {
        return context.ApplicationStorage;
    }
}
