using System;

namespace NagCode.BL
{
    /// <summary>
    /// Author: NBT
    /// ClipboardNotification for managing clipboard notifications
    /// </summary>
    public sealed class ClipboardNotification
    {
        /// <summary>
        /// The contents of the clipboard is updated.
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
