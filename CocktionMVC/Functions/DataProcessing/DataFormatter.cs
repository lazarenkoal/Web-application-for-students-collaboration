using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace CocktionMVC.Functions
{
    public class DataFormatter
    {
        /// <summary>
        /// Метод для формирвания данных из словаря
        /// для последующей отправки клиентам
        /// Данный метод формирует строку с информацией о коэффициентах
        /// для всех продуктов.
        /// </summary>
        /// <param name="data">Словарь, который необходимо отформатировать</param>
        /// <returns>Массив типа string с данными</returns>
        public static string DictionaryConverter(Dictionary<string, double> data)
        {
            string result ="";
            int dictLength = data.Count;
            if (dictLength == 0)
            {
                result = "Еще никто из пользователей не поставил ставки на аукционе";
            }
            else
            {
                //Достаем все ключи и значения из словаря
                string[] key = data.Select(x => x.Key).ToArray();
                double[] value = data.Select(x => x.Value).ToArray();

                //Формируем результирующий массив данных
                for (int i = 0; i < dictLength; i++)
                {
                    result += (String.Format("{0} => {1:f2} \n", key[i], value[i]));
                }

            }
            
            //возвращаем результат
            return result;
        }

        /// <summary>
        /// Из строки в формате 1!2!34!48
        /// достает айдишники аукционных домов
        /// </summary>
        /// <param name="housesId">Строка с зашифрованными айдишниками домов</param>
        /// <returns>Интовый массив с номерами</returns>
        public static int[] GetHouseIds(string housesId)
        {
            string[] locIds = housesId.Split('!');
            int[] locIdsInt = Array.ConvertAll(locIds, x => int.Parse(x));
            return locIdsInt;
        }
    }
}