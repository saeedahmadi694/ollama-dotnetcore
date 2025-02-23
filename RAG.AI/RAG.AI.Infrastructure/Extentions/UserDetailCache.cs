using BookHouse.Core.Utilities;
using RAG.AI.Infrastructure.Dtos.Users;

namespace RAG.AI.Infrastructure.Extentions;
public static class UserDetailCache
{
    public static ExpirableDictionary<int, UserDetailDto> UserDetailDictionary = new();
    //public static ExpirableDictionary<string, UserDetailDto> UserDetailWithNationalIdCache = new();
}




