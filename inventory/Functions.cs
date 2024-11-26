using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace inventory
{
    public static class Functions
    {
        // Muestra un formulario dentro de un panel, limpiando previamente cualquier contenido del panel.
        public static void ShowForm(TabPage page, Form form)
        {
            page.Controls.Clear();

            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            //form.Dock = DockStyle.Fill;

            page.Controls.Add(form);
            form.Show();
        }

        // Funciones para permitir mover un formulario desde su barra de título.
        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        private const int WM_SYSCOMMAND = 0x0112;
        private const int SC_MOVE = 0xF010;
        private const int HTCAPTION = 0x0002;

        // Permite mover el formulario haciendo clic y arrastrando en cualquier parte del formulario.
        public static void DragFrom(Control control)
        {
            control.MouseDown += (sender, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    ReleaseCapture();
                    SendMessage(control.FindForm().Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
                    //SendMessage(control.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
                }
            };
        }

        // Función para redondear los bordes de un formulario.
        [DllImport("gdi32.dll", SetLastError = true)]
        private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

        // Aplica bordes redondeados a un formulario con el radio especificado.
        public static void RoundForm(Form form, int radio)
        {
            IntPtr rgn = CreateRoundRectRgn(0, 0, form.Width, form.Height, radio, radio);
            form.Region = Region.FromHrgn(rgn);
        }
    }
}
