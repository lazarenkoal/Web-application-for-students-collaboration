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
        /// Данный метод формирует строку с информацией о коэффициентах
        /// для всех продуктов.
        /// </summary>
        /// <param name="data">Словарь, который необходимо отформатировать</param>
        /// <returns>Массив типа string с данными</returns>
        public static string DictionaryConverter(Dictionary<string, double> data)
        {
            int dictLength = data.Count;
            string[] key = new string[dictLength];
            double[] value = new double[dictLength];
            string result = "";

            //Достаем все ключи и значения из словаря
            key = data.Select(x => x.Key).ToArray();
            value = data.Select(x => x.Value).ToArray();

            //Формируем результирующий массив данных
            for (int i = 0; i < dictLength; i++)
            {
                result += String.Format("Товар {0} с коэффициентом {1}", key[i], value[i]) + "\n";
            }

            //возвращаем результат
            return result;
        }
    }
}