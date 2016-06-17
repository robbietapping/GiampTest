using KaliidoAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KaliidoAPI
{
    public interface IKaliidoServiceFactory
    {
        TokenResponse GlobalAccessTokenInformation { get; set; }
        KaliidoServiceFactory Instance { get; }

        Task<TokenResponse> loginSession(string username, string password);

        // ACCOUNT
        Task<dynamic> SignUp(int distance);
        // USER
        Task<List<KaliidoUser>> getUsersClosestByDistance(int distance);
        Task<dynamic> getUsersClosestByDistance(int distance, double latitude, double longitude);
        Task <dynamic> getUserSearchByInterests(int[] interests);
        Task<dynamic> getUsersByName(string name, int area);
        Task<dynamic> getUserById(int userid);
        Task<dynamic> sendFriendRequest(int userid);
        Task<dynamic> acceptFriendRequest(int userid);
        Task<dynamic> declineFriendRequest(int userid);
        Task<dynamic> getListOfUsersWithFriendRequests();
        Task<dynamic> getListOfFriends();
        Task<dynamic> favorite(int userid);
        Task<dynamic> unFavorite(int userid);
        Task<dynamic> report(int userid);
        Task<dynamic> block(int userid);
        Task<dynamic> updateNote(int userid, string note);
        Task<dynamic> getMyFavoriteUsers();
        Task<dynamic> GetFullInfoByUserIds(int[] ids);
        Task<dynamic> GetShortInfoByUserIds(int[] ids);
        Task<dynamic> GetFullInfoByQuickbloxUserIds(int[] quickBoxIds);
        Task<dynamic> UpdateLocation(double latitude, double longitude, decimal accuracy);

    }
}