using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimpleInjector;
using WDACC.Services;

namespace WDACC.Commons
{
    public class MyModelBinder
    {
        private Container container;
        private ModelBinderDictionary binders;
        public ControllerContext controllerContext { get; set; }
        public ModelStateDictionary modelState { get; set; }

        public MyModelBinder(Container container)
        {
            this.container = container;
        }

        public void SetContext(ControllerContext controllerContext, ModelBinderDictionary binders, ModelStateDictionary modelState)
        {
            this.controllerContext = controllerContext;
            this.binders = binders;
            this.modelState = modelState;
        }

        public T BindModel<T>(IValueProvider valueProvider)
        {
            Type type = typeof(T);
            T model = (T)container.GetInstance(type);

            if (valueProvider != null)
            {
                Type modelType = model.GetType();
                var binder = this.binders.GetBinder(type);
                var bindingContext = new ModelBindingContext()
                {
                    ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => model, modelType),
                    ModelState = this.modelState,
                    ValueProvider = valueProvider
                };
                binder.BindModel(this.controllerContext, bindingContext);
            }

            return model;
        }

        public T BindModel<T>(ControllerContext controllerContext,
                ModelBinderDictionary binders,
                ModelStateDictionary modelState,
                FormCollection collection)
        {
            Type type = typeof(T);
            T model = (T)container.GetInstance(type);

            if (collection != null)
            {
                Type modelType = model.GetType();
                var binder = binders.GetBinder(type);
                var bindingContext = new ModelBindingContext()
                {
                    ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => model, modelType),
                    ModelState = modelState,
                    ValueProvider = collection
                };
                binder.BindModel(controllerContext, bindingContext);
            }

            return model;
        }

        public T BindSimply<T>(string name, IValueProvider valueProvider)
        {
            object result = null;
            var value = valueProvider.GetValue(name);
            if (value != null)
            {
                if (typeof(T) == typeof(long?))
                {
                    result = System.Convert.ToInt64(value.AttemptedValue);
                }
                else
                {
                    result = value.AttemptedValue.ToString();
                }
            }

            return (T)result;
        }

    }
}
