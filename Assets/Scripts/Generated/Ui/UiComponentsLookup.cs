//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentLookupGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public static class UiComponentsLookup {

    public const int GlobalEvents = 0;
    public const int Animation = 1;
    public const int Close = 2;
    public const int Protected = 3;
    public const int Cancel = 4;
    public const int Check = 5;
    public const int Confirm = 6;
    public const int Pressed = 7;
    public const int Reject = 8;
    public const int CloseAll = 9;
    public const int CloseWindows = 10;
    public const int OpenWindow = 11;
    public const int Container = 12;
    public const int CreateWindow = 13;
    public const int View = 14;

    public const int TotalComponents = 15;

    public static readonly string[] componentNames = {
        "GlobalEvents",
        "Animation",
        "Close",
        "Protected",
        "Cancel",
        "Check",
        "Confirm",
        "Pressed",
        "Reject",
        "CloseAll",
        "CloseWindows",
        "OpenWindow",
        "Container",
        "CreateWindow",
        "View"
    };

    public static readonly System.Type[] componentTypes = {
        typeof(Main.Globals.GlobalEventsComponent),
        typeof(UI.Animation.AnimationComponent),
        typeof(UI.Close.CloseComponent),
        typeof(UI.Close.ProtectedComponent),
        typeof(UI.Events.CancelComponent),
        typeof(UI.Events.CheckComponent),
        typeof(UI.Events.ConfirmComponent),
        typeof(UI.Events.PressedComponent),
        typeof(UI.Events.RejectComponent),
        typeof(UI.Global.CloseAllComponent),
        typeof(UI.Global.CloseWindowsComponent),
        typeof(UI.Global.OpenWindowComponent),
        typeof(UI.Open.ContainerComponent),
        typeof(UI.Open.CreateWindowComponent),
        typeof(UI.ViewComponent)
    };
}
