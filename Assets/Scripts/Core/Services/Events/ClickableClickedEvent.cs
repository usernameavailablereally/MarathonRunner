using Core.Controllers;

namespace Core.Services.Events
{
    public class ClickableClickedEvent : GameEventBase
    {
        public IClickable Clickable { get; private set; }
        public ClickableClickedEvent(IClickable clickable)
        {
            
        }
    }
}