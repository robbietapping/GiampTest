using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Web;
using System.Net;
using System.IO;
using RestSharp;
using System.Collections.Specialized;
using KaliidoAPI.Models;


namespace KaliidoAPI
{
    public class KaliidoServiceFactory : IKaliidoServiceFactory
    {

        public const string BASE_AUTH_URL = @"http://kaliido-authapi.azurewebsites.net/";
        public const string BASE_RESOURCES_URL = @"http://kaliido-resource.azurewebsites.net/";

        private static KaliidoServiceFactory _instance;


        private TokenResponse _globalAccessTokenInformation = null;


        public  TokenResponse GlobalAccessTokenInformation
        {
            get {
                return _globalAccessTokenInformation;

            }
            set
            {
                _globalAccessTokenInformation = value;
            }

        }


        public KaliidoServiceFactory Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new KaliidoServiceFactory();
                }
                return _instance;
            }
        }


        public async Task<TokenResponse> loginSession(string username, string password)
        {

            //  System.Net.WebRequest wr = System.Net.WebRequest.Create(String.Format(@"{0}token", BASE_AUTH_URL));
            // EXAMPLE OF RESPONSE YOU SHOULD RECIEVE FROM THE SERVER
            //{
            //    "access_token": "I0YjTZSCt0TaBBjZy4K1ICPqufEYTDAW-FhW9mpWqkd2QdHtilIUqGE1rZMAWUeEvfC8SYRPfaetda3dVAeLvL8U59wxECW9YuW5JyT2u_7XJFXPiSAWiKi-3NRgFQHtvEE5ZUtMMN0GPcKd8tql6nXR71XYhA-navU6WTrSkWOd9QUOWurY3_XPxVQ5zgIB3aGP7bCEbZX9LpHQzwA8vVL7b6MpKu-m9L3N5cwJhgxLdhqvh0Hypt8ugeYX_BMIPLm7qCNFTWc-OvI9pKeo53d1Y67pQckEgxpjVmPvld9NzElCCbp-u1ALmhr5cMvFAC10Y8gydyOGCGFjE41pzxI5T4kBE3DSXAvxWlqWm3KrXgnDUlcV0nj_j6cC4vJIZXxjwVgmoc5C8XRZCZi3j8z0yUL9IrNAZog_0dFltbWpjZdvilGaQlGxrwLvBGPA",
            //  "token_type": "bearer",
            //  "expires_in": 1799,
            //  "refresh_token": "566e819b8abe4bc88f54ffe3c20b865f",
            //  "as:client_id": "TestNativeKaliidoClient",
            //  "email": "robbie@kaliido.com",
            //  "quickblox_id": "6743772",
            //  ".issued": "Mon, 06 Jun 2016 08:40:05 GMT",
            //  ".expires": "Mon, 06 Jun 2016 09:10:05 GMT"
            //}

            try
            {
                var client = new RestClient(String.Format(@"{0}token", BASE_AUTH_URL));
                var request = new RestRequest(Method.POST);
                //request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
              
                string requestVariables = String.Format("client_id=TestNativeKaliidoClient&client_secret=123@abc&grant_type=password&username={0}&password={1}&=", username, password);

                Dictionary<string,object> nvc = new Dictionary<string, object>();
                nvc.Add("client_id", "TestNativeKaliidoClient");
                nvc.Add("client_secret", "123@abc");
                nvc.Add("grant_type", "password");

                nvc.Add("username", username);
                nvc.Add("password", password);



                request.AddParameter("application/x-www-form-urlencoded", String.Format("client_id=TestNativeKaliidoClient&client_secret=123@abc&grant_type=password&username={0}&password={1}&=", username, password), ParameterType.RequestBody);

                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");

                IRestResponse response = await client.ExecuteTaskAsync(request);


                dynamic responseObject = Newtonsoft.Json.JsonConvert.DeserializeObject<TokenResponse>(response.Content);
                _globalAccessTokenInformation = responseObject;
                //_globalAccessTokenInformation = new Dictionary<string, object>();
                //_globalAccessTokenInformation.Add("access_token", responseObject.access_token);
                //_globalAccessTokenInformation.Add("token_type", responseObject.token_type);
                //_globalAccessTokenInformation.Add("refresh_token", responseObject.refresh_token);
                //_globalAccessTokenInformation.Add("expires_in", responseObject.expires_in);
                //_globalAccessTokenInformation.Add("email", responseObject.email);
                //_globalAccessTokenInformation.Add("expiresTime", DateTime.Now.AddSeconds(Convert.ToInt16(responseObject.expires_in)));

                return responseObject;
            }catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                throw ex;
            }

        }



        public async Task<List<KaliidoUser>> getUsersClosestByDistance(int distance)
        {
            var client = new RestClient(String.Format(@"{0}/api/user/SearchClosestByDistance/{1}", BASE_RESOURCES_URL, distance.ToString()));
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", String.Format("Bearer {0}", _globalAccessTokenInformation.access_token));
            request.AddHeader("content-type", "application/json");

            request.AddHeader("cache-control", "no-cache");

            IRestResponse response = await client.ExecuteTaskAsync(request);

            List<KaliidoUser> myList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<KaliidoUser>>(response.Content);

            return myList;
        }

        public async Task<dynamic> getUsersClosestByDistance(int distance, double latitude, double longitude)
        {
            var client = new RestClient(String.Format(@"{0}/api/user/SearchClosest/{1}/{2}/{3}", 
                BASE_RESOURCES_URL, distance.ToString(), latitude.ToString(), longitude.ToString()));
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", String.Format("Bearer {0}", _globalAccessTokenInformation.access_token));
            request.AddHeader("content-type", "application/json");

            request.AddHeader("cache-control", "no-cache");

            IRestResponse response = await client.ExecuteTaskAsync(request);

            return Newtonsoft.Json.JsonConvert.DeserializeObject(response.Content);
        }

        public async Task<dynamic> getUserSearchByInterests(int[] interests) // HOW CAN I PASS THAT ARRAY TO THE WEB APIS???
        {
            var client = new RestClient(String.Format(@"{0}/api/user/SearchByInterests", BASE_RESOURCES_URL));
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", String.Format("Bearer {0}", _globalAccessTokenInformation.access_token));
            request.AddHeader("content-type", "application/json");

            request.AddHeader("cache-control", "no-cache");

            IRestResponse response = await client.ExecuteTaskAsync(request);

            return Newtonsoft.Json.JsonConvert.DeserializeObject(response.Content);
        }

        public async Task<dynamic> getUsersByName(string name, int area = 0)
        {
            var client = new RestClient(String.Format(@"{0}/api/user/SearchByName/{1}/{2}", 
                BASE_RESOURCES_URL, name, area.ToString()));
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", String.Format("Bearer {0}", _globalAccessTokenInformation.access_token));
            request.AddHeader("content-type", "application/json");

            request.AddHeader("cache-control", "no-cache");

            IRestResponse response = await client.ExecuteTaskAsync(request);

            return Newtonsoft.Json.JsonConvert.DeserializeObject(response.Content);
        }

        public async Task<dynamic> getUserById(int userid)
        {
            var client = new RestClient(String.Format(@"{0}/api/user/Get/{1}",
                BASE_RESOURCES_URL, userid.ToString()));
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", String.Format("Bearer {0}", _globalAccessTokenInformation.access_token));
            request.AddHeader("content-type", "application/json");

            request.AddHeader("cache-control", "no-cache");

            IRestResponse response = await client.ExecuteTaskAsync(request);

            return Newtonsoft.Json.JsonConvert.DeserializeObject(response.Content);
        }

        public async Task<dynamic> sendFriendRequest(int userid)
        {
            var client = new RestClient(String.Format(@"{0}/api/user/SendFriendRequest/{1}",
                BASE_RESOURCES_URL, userid.ToString()));
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", String.Format("Bearer {0}", _globalAccessTokenInformation.access_token));
            request.AddHeader("content-type", "application/json");

            request.AddHeader("cache-control", "no-cache");

            IRestResponse response = await client.ExecuteTaskAsync(request);

            return Newtonsoft.Json.JsonConvert.DeserializeObject(response.Content);
        }

        public async Task<dynamic> acceptFriendRequest(int userid)
        {
            var client = new RestClient(String.Format(@"{0}/api/user/AcceptFriendRequest/{1}",
                BASE_RESOURCES_URL, userid.ToString()));
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", String.Format("Bearer {0}", _globalAccessTokenInformation.access_token));
            request.AddHeader("content-type", "application/json");

            request.AddHeader("cache-control", "no-cache");

            IRestResponse response = await client.ExecuteTaskAsync(request);

            return Newtonsoft.Json.JsonConvert.DeserializeObject(response.Content);
        }
        public async Task<dynamic> declineFriendRequest(int userid)
        {
            var client = new RestClient(String.Format(@"{0}/api/user/DeclaneFriendRequest/{1}",
                BASE_RESOURCES_URL, userid.ToString()));
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", String.Format("Bearer {0}", _globalAccessTokenInformation.access_token));
            request.AddHeader("content-type", "application/json");

            request.AddHeader("cache-control", "no-cache");

            IRestResponse response = await client.ExecuteTaskAsync(request);

            return Newtonsoft.Json.JsonConvert.DeserializeObject(response.Content);
        }

        public async Task<dynamic> getListOfUsersWithFriendRequests()
        {
            var client = new RestClient(String.Format(@"{0}/api/user/getListOfUsersWithFriendRequests",
                BASE_RESOURCES_URL));
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", String.Format("Bearer {0}", _globalAccessTokenInformation.access_token));
            request.AddHeader("content-type", "application/json");

            request.AddHeader("cache-control", "no-cache");

            IRestResponse response = await client.ExecuteTaskAsync(request);

            return Newtonsoft.Json.JsonConvert.DeserializeObject(response.Content);
        }

        public async Task<dynamic> getListOfFriends()
        {
            var client = new RestClient(String.Format(@"{0}/api/user/getListOfFriends",
                BASE_RESOURCES_URL));
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", String.Format("Bearer {0}", _globalAccessTokenInformation.access_token));
            request.AddHeader("content-type", "application/json");

            request.AddHeader("cache-control", "no-cache");

            IRestResponse response = await client.ExecuteTaskAsync(request);

            return Newtonsoft.Json.JsonConvert.DeserializeObject(response.Content);
        }

        public async Task<dynamic> favorite(int userid)
        {
            var client = new RestClient(String.Format(@"{0}/api/user/Favorite/{1}",
                BASE_RESOURCES_URL, userid.ToString()));
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", String.Format("Bearer {0}", _globalAccessTokenInformation.access_token));
            request.AddHeader("content-type", "application/json");

            request.AddHeader("cache-control", "no-cache");

            IRestResponse response = await client.ExecuteTaskAsync(request);

            return Newtonsoft.Json.JsonConvert.DeserializeObject(response.Content);
        }

        public async Task<dynamic> unFavorite(int userid)
        {
            var client = new RestClient(String.Format(@"{0}/api/user/UnFavorite/{1}",
                BASE_RESOURCES_URL, userid.ToString()));
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", String.Format("Bearer {0}", _globalAccessTokenInformation.access_token));
            request.AddHeader("content-type", "application/json");

            request.AddHeader("cache-control", "no-cache");

            IRestResponse response = await client.ExecuteTaskAsync(request);

            return Newtonsoft.Json.JsonConvert.DeserializeObject(response.Content);
        }

        public async Task<dynamic> report(int userid)
        {
            var client = new RestClient(String.Format(@"{0}/api/user/report/{1}",
                BASE_RESOURCES_URL, userid.ToString()));
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", String.Format("Bearer {0}", _globalAccessTokenInformation.access_token));
            request.AddHeader("content-type", "application/json");

            request.AddHeader("cache-control", "no-cache");

            IRestResponse response = await client.ExecuteTaskAsync(request);

            return Newtonsoft.Json.JsonConvert.DeserializeObject(response.Content);
        }

        public async Task<dynamic> block(int userid)
        {
            var client = new RestClient(String.Format(@"{0}/api/user/block/{1}",
                BASE_RESOURCES_URL, userid.ToString()));
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", String.Format("Bearer {0}", _globalAccessTokenInformation.access_token));
            request.AddHeader("content-type", "application/json");

            request.AddHeader("cache-control", "no-cache");

            IRestResponse response = await client.ExecuteTaskAsync(request);

            return Newtonsoft.Json.JsonConvert.DeserializeObject(response.Content);
        }

        public async Task<dynamic> unBlock(int userid)
        {
            var client = new RestClient(String.Format(@"{0}/api/user/unBlock/{1}",
                BASE_RESOURCES_URL, userid.ToString()));
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", String.Format("Bearer {0}", _globalAccessTokenInformation.access_token));
            request.AddHeader("content-type", "application/json");

            request.AddHeader("cache-control", "no-cache");

            IRestResponse response = await client.ExecuteTaskAsync(request);

            return Newtonsoft.Json.JsonConvert.DeserializeObject(response.Content);
        }

        public async Task<dynamic> updateNote(int userid, string note) // HOW CAN I PASS THE NOTE TO THE WEB APIS???
        {
            var client = new RestClient(String.Format(@"{0}/api/user/UpdateNote/{1}",
                BASE_RESOURCES_URL, userid.ToString()));
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", String.Format("Bearer {0}", _globalAccessTokenInformation.access_token));
            request.AddHeader("content-type", "application/json");

            request.AddHeader("cache-control", "no-cache");

            IRestResponse response = await client.ExecuteTaskAsync(request);

            return Newtonsoft.Json.JsonConvert.DeserializeObject(response.Content);
        }

        public async Task<dynamic> getMyFavoriteUsers()
        {
            var client = new RestClient(String.Format(@"{0}/api/user/GetMyFavoriteUsers",
                BASE_RESOURCES_URL));
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", String.Format("Bearer {0}", _globalAccessTokenInformation.access_token));
            request.AddHeader("content-type", "application/json");

            request.AddHeader("cache-control", "no-cache");

            IRestResponse response = await client.ExecuteTaskAsync(request);

            return Newtonsoft.Json.JsonConvert.DeserializeObject(response.Content);
        }

        public async Task<dynamic> GetFullInfoByUserIds(int[] ids) // HOW CAN I PASS THAT ARRAY TO THE WEB APIS???
        {
            var client = new RestClient(String.Format(@"{0}/api/user/GetFullInfoByUserIds",
                BASE_RESOURCES_URL));
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", String.Format("Bearer {0}", _globalAccessTokenInformation.access_token));
            request.AddHeader("content-type", "application/json");

            request.AddHeader("cache-control", "no-cache");

            IRestResponse response = await client.ExecuteTaskAsync(request);

            return Newtonsoft.Json.JsonConvert.DeserializeObject(response.Content);
        }

        public async Task<dynamic> GetShortInfoByUserIds(int[] ids) // HOW CAN I PASS THAT ARRAY TO THE WEB APIS???
        {
            var client = new RestClient(String.Format(@"{0}/api/user/GetShortInfoByUserIds",
                BASE_RESOURCES_URL));
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", String.Format("Bearer {0}", _globalAccessTokenInformation.access_token));
            request.AddHeader("content-type", "application/json");

            request.AddHeader("cache-control", "no-cache");

            IRestResponse response = await client.ExecuteTaskAsync(request);

            return Newtonsoft.Json.JsonConvert.DeserializeObject(response.Content);
        }

        public async Task<dynamic> GetFullInfoByQuickbloxUserIds(int[] quickBoxIds) // HOW CAN I PASS THAT ARRAY TO THE WEB APIS???
        {
            var client = new RestClient(String.Format(@"{0}/api/user/GetFullInfoByQuickbloxUserIds",
                BASE_RESOURCES_URL));
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", String.Format("Bearer {0}", _globalAccessTokenInformation.access_token));
            request.AddHeader("content-type", "application/json");

            request.AddHeader("cache-control", "no-cache");

            IRestResponse response = await client.ExecuteTaskAsync(request);

            return Newtonsoft.Json.JsonConvert.DeserializeObject(response.Content);
        }

        public async Task<dynamic> UpdateLocation(double latitude, double longitude, decimal accuracy)
        {
            var client = new RestClient(String.Format(@"{0}/api/user/UpdateLocation",
                BASE_RESOURCES_URL));
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", String.Format("Bearer {0}", _globalAccessTokenInformation.access_token));
            request.AddHeader("content-type", "application/json");

            request.AddHeader("cache-control", "no-cache");

            IRestResponse response = await client.ExecuteTaskAsync(request);

            return Newtonsoft.Json.JsonConvert.DeserializeObject(response.Content);
        }

        public Task<dynamic> SignUp(int distance)
        {
            throw new NotImplementedException();
        }
    }
}
