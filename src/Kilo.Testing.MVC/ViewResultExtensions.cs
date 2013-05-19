using System;
using System.Diagnostics;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kilo.Testing.Mvc
{
    internal static class ViewResultExtensions
    {
        /// <summary>
        /// Asserts that the view model is of the correct type.
        /// </summary>
        /// <typeparam name="T">The model type to assert</typeparam>
        [DebuggerHidden]
        internal static void AssertModelIsOfType<T>(this ViewResultBase result)
        {
            AssertHasModel(result);
            Assert.IsInstanceOfType(result.Model, typeof(T));
        }

        /// <summary>
        /// Asserts that the JSON model is of the correct type.
        /// </summary>
        /// <typeparam name="T">The type to assert</typeparam>
        [DebuggerHidden]
        internal static void AssertModelIsOfType<T>(this JsonResult result)
        {
            AssertHasModel(result);
            Assert.IsInstanceOfType(result.Data, typeof(T));
        }

        /// <summary>
        /// Asserts that the default view has been returned from the action. The default view is defined as being an empty string.
        /// </summary>
        [DebuggerHidden]
        internal static void AssertHasDefaultView(this ViewResultBase result)
        {
            Assert.AreEqual(string.Empty, result.ViewName);
        }

        /// <summary>
        /// Asserts that the specified view name has been returned from the action.
        /// </summary>
        /// <param name="expected">The expected view name</param>
        /// <param name="ignoreCase">Whether case should be ignored or not.</param>
        [DebuggerHidden]
        internal static void AssertViewName(this ViewResultBase result, string expected, bool ignoreCase = false)
        {
            Assert.AreEqual(expected, result.ViewName, ignoreCase);
        }

        /// <summary>
        /// Asserts whether a model has been returned from the view or not.
        /// </summary>
        [DebuggerHidden]
        internal static void AssertHasModel(this ViewResultBase result)
        {
            Assert.IsNotNull(result.Model);
        }

        /// <summary>
        /// Asserts whether a model has been returned from the view or not.
        /// </summary>
        [DebuggerHidden]
        internal static void AssertHasModel(this JsonResult result)
        {
            Assert.IsNotNull(result.Data);
        }

        /// <summary>
        /// Asserts that no model has bee returned from the view.
        /// </summary>
        [DebuggerHidden]
        internal static void AssertEmptyModel(this ViewResultBase result)
        {
            Assert.IsNull(result.Model);
        }

        /// <summary>
        /// Verifies that the model meets the specified criteria
        /// </summary>
        /// <typeparam name="T">The type of the model</typeparam>
        /// <param name="result">The result containing the model to test</param>
        /// <param name="predicate">The predicate to test the model against</param>
        [DebuggerHidden]
        internal static void VerifyModel<T>(this ViewResultBase result, Func<T, bool> predicate) where T : class
        {
            Assert.IsNotNull(result);
            VerifyModel<T>(result.Model as T, predicate);
        }

        /// <summary>
        /// Verifies that the model meets the specified criteria
        /// </summary>
        /// <typeparam name="T">The type of the model</typeparam>
        /// <param name="result">The result containing the model to test</param>
        /// <param name="predicate">The predicate to test the model against</param>
        [DebuggerHidden]
        internal static void VerifyModel<T>(this JsonResult result, Func<T, bool> predicate) where T : class
        {
            Assert.IsNotNull(result);
            VerifyModel<T>(result.Data as T, predicate);
        }

        /// <summary>
        /// Verifies the model.
        /// </summary>
        /// <typeparam name="T">The model type</typeparam>
        /// <param name="model">The model.</param>
        /// <param name="predicate">The predicate.</param>
        [DebuggerHidden]
        internal static void VerifyModel<T>(T model, Func<T, bool> predicate) where T : class
        {
            Assert.IsNotNull(model);
            Assert.IsInstanceOfType(model, typeof(T));
            Assert.IsTrue(predicate(model as T));
        }
    }
}
