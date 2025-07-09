using System;

namespace GGCustomToolbar
{
    [AttributeUsage(AttributeTargets.Method)]
    public class EditorToolbarButtonAttribute : Attribute
    {
        public string IconName { get; }
        public string Tooltip { get; }
        public int Priority { get; } = 0;
        public EditorToolbarPosition Position { get; } = EditorToolbarPosition.LeftLeft;
        public bool DisableOnPlayMode { get; } = false;
        public EditorToolbarButtonAttribute() { }
        public EditorToolbarButtonAttribute(string iconName, string tooltip = "", int priority = 0, EditorToolbarPosition position = EditorToolbarPosition.LeftLeft, bool disableOnPlayMode = false)
        {
            IconName = iconName;
            Tooltip = tooltip;
            Priority = priority;
            Position = position;
            DisableOnPlayMode = disableOnPlayMode;
        }

        public EditorToolbarButtonAttribute(string iconName, bool disableOnPlayMode)
        {
            IconName = iconName;
            DisableOnPlayMode = disableOnPlayMode;
        }
    }
}
