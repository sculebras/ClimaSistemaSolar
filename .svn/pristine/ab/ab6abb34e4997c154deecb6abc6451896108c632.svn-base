using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SF.Core.CustomAnnotations
{
    //2014-09-04 11:56:07 sculebras: 
    /// <summary>
    /// General Validations for model classes Custom Annotations.
    /// </summary>
    public class CustomAnnotationsValidations
    {
        /// <summary>
        /// Gets a PropertyInfo from a validation Context according with the property name.
        /// Validates if property name exists and checks the Type.
        /// </summary>
        /// <typeparam name="T">Type that will be evaluated against the property</typeparam>
        /// <param name="oValidationContext">Validation context.</param>
        /// <param name="strPropertyName">Name of the property.</param>
        /// <returns></returns>
        public static PropertyInfo getPropertyInfo<T>(ValidationContext oValidationContext,
                                                      string strPropertyName)
        {
            if (oValidationContext== null) throw new ArgumentNullException("oValidationContext");
            if (oValidationContext.ObjectInstance == null) throw new ArgumentNullException("oValidationContext.ObjectInstance");
            if (string.IsNullOrWhiteSpace(strPropertyName)) throw new ArgumentNullException("strPropertyName");
            
            PropertyInfo oPropertyInfo = oValidationContext.ObjectType.GetProperty(strPropertyName);
            if (oPropertyInfo==null)
            {
                throw new ApplicationException(string.Format("Model class annotation definition error.\nA property with name '{0}' doesn´t exist in model class {1}.\nCheck annotations in the model class.", strPropertyName, oValidationContext.ObjectType.Name));
            }
            if (!oPropertyInfo.PropertyType.Equals(typeof(T)))
            {
                throw new ApplicationException(String.Format("Model class annotation definition error.\nThe property {0} is not of type {1} in model class: '{2}'.\nCheck annotations in the model class.", oPropertyInfo.Name, typeof(T).Name, oValidationContext.ObjectType.Name));
            }
            return oPropertyInfo; 
        }

        /// <summary>
        /// Gets a property value according to the property name.
        /// Validates exsitance of the property and type.
        /// </summary>
        /// <typeparam name="T">Type of the property that will be evaluated against the property.</typeparam>
        /// <param name="oValidationContext">Validation context.</param>
        /// <param name="strPropertyName">Name of the property.</param>
        /// <returns></returns>
        public static T getPropertyValue<T>(ValidationContext oValidationContext, string strPropertyName)
        {
            PropertyInfo oPropertyInfo = CustomAnnotationsValidations.getPropertyInfo<T>(oValidationContext, strPropertyName);
            return (T)oPropertyInfo.GetValue(oValidationContext.ObjectInstance);
        }


    }//End class
}//End Namespace				
				
