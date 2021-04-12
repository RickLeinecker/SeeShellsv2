using System.Collections.Generic;
using System.Windows.Media;
using Unity;

using SeeShellsV2.UI;
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

    internal static class Utilities
    {
        /// <summary>
        /// Iterate over WPF Visual Trees
        /// </summary>
        /// <param name="parent">the parent Visual</param>
        /// <param name="recurse">enable to recursively iterate over the tree</param>
        /// <returns>WPF Visual Tree enumerable</returns>
        public static IEnumerable<DependencyObject> GetChildren(this DependencyObject parent, bool recurse = true)
        {
            if (parent != null)
            {
                foreach (var child in LogicalTreeHelper.GetChildren(parent))
                {
                    if (child is DependencyObject d)
                    {
                        yield return d;

                        if (recurse)
                        {
                            foreach (var grandChild in d.GetChildren(true))
                            {
                                yield return grandChild;
                            }
                        }
                    }
                }
            }
        }
    }
}
