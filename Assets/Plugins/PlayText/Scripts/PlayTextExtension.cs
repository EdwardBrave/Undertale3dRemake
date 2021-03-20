using PlayTextSupport;

namespace GraphSpace
{
    public static class PlayTextExtension
    {
        public static void Play(this DialogueGraph graph)
        {
            EventCenter.GetInstance().EventTriggered("PlayText.Play", graph);
        }

        public static void Next()
        {
            EventCenter.GetInstance().EventTriggered("PlayText.Next");
        }
        
        public static void OptionUp()
        {
            EventCenter.GetInstance().EventTriggered("PlayText.OptionUp");
        }
        
        public static void OptionDown()
        {
            EventCenter.GetInstance().EventTriggered("PlayText.OptionDown");
        }
        
        public static void Stop()
        {
            EventCenter.GetInstance().EventTriggered("PlayText.Stop");
        }
    }
}