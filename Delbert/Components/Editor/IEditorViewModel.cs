using Delbert.Infrastructure.Abstract;
using Delbert.Model;

namespace Delbert.Components.Editor
{
    public interface IEditorViewModel : IScreenViewModel
    {
        string Text { get; }
        PageDto Page { get; }
        void EditorSelected();
    }
}