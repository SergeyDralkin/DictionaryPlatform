using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RusDictionary
{
    [Flags]
    public enum JSONFlags
    {
        /// <summary>
        /// Выбрать данные из таблицы
        /// </summary>
        Select = 0,
        /// <summary>
        /// Обновить данные в таблице
        /// </summary>
        Update = 1,
        /// <summary>
        /// Удалить даннные из таблицы
        /// </summary>
        Delete = 2,
        /// <summary>
        /// Вставить данные в таблицу
        /// </summary>
        Insert = 3
    }
}
