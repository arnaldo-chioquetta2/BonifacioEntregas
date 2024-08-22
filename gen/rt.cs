using System.Drawing;
using System.Windows.Forms;

namespace TeleBonifacio
{
    public static class rt
    {
        public static float scaleFactor = 1.4f;

        public static bool IsLargeScreen()
        {
            Rectangle resolution = Screen.PrimaryScreen.Bounds;
            return resolution.Width >= 1920 && resolution.Height >= 1080;
            // return true;
        }

        public static void AdjustControl(Control ctrl, Form form)
        {
            // Ajuste da fonte para todos os controles
            if (ctrl.Font != null)
            {
                ctrl.Font = new Font(ctrl.Font.FontFamily, ctrl.Font.Size * scaleFactor, ctrl.Font.Style);
            }

            // Redimensionamento e repositionamento de controles
            ctrl.Size = new Size((int)(ctrl.Width * scaleFactor), (int)(ctrl.Height * scaleFactor));
            ctrl.Location = new Point((int)(ctrl.Location.X * scaleFactor), (int)(ctrl.Location.Y * scaleFactor));

            // Especificamente para botões, ajustar o tamanho do texto e a posição baseada na ancoragem
            if (ctrl is Button btn)
            {
                AdjustButtonText(btn);
                // Ajuste para manter à direita com margem consistente
                if (btn.Anchor.HasFlag(AnchorStyles.Right))
                {
                    btn.Location = new Point(form.ClientSize.Width - btn.Width - 20, btn.Location.Y);
                }
            }

            // Recursivamente ajustar controles filhos
            foreach (Control child in ctrl.Controls)
            {
                AdjustControl(child, form);
            }

            if (ctrl is DataGridView dgv)
            {
                AdjustDataGridView(dgv);
            }
        }

        public static void AdjustButtonText(Button btn)
        {
            Graphics g = btn.CreateGraphics();
            SizeF size = g.MeasureString(btn.Text, btn.Font);

            // Se o texto estiver muito largo para o botão, aumente a largura do botão.
            if (size.Width > btn.Width - 10) // 10 pixels de margem
            {
                btn.Width = (int)(size.Width + 10);
            }
            g.Dispose();
        }

        public static void AdjustFormComponents(Form form)
        {
            if (glo.Adaptar && IsLargeScreen())
            {
                form.Size = new Size((int)(form.Width * scaleFactor), (int)(form.Height * scaleFactor));
                foreach (Control ctrl in form.Controls)
                {
                    AdjustControl(ctrl, form);
                }
            }
        }

        public static void AdjustDataGridView(DataGridView dgv)
        {
            float scaleFactor = 1.4f;
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 12 * scaleFactor);
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12 * scaleFactor, FontStyle.Regular);
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgv.ColumnHeadersHeight = (int)(dgv.ColumnHeadersHeight * scaleFactor);

            foreach (DataGridViewColumn col in dgv.Columns)
            {
                col.Width = (int)(col.Width * scaleFactor);
            }

            dgv.RowTemplate.Height = (int)(dgv.RowTemplate.Height * scaleFactor);
            foreach (DataGridViewRow row in dgv.Rows)
            {
                row.Height = (int)(row.Height * scaleFactor);
            }
        }


    }
}
