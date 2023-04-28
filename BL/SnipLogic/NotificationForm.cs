using System.Windows.Forms;

namespace NagCode.BL.SnipLogic
{
  /// <summary>
  /// Hidden form to recieve the WM_CLIPBOARDUPDATE message.
  /// </summary>
  public class NotificationForm : Form
  {
    public NotificationForm()
    {
      NativeMethods.SetParent(Handle, NativeMethods.HWND_MESSAGE);
      NativeMethods.AddClipboardFormatListener(Handle);
    }

    protected override void WndProc(ref Message m)
    {
      if (m.Msg == NativeMethods.WM_CLIPBOARDUPDATE)
      {
        ClipboardNotification.OnClipboardUpdate(null);
      }
      base.WndProc(ref m);
    }
  }
}
