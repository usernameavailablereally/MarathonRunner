namespace Core.Services.Events
{
    public class RoundOverEvent : GameEventBase
    {
        public float TimeRecord;
        public RoundOverEvent(float timeRecord) 
        {
            TimeRecord = timeRecord;
        }
    }
}