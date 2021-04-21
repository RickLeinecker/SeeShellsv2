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

            foreach (var child in (window.Content as DependencyObject).GetChildren())
                container.BuildUp(child.GetType(), child);

            return window;
        }
    }
}
