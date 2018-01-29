using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HETSAPI.Helpers
{
    /// <summary>
    /// Comma-Separated Value Binding Helper
    /// </summary>
    public class CsvArrayBinder : IModelBinder
    {
        /// <summary>
        /// Converts comma separated values into a typed array.
        /// </summary>
        /// <param name="bindingContext"></param>
        /// <returns></returns>
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (!bindingContext.ModelMetadata.IsEnumerableType)
            {
                return Task.FromResult(0);
            }

            ValueProviderResult csvString = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (csvString.Equals(ValueProviderResult.None))
            {
                return Task.FromResult(0);
            }

            try
            {
                Type elementType = bindingContext.ModelMetadata.ElementType;
                TypeConverter converter = TypeDescriptor.GetConverter(elementType);

                var defaultValues = csvString.FirstValue.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(x => converter.ConvertFromString(x)).ToArray();

                Array typedValues = Array.CreateInstance(elementType, defaultValues.Length);

                defaultValues.CopyTo(typedValues, 0);
                bindingContext.ModelState.SetModelValue(bindingContext.ModelName, csvString);
                bindingContext.Result = ModelBindingResult.Success(typedValues);
            }
            catch (Exception e)
            {
                if (e is FormatException && e.InnerException != null)
                {
                    e = ExceptionDispatchInfo.Capture(e.InnerException).SourceException;
                }

                bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, e, bindingContext.ModelMetadata);
            }

            return Task.FromResult(0);
        }
    }
}
