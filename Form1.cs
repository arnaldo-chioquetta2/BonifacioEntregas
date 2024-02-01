using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TeleBonifacio;

namespace TeleBonifacio
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            AbrirOuFocarFormulario<operLancamento>();
        }

        private void AbrirOuFocarFormulario<T>() where T : Form, new()
        {
            T formExistente = Application.OpenForms.OfType<T>().FirstOrDefault();
            if (formExistente != null)
            {
                if (formExistente.WindowState == FormWindowState.Minimized)
                {
                    formExistente.WindowState = FormWindowState.Normal;
                }
                formExistente.BringToFront();
                formExistente.Focus();
            }
            else
            {
                T novoForm = new T();
                novoForm.Show();
            }
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            AbrirOuFocarFormulario<fCadEntregadores>();
        }

        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            PictureBox pb = sender as PictureBox;
            if (pb != null)
            {
                pb.BorderStyle = BorderStyle.Fixed3D; // Efeito de pressionado
            }
        }

        private void pictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            PictureBox pb = sender as PictureBox;
            if (pb != null)
            {
                pb.BorderStyle = BorderStyle.FixedSingle; // Volta ao estilo original
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            AbrirOuFocarFormulario<fCadClientes>();
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            AbrirOuFocarFormulario<oprConfig>();
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            AbrirOuFocarFormulario<CadVendedores>();
        }
    }

}
