using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Printing;

namespace TeleBonifacio.rel
{
    public class Receipt : IDisposable
    {
        public string storeName = "";
        public string storeAddress = "";
        public string storePhone = "";
        public string customerName = "";
        public string customerAddress = "";
        public string datecustomerAddress = "";
        public string time = "";
        public string invoiceNumber = "";
        public string items = "";
        public string date = "";

        public string total = "";
        public string paymentMethod = "";
        private readonly Font _font;
        private readonly Pen _pen;
        private readonly Brush _brush;

        public Receipt()
        {
            _font = new Font("Arial", 12);
            _pen = new Pen(Color.Black);
            _brush = new SolidBrush(Color.Black);
        }

        public void Dispose()
        {
            _font.Dispose();
            _pen.Dispose();
            _brush.Dispose();
        }

        public void Print(string storeName, string storeAddress, string storePhone, string customerName, string customerAddress, string date, string time, string invoiceNumber, string items, string total, string paymentMethod)
        {
            // Criar um novo documento de impressão
            var printDocument = new PrintDocument();

            // Definir as propriedades do documento de impressão
            printDocument.DocumentName = "Receipt";
            printDocument.PrintPage += PrintPage;

            // Imprimir o documento
            printDocument.Print();

            // Função que imprime a página do recibo
        }

        private void PrintPage(object sender, PrintPageEventArgs e)
        {
            // Desenhar o cabeçalho do recibo
            e.Graphics.DrawString(storeName, _font, _brush, new Point(10, 10));
            e.Graphics.DrawString(storeAddress, _font, _brush, new Point(10, 30));
            e.Graphics.DrawString(storePhone, _font, _brush, new Point(10, 50));

            // Desenhar o corpo do recibo
            e.Graphics.DrawString("Cliente:", _font, _brush, new Point(10, 70));
            e.Graphics.DrawString(customerName, _font, _brush, new Point(10, 90));
            e.Graphics.DrawString(customerAddress, _font, _brush, new Point(10, 110));
            e.Graphics.DrawString("Data:", _font, _brush, new Point(10, 130));
            e.Graphics.DrawString(date, _font, _brush, new Point(10, 150));
            e.Graphics.DrawString("Hora:", _font, _brush, new Point(10, 170));
            e.Graphics.DrawString(time, _font, _brush, new Point(10, 190));
            e.Graphics.DrawString("Número da Fatura:", _font, _brush, new Point(10, 210));
            e.Graphics.DrawString(invoiceNumber, _font, _brush, new Point(10, 230));
            e.Graphics.DrawString("Itens:", _font, _brush, new Point(10, 250));
            e.Graphics.DrawString(items, _font, _brush, new Point(10, 270));

            // Desenhar o rodapé do recibo
            e.Graphics.DrawString("Total:", _font, _brush, new Point(10, 290));
            e.Graphics.DrawString(total, _font, _brush, new Point(10, 310));
            e.Graphics.DrawString("Forma de Pagamento:", _font, _brush, new Point(10, 330));
            e.Graphics.DrawString(paymentMethod, _font, _brush, new Point(10, 350));

            // Draw items table header
            //y += 20;
            //e.Graphics.DrawString("Itens", _font, _brush, new Point(10, y));
            //y += 10;
            //e.Graphics.DrawLine(_pen, new Point(10, y), new Point(e.PageBounds.Width - 10, y)); // Line under header
            //y += 10;
            //// Draw table columns (adjust spacing as needed)
            //e.Graphics.DrawString("Nome", _font, _brush, new Point(10, y));
            //e.Graphics.DrawString("Qtd.", _font, _brush, new Point(e.PageBounds.Width - 80, y));
            //e.Graphics.DrawString("Preço", _font, _brush, new Point(e.PageBounds.Width - 40, y));
            //y += 10;
            //e.Graphics.DrawLine(_pen, new Point(10, y), new Point(e.PageBounds.Width - 10, y)); // Line under columns

            //// Draw each item
            //foreach (var item in items)
            //{
            //    y += 10;
            //    e.Graphics.DrawString(item.Name, _font, _brush, new Point(10, y));
            //    e.Graphics.DrawString(item.Quantity.ToString(),

        }


        }

}

