using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KaliidoAPI;

namespace KaliidoAPITest
{
    class Program
    {

        public static KaliidoServiceFactory serviceFactory;

        static void Main(string[] args)
        {
           

            serviceFactory = new KaliidoServiceFactory().Instance;
          Process();

            Console.ReadKey();

        }

        public static async void Process()
        {
            TokenResponse response = await serviceFactory.loginSession("robbie@kaliido.com", "Matt2105*");

            if (response.access_token != null)
            {
                dynamic myResult;

                Console.WriteLine("==============================================================================================");
                Console.WriteLine("test serviceFactory.getUsersClosestByDistance(2000)");
                Console.WriteLine("==============================================================================================");
                try
                {
                    myResult = await serviceFactory.getUsersClosestByDistance(2000);
                    //Java.lang.

                    Console.WriteLine(myResult);
                } catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

                Console.WriteLine("==============================================================================================");
                Console.WriteLine("test serviceFactory.getUsersClosestByDistance(2000, -33.876607, 151.213734)");
                Console.WriteLine("==============================================================================================");
                try
                {
                    myResult = await serviceFactory.getUsersClosestByDistance(2000, 33, 15);
                    Console.WriteLine(myResult);
                } catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

                Console.WriteLine("==============================================================================================");
                Console.WriteLine("test serviceFactory.getUserSearchByInterests(int[] interests)");
                Console.WriteLine("==============================================================================================");
                try
                {  //doesnt work
                    int[] myInterests = { 1, 2, 3 };
                    myResult = await serviceFactory.getUserSearchByInterests(myInterests);
                    Console.WriteLine(myResult);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                
                Console.WriteLine("==============================================================================================");
                Console.WriteLine("test serviceFactory.getUsersByName(string name, int area = 0)");
                Console.WriteLine("==============================================================================================");
                try
                {
                    myResult = await serviceFactory.getUsersByName("Robbie", 0);
                    Console.WriteLine(myResult);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

                //Console.WriteLine("==============================================================================================");
                Console.WriteLine("test serviceFactory.getUserById(int userid)");
                Console.WriteLine("==============================================================================================");
                try
                {
                    myResult = await serviceFactory.getUserById(20102);
                    Console.WriteLine(myResult);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

                Console.WriteLine("==============================================================================================");
                Console.WriteLine("test serviceFactory.sendFriendRequest(int userid)");
                Console.WriteLine("==============================================================================================");
                try
                {  // doesnt work
                    myResult = await serviceFactory.sendFriendRequest(20102);
                    Console.WriteLine(myResult);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            else
            {
                Console.WriteLine("failed to authenticate");
            }

        }



        //public async Task<bool> ProcessLogin(string username, string password)
        //{

        //    dynamic response = await serviceFactory.loginSession("robbie@kaliido.com", "Matt2105*");

        //    if (response.access_token != null)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }


        //}

        //public async Task<bool> ProcessLogin(string username, string password)
        //{

        //    dynamic response = await serviceFactory.loginSession("robbie@kaliido.com", "Matt2105*");

        //    if (response.access_token != null)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }


        //}




    }
}
