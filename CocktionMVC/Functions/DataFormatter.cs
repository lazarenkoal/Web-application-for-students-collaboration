using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CocktionMVC.Functions
{
    public class DataFormatter
    {
        /// <summary>
        /// Метод для формирвания данных из словаря
        /// для последующей отправки клиентам
        /// </summary>
        /// <param name="data">Словарь, который необходимо отформатировать</param>
        /// <returns>Массив типа string с данными</returns>
        public static string DictionaryConverter(Dictionary<string, int> data)
        {
            int dictLength = data.Count;
            string[] key = new string[dictLength];
            int[] value = new int[dictLength];
            string result = "";

            //Достаем все ключи и значения из словаря
            key = data.Select(x => x.Key).ToArray();
            value = data.Select(x => x.Value).ToArray();

            //Формируем результирующий массив данных
            for (int i = 0; i < dictLength; i++)
            {
                result += String.Format("На товар с номером {0} поставили {1} яиц", key[i], value[i]) + "\n";
            }

            //возвращаем результат
            return result;
        }
    }
}