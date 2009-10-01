using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace WebFormsMvp.Binder
{
    /// <summary>
    /// Represents cached method information required to automatically wire up presenters to views in a Web Forms Model-View-Presenter application
    /// </summary>
    internal class PresenterBindInfo
    {
        /// <summary>
        /// Gets or sets the type of the presenter.
        /// </summary>
        /// <value>The type of the presenter.</value>
        public Type PresenterType { get; private set; }
        
        /// <summary>
        /// Gets or sets the type of the view.
        /// </summary>
        /// <value>The type of the view.</value>
        public Type ViewType { get; private set; }

        DynamicMethod dynamicMethod;

        /// <summary>
        /// Initializes a new instance of the <see cref="PresenterBindInfo"/> class.
        /// </summary>
        /// <param name="presenterType">Type of the presenter.</param>
        /// <param name="viewType">Type of the view.</param>
        /// <param name="resolveDependencies">if set to <c>true</c> public properties of the presenter will be resolved using the configured IOC container.</param>
        public PresenterBindInfo(Type presenterType, Type viewType)
        {
            PresenterType = presenterType;
            ViewType = viewType;

            var constructor = PresenterType.GetConstructor(new Type[] { ViewType });
            if (constructor == null)
            {
                throw new InvalidOperationException(string.Format("{0} is missing expected constructor.", PresenterType.Name));
            }

            dynamicMethod = new DynamicMethod("DynamicConstructor", PresenterType, new Type[] { ViewType }, PresenterType.Module, true);
            ILGenerator ilgen = dynamicMethod.GetILGenerator();
            ilgen.Emit(OpCodes.Nop);
            ilgen.Emit(OpCodes.Ldarg_0);
            ilgen.Emit(OpCodes.Newobj, constructor);
            ilgen.Emit(OpCodes.Ret);
        }

        /// <summary>
        /// Creates the presenter and wires it up to the specified view.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <returns>The presenter.</returns>
        public IPresenter Create(IView view)
        {
            return (IPresenter)dynamicMethod.Invoke(null, new object[] { view });
        }
    }
}