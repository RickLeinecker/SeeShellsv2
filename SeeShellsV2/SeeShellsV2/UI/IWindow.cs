using System.Windows;

namespace SeeShellsV2.UI
{
    public interface IWindow
    {
        public object Content { get; set; }
        public string Name { get; set; }
        public Window Owner { get; set; }
        public string Title { get; set; }
        public Visibility Visibility { get; set; }

        public void Show();
        public void Hide();
        public void Close();
    }
}
