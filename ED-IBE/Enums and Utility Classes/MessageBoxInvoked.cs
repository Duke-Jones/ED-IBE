using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows.Forms
{
    class MessageBoxInvoked
    {
        public static DialogResult Show(Form parent, string message, string headline, MessageBoxButtons messageBoxButtons, MessageBoxIcon messageBoxIcon, MessageBoxDefaultButton messageBoxDefaultButton = MessageBoxDefaultButton.Button1)
        {
            if (parent.InvokeRequired)
            {
                return (DialogResult) parent.Invoke(new Func<DialogResult>(
                                        () => { return MessageBox.Show(parent, message, headline, messageBoxButtons, messageBoxIcon, messageBoxDefaultButton); }));
            }
            else
            {
                return MessageBox.Show(parent, message, headline, messageBoxButtons, messageBoxIcon, messageBoxDefaultButton);
            }
        }
    }
    class FolderBrowserDialogInvoked
    {
        public DialogResult ShowDialog(Form primaryGUI)
        {
            throw new NotImplementedException();
        }
    }
}
