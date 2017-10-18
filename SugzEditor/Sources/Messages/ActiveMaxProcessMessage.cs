using SugzEditor.ViewModels;

namespace SugzEditor.Messages
{
	public class ActiveMaxProcessMessage
    {
		public MaxProcessViewModel MaxProcess { get; protected set; }
		public ActiveMaxProcessMessage(MaxProcessViewModel maxProcess)
		{
			MaxProcess = maxProcess;
		}
	}
}
