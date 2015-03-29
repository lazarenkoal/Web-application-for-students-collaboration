using System;
using System.Collections.Generic;
using System.Linq;

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
            int dictLength = data.Count;
            string result = "";

            //Достаем все ключи и значения из словаря
            string[] key = data.Select(x => x.Key).ToArray();
            double[] value = data.Select(x => x.Value).ToArray();

            //Формируем результирующий массив данных
            for (int i = 0; i < dictLength; i++)
            {
                result += String.Format("Товар {0} с коэффициентом {1:f2} \n", key[i], value[i]);
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