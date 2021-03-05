namespace Main.Globals
{
    public static class EcsEnvironment
    {
        public static GameEntity GameEventsEntity => Contexts.sharedInstance.game.globalEventsEntity;
        
        public static UiEntity UiEventsEntity => Contexts.sharedInstance.ui.globalEventsEntity;

        public static InputEntity InputEventsEntity => Contexts.sharedInstance.input.globalEventsEntity;
        
        public static CoreEntity CoreEventsEntity => Contexts.sharedInstance.core.globalEventsEntity;
    }
}