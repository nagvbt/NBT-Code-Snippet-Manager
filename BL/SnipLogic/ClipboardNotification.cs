using System;

namespace NagCode.BL.SnipLogic
{
  public sealed class ClipboardNotification
  {
    /// <summary>
    /// Occurs when the contents of the clipboard is updated.
    /// </summary>
    public static event EventHandler ClipboardUpdate;

    private static NotificationForm _form = new NotificationForm();

    /// <summary>
    /// Raises the <see cref="ClipboardUpdate"/> event.
    /// </summary>
    /// <param name="e">Event arguments for the event.</param>
    public static void OnClipboardUpdate(EventArgs e)
    {
      ClipboardUpdate?.Invoke(null, e);
    }

    
  }
}
