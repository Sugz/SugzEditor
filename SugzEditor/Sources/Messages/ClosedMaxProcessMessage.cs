using SugzEditor.ViewModels;

namespace SugzEditor.Messages
{
	public class ClosedMaxProcessMessage
	{
		public MaxProcessViewModel MaxProcess { get; protected set; }
		public ClosedMaxProcessMessage(MaxProcessViewModel maxProcess)
		{
			MaxProcess = maxProcess;
		}
	}
}
