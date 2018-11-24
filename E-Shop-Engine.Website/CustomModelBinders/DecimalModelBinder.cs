using System;
using System.Globalization;
using System.Web.Mvc;

namespace E_Shop_Engine.Website.CustomModelBinders
{
    // Add this model binder to use "." or "," as separator.
    public class DecimalModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            object result = null;
            string modelName = bindingContext.ModelName;
            string attemptedValue = bindingContext.ValueProvider.GetValue(modelName).AttemptedValue;

            string wantedSeparator = NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;
            string alternateSeparator = (wantedSeparator == "," ? "." : ",");

            if (attemptedValue.IndexOf(wantedSeparator) == -1 && attemptedValue.IndexOf(alternateSeparator) != -1)
            {
                attemptedValue = attemptedValue.Replace(alternateSeparator, wantedSeparator);
            }

            try
            {
                if (bindingContext.ModelMetadata.IsNullableValueType && string.IsNullOrWhiteSpace(attemptedValue))
                {
                    return null;
                }

                result = decimal.Parse(attemptedValue, NumberStyles.Any);
            }
            catch (FormatException e)
            {
                bindingContext.ModelState.AddModelError(modelName, e);
            }

            return result;
        }
    }
}