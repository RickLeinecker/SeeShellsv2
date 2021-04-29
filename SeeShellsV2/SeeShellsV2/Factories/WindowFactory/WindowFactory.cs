using Unity;

using SeeShellsV2.UI;
using SeeShellsV2.Utilities;
using System.Windows;

namespace SeeShellsV2.Factories
{
    /// <summary>
    /// Produces System.Windows.Window objects.
    /// Automatically resolves dependencies of the new window and its children.
    /// </summary>
    public class WindowFactory : IWindowFactory
    {
        /// <summary>
        /// We are wrapping the application's unity container in this factory object in order
        /// to support runtime window initialization without passing container references
        /// to every corner of the codebase.
        /// </summary>
        private readonly IUnityContainer container;

        public WindowFactory([Dependency] IUnityContainer container)
        {
            this.container = container;
        }

        /// <summary>
        /// Create an instance of System.Windows.Window
        /// </summary>
        /// <param name="subtype">name of specific subtype of Window to create</param>
        /// <returns>a new Window</returns>
        public IWindow Create(string name)
        {
            IWindow window = container.Resolve<IWindow>(name);

            // iterate over the window's logical tree and resolve the dependencies of the UI views.
            // services, repositories, and viewmodels are constructed here.
            foreach (var child in (window.Content as DependencyObject).GetChildren())
                container.BuildUp(child.GetType(), child);

            return window;
        }
    }
}
