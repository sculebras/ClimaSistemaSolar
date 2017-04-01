using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.Core.Extensions
{
    /// <summary>
    /// Extensión para enumeraciones con valores string en las descripciones.
    /// </summary>
    public static class EnumExtension
    {
        /// <summary>
        /// Devuelve el valor  string del atributo Description del item de la enumeracion.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum value)
        {
            var descriptionAttribute = (DescriptionAttribute)value.GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(false)
                .Where(a => a is DescriptionAttribute)
                .FirstOrDefault();

            return descriptionAttribute != null ? descriptionAttribute.Description : value.ToString();
        }

        /// <summary>
        /// Devuelve el Nombre del item de la enumeracion.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetName(this Enum value)
        {
            return value.GetType().GetField(value.ToString()).Name;
        }

        /// <summary>
        /// Devuelve a la enumeracion como una coleccion para que pueda ser iterada.
        /// </summary>
        /// <typeparam name="T">Tipo de la Enumeracion</typeparam>
        /// <returns></returns>
        public static T[] GetCollection<T>()
        {
            return (T[])System.Enum.GetValues(typeof(T));
        }
    }

}
