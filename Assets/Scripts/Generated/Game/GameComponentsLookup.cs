//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentLookupGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public static class GameComponentsLookup {

    public const int Animator = 0;
    public const int Destroyed = 1;
    public const int PlayerController = 2;
    public const int Follow = 3;
    public const int Motion = 4;
    public const int MoveInDirection = 5;
    public const int View = 6;
    public const int Collider = 7;
    public const int Reaction = 8;

    public const int TotalComponents = 9;

    public static readonly string[] componentNames = {
        "Animator",
        "Destroyed",
        "PlayerController",
        "Follow",
        "Motion",
        "MoveInDirection",
        "View",
        "Collider",
        "Reaction"
    };

    public static readonly System.Type[] componentTypes = {
        typeof(Game.Animation.AnimatorComponent),
        typeof(Game.Cleanup.DestroyedComponent),
        typeof(Game.Controllers.PlayerControllerComponent),
        typeof(Game.Follow.FollowComponent),
        typeof(Game.Motion.MotionComponent),
        typeof(Game.Motion.MoveInDirectionComponent),
        typeof(Game.Motion.ViewComponent),
        typeof(Game.Reaction.ColliderComponent),
        typeof(Game.Reaction.ReactionComponent)
    };
}
