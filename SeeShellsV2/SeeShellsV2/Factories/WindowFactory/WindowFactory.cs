using System.Collections.Generic;
using System.Windows.Media;
using Unity;

using SeeShellsV2.UI;

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

            foreach (var child in (window.Content as Visual).GetChildren())
            {
                if (child is AvalonDock.DockingManager)
                {
                    var dockableEnumerator = (child as AvalonDock.DockingManager).LogicalChildrenPublic;
                    while (dockableEnumerator.MoveNext())
                    {
                        container.BuildUp(dockableEnumerator.Current.GetType(), dockableEnumerator.Current);

                        foreach (var child2 in (dockableEnumerator.Current as Visual).GetChildren())
                        {
                            container.BuildUp(child2.GetType(), child2);
                        }
                    }
                }
                else
                {
                    container.BuildUp(child.GetType(), child);
                }
            }

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
        public static IEnumerable<Visual> GetChildren(this Visual parent, bool recurse = true)
        {
            if (parent != null)
            {
                int count = VisualTreeHelper.GetChildrenCount(parent);
                for (int i = 0; i < count; i++)
                {
                    // Retrieve child visual at specified index value.
                    var child = VisualTreeHelper.GetChild(parent, i) as Visual;

                    if (child != null)
                    {
                        yield return child;

                        if (recurse)
                        {
                            foreach (var grandChild in child.GetChildren(true))
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
