using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CocktionMVC.Functions.DataProcessing
{
    public class HouseSerializer
    {
        public static int[] TakeHouseIdsFromString(string housesString)
        {
            string[] idString = housesString.TrimEnd('!').TrimStart('?').Split('!');
            int[] ids = new int[idString.Length];
            for (int i = 0; i < idString.Length; i++)
            {
                ids[i] = int.Parse(idString[i]);
            }
            return ids;
        }
    }
}