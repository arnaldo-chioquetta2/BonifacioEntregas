using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Printing;
using TeleBonifacio.gen;

namespace TeleBonifacio.rel
{
    public class Receipt : IDisposable
    {
        private string storeName = "";
        private string storeAddress = "";
        private string storePhone = "";
        private string customerName = "";
        private DateTime DataIni;

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

        public void Print(string storeName, string storeAddress, string storePhone, string customerName, string total, DateTime DataIni)
        {
            this.storeName = storeName;
            this.storeAddress = storeAddress;
            this.storePhone = storePhone;
            this.customerName = customerName;
            this.total = total;
            this.DataIni = DataIni;
            var printDocument = new PrintDocument();
            printDocument.DocumentName = "Receipt";
            printDocument.PrintPage += PrintPage;
            printDocument.Print();
        }

        private void PrintPage(object sender, PrintPageEventArgs e)
        {
            int startX = 10;
            int startY = 10;
            int offset = 40;
            string dataPagamento;
            if (this.DataIni.Date == DateTime.Now.Date)
            {
                dataPagamento = "do dia " + DateTime.Now.ToString("dd/MM/yyyy");
            }
            else
            {
                dataPagamento = "de " + this.DataIni.ToString("dd/MM/yyyy") + " a " + DateTime.Now.ToString("dd/MM/yyyy");
            }
            ValorPorExtenso valorPorExtenso = new ValorPorExtenso();
            string valorPorExtensoString = valorPorExtenso.EscreverPorExtenso(this.total);
            e.Graphics.DrawString("Recibo", new Font(_font.FontFamily, 18, FontStyle.Bold), _brush, startX, startY);
            startX = +100;
            e.Graphics.DrawString(storeName, _font, _brush, startX, startY + offset);            
            e.Graphics.DrawString("Telefone: " + storePhone, _font, _brush, startX, startY + offset * 2);
            e.Graphics.DrawString("Endereço: " + storeAddress, _font, _brush, startX, startY + offset * 3);
            e.Graphics.DrawLine(_pen, new Point(startX, startY + offset * 4), new Point(e.PageBounds.Width - startX, startY + offset * 4));
            e.Graphics.DrawString("Nome: "+ customerName, _font, _brush, startX, startY + offset * 5);           
            e.Graphics.DrawString("Valor: "+ total, _font, _brush, startX, startY + offset * 6);
            e.Graphics.DrawString(valorPorExtensoString, _font, _brush, startX, startY + offset * 7);
            e.Graphics.DrawString("Concordo com o valor acima citado", _font, _brush, startX, startY + offset * 8);
            e.Graphics.DrawString("Pagamento das vendas " + dataPagamento, _font, _brush, startX, startY + offset * 9);
            e.Graphics.DrawString("Ass:", _font, _brush, startX, startY + offset * 11);
            int MgeY = (startY + offset * 11) + 20;
            e.Graphics.DrawLine(_pen, new Point((startX+50), MgeY), new Point(e.PageBounds.Width - (startX + 50), MgeY));
            e.Graphics.DrawString("Porto Alegre, " + DateTime.Now.ToString("dd/MM/yyyy"), _font, _brush, startX, startY + offset * 12);
        }

    }

}

