using System;
using System.Drawing;
using System.Windows.Forms;

namespace TeleBonifacio
{
    partial class OperPagar
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OperPagar));
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btPDF = new System.Windows.Forms.Button();
            this.ckPago = new System.Windows.Forms.CheckBox();
            this.chPermanente = new System.Windows.Forms.CheckBox();
            this.btFiltrar = new System.Windows.Forms.Button();
            this.dtpDataPagamento = new System.Windows.Forms.DateTimePicker();
            this.dtpDataVencimento = new System.Windows.Forms.DateTimePicker();
            this.dtpDataEmissao = new System.Windows.Forms.DateTimePicker();
            this.btnAdicionar = new System.Windows.Forms.Button();
            this.btExcluir = new System.Windows.Forms.Button();
            this.btLimparFiltro = new System.Windows.Forms.Button();
            this.labelFornecedor = new System.Windows.Forms.Label();
            this.cmbForn = new System.Windows.Forms.ComboBox();
            this.labelDataEmissao = new System.Windows.Forms.Label();
            this.labelDataVencimento = new System.Windows.Forms.Label();
            this.labelValorTotal = new System.Windows.Forms.Label();
            this.txValorTotal = new System.Windows.Forms.TextBox();
            this.labelChaveNotaFiscal = new System.Windows.Forms.Label();
            this.txChaveNotaFiscal = new System.Windows.Forms.TextBox();
            this.labelDescricao = new System.Windows.Forms.Label();
            this.txDescricao = new System.Windows.Forms.TextBox();
            this.labelDataPagamento = new System.Windows.Forms.Label();
            this.labelObservacoes = new System.Windows.Forms.Label();
            this.txObservacoes = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.tbContas = new System.Windows.Forms.TabControl();
            this.tbTempos = new System.Windows.Forms.TabPage();
            this.dataGrid1 = new System.Windows.Forms.DataGridView();
            this.tbPerms = new System.Windows.Forms.TabPage();
            this.dataGrid2 = new System.Windows.Forms.DataGridView();
            this.txNvForn = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.groupBox3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tbContas.SuspendLayout();
            this.tbTempos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).BeginInit();
            this.tbPerms.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid2)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btPDF);
            this.groupBox3.Controls.Add(this.ckPago);
            this.groupBox3.Controls.Add(this.chPermanente);
            this.groupBox3.Controls.Add(this.btFiltrar);
            this.groupBox3.Controls.Add(this.dtpDataPagamento);
            this.groupBox3.Controls.Add(this.dtpDataVencimento);
            this.groupBox3.Controls.Add(this.dtpDataEmissao);
            this.groupBox3.Controls.Add(this.btnAdicionar);
            this.groupBox3.Controls.Add(this.btExcluir);
            this.groupBox3.Controls.Add(this.btLimparFiltro);
            this.groupBox3.Controls.Add(this.labelFornecedor);
            this.groupBox3.Controls.Add(this.cmbForn);
            this.groupBox3.Controls.Add(this.labelDataEmissao);
            this.groupBox3.Controls.Add(this.labelDataVencimento);
            this.groupBox3.Controls.Add(this.labelValorTotal);
            this.groupBox3.Controls.Add(this.txValorTotal);
            this.groupBox3.Controls.Add(this.labelChaveNotaFiscal);
            this.groupBox3.Controls.Add(this.txChaveNotaFiscal);
            this.groupBox3.Controls.Add(this.labelDescricao);
            this.groupBox3.Controls.Add(this.txDescricao);
            this.groupBox3.Controls.Add(this.labelDataPagamento);
            this.groupBox3.Controls.Add(this.labelObservacoes);
            this.groupBox3.Controls.Add(this.txObservacoes);
            this.groupBox3.Controls.Add(this.txNvForn);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(720, 166);
            this.groupBox3.TabIndex = 41;
            this.groupBox3.TabStop = false;
            // 
            // btPDF
            // 
            this.btPDF.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btPDF.Enabled = false;
            this.btPDF.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.btPDF.Location = new System.Drawing.Point(454, 138);
            this.btPDF.Name = "btPDF";
            this.btPDF.Size = new System.Drawing.Size(75, 23);
            this.btPDF.TabIndex = 49;
            this.btPDF.Text = "PDF";
            this.btPDF.UseVisualStyleBackColor = true;
            this.btPDF.Click += new System.EventHandler(this.btPDF_Click);
            // 
            // ckPago
            // 
            this.ckPago.AutoSize = true;
            this.ckPago.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ckPago.Location = new System.Drawing.Point(596, 82);
            this.ckPago.Name = "ckPago";
            this.ckPago.Size = new System.Drawing.Size(55, 19);
            this.ckPago.TabIndex = 48;
            this.ckPago.Text = "Pago";
            this.ckPago.UseVisualStyleBackColor = true;
            this.ckPago.CheckedChanged += new System.EventHandler(this.ckPago_CheckedChanged);
            // 
            // chPermanente
            // 
            this.chPermanente.AutoSize = true;
            this.chPermanente.Enabled = false;
            this.chPermanente.Location = new System.Drawing.Point(535, 143);
            this.chPermanente.Name = "chPermanente";
            this.chPermanente.Size = new System.Drawing.Size(83, 17);
            this.chPermanente.TabIndex = 15;
            this.chPermanente.Text = "Permanente";
            this.chPermanente.UseVisualStyleBackColor = true;
            this.chPermanente.CheckedChanged += new System.EventHandler(this.chPermanente_CheckedChanged);
            // 
            // btFiltrar
            // 
            this.btFiltrar.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btFiltrar.Enabled = false;
            this.btFiltrar.Location = new System.Drawing.Point(211, 139);
            this.btFiltrar.Name = "btFiltrar";
            this.btFiltrar.Size = new System.Drawing.Size(75, 23);
            this.btFiltrar.TabIndex = 10;
            this.btFiltrar.Text = "Filtrar";
            this.btFiltrar.UseVisualStyleBackColor = true;
            this.btFiltrar.Click += new System.EventHandler(this.btFiltrar_Click);
            // 
            // dtpDataPagamento
            // 
            this.dtpDataPagamento.Enabled = false;
            this.dtpDataPagamento.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.dtpDataPagamento.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDataPagamento.Location = new System.Drawing.Point(500, 76);
            this.dtpDataPagamento.Name = "dtpDataPagamento";
            this.dtpDataPagamento.Size = new System.Drawing.Size(90, 23);
            this.dtpDataPagamento.TabIndex = 6;
            this.dtpDataPagamento.Tag = "H";
            this.dtpDataPagamento.ValueChanged += new System.EventHandler(this.dtpDataPagamento_ValueChanged);
            // 
            // dtpDataVencimento
            // 
            this.dtpDataVencimento.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.dtpDataVencimento.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDataVencimento.Location = new System.Drawing.Point(500, 47);
            this.dtpDataVencimento.Name = "dtpDataVencimento";
            this.dtpDataVencimento.Size = new System.Drawing.Size(90, 23);
            this.dtpDataVencimento.TabIndex = 5;
            this.dtpDataVencimento.Tag = "H";
            this.dtpDataVencimento.ValueChanged += new System.EventHandler(this.dtpDataVencimento_ValueChanged);
            // 
            // dtpDataEmissao
            // 
            this.dtpDataEmissao.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.dtpDataEmissao.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDataEmissao.Location = new System.Drawing.Point(500, 15);
            this.dtpDataEmissao.Name = "dtpDataEmissao";
            this.dtpDataEmissao.Size = new System.Drawing.Size(90, 23);
            this.dtpDataEmissao.TabIndex = 4;
            this.dtpDataEmissao.Tag = "H";
            this.dtpDataEmissao.ValueChanged += new System.EventHandler(this.dtpDataEmissao_ValueChanged);
            // 
            // btnAdicionar
            // 
            this.btnAdicionar.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnAdicionar.Enabled = false;
            this.btnAdicionar.Location = new System.Drawing.Point(130, 139);
            this.btnAdicionar.Name = "btnAdicionar";
            this.btnAdicionar.Size = new System.Drawing.Size(75, 23);
            this.btnAdicionar.TabIndex = 9;
            this.btnAdicionar.Text = "Adicionar";
            this.btnAdicionar.UseVisualStyleBackColor = true;
            this.btnAdicionar.Click += new System.EventHandler(this.btnAdicionar_Click_1);
            // 
            // btExcluir
            // 
            this.btExcluir.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btExcluir.Enabled = false;
            this.btExcluir.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.btExcluir.Location = new System.Drawing.Point(373, 139);
            this.btExcluir.Name = "btExcluir";
            this.btExcluir.Size = new System.Drawing.Size(75, 23);
            this.btExcluir.TabIndex = 12;
            this.btExcluir.Text = "Excluir";
            this.btExcluir.UseVisualStyleBackColor = true;
            this.btExcluir.Click += new System.EventHandler(this.btnExcluir_Click);
            // 
            // btLimparFiltro
            // 
            this.btLimparFiltro.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btLimparFiltro.Enabled = false;
            this.btLimparFiltro.Location = new System.Drawing.Point(292, 139);
            this.btLimparFiltro.Name = "btLimparFiltro";
            this.btLimparFiltro.Size = new System.Drawing.Size(75, 23);
            this.btLimparFiltro.TabIndex = 11;
            this.btLimparFiltro.Text = "Limpar";
            this.btLimparFiltro.UseVisualStyleBackColor = true;
            this.btLimparFiltro.Click += new System.EventHandler(this.btLimparFiltro_Click);
            // 
            // labelFornecedor
            // 
            this.labelFornecedor.AutoSize = true;
            this.labelFornecedor.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.labelFornecedor.Location = new System.Drawing.Point(87, 19);
            this.labelFornecedor.Name = "labelFornecedor";
            this.labelFornecedor.Size = new System.Drawing.Size(78, 16);
            this.labelFornecedor.TabIndex = 47;
            this.labelFornecedor.Text = "Fornecedor";
            // 
            // cmbForn
            // 
            this.cmbForn.DisplayMember = "Nome";
            this.cmbForn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbForn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.cmbForn.Location = new System.Drawing.Point(174, 19);
            this.cmbForn.Name = "cmbForn";
            this.cmbForn.Size = new System.Drawing.Size(204, 23);
            this.cmbForn.TabIndex = 0;
            this.cmbForn.ValueMember = "Id";
            this.cmbForn.SelectedIndexChanged += new System.EventHandler(this.cmbForn_SelectedIndexChanged);
            // 
            // labelDataEmissao
            // 
            this.labelDataEmissao.AutoSize = true;
            this.labelDataEmissao.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.labelDataEmissao.Location = new System.Drawing.Point(401, 22);
            this.labelDataEmissao.Name = "labelDataEmissao";
            this.labelDataEmissao.Size = new System.Drawing.Size(93, 16);
            this.labelDataEmissao.TabIndex = 47;
            this.labelDataEmissao.Text = "Data Emissão";
            // 
            // labelDataVencimento
            // 
            this.labelDataVencimento.AutoSize = true;
            this.labelDataVencimento.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.labelDataVencimento.Location = new System.Drawing.Point(384, 51);
            this.labelDataVencimento.Name = "labelDataVencimento";
            this.labelDataVencimento.Size = new System.Drawing.Size(111, 16);
            this.labelDataVencimento.TabIndex = 47;
            this.labelDataVencimento.Text = "Data Vencimento";
            // 
            // labelValorTotal
            // 
            this.labelValorTotal.AutoSize = true;
            this.labelValorTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.labelValorTotal.Location = new System.Drawing.Point(420, 106);
            this.labelValorTotal.Name = "labelValorTotal";
            this.labelValorTotal.Size = new System.Drawing.Size(74, 16);
            this.labelValorTotal.TabIndex = 47;
            this.labelValorTotal.Text = "Valor Total";
            // 
            // txValorTotal
            // 
            this.txValorTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txValorTotal.Location = new System.Drawing.Point(500, 106);
            this.txValorTotal.Name = "txValorTotal";
            this.txValorTotal.Size = new System.Drawing.Size(90, 21);
            this.txValorTotal.TabIndex = 8;
            this.txValorTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txValorTotal.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txValorTotal_KeyUp);
            // 
            // labelChaveNotaFiscal
            // 
            this.labelChaveNotaFiscal.AutoSize = true;
            this.labelChaveNotaFiscal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.labelChaveNotaFiscal.Location = new System.Drawing.Point(54, 51);
            this.labelChaveNotaFiscal.Name = "labelChaveNotaFiscal";
            this.labelChaveNotaFiscal.Size = new System.Drawing.Size(118, 16);
            this.labelChaveNotaFiscal.TabIndex = 47;
            this.labelChaveNotaFiscal.Text = "Chave Nota Fiscal";
            // 
            // txChaveNotaFiscal
            // 
            this.txChaveNotaFiscal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txChaveNotaFiscal.Location = new System.Drawing.Point(174, 49);
            this.txChaveNotaFiscal.Name = "txChaveNotaFiscal";
            this.txChaveNotaFiscal.Size = new System.Drawing.Size(204, 21);
            this.txChaveNotaFiscal.TabIndex = 1;
            this.txChaveNotaFiscal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txChaveNotaFiscal.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txChaveNotaFiscal_KeyUp);
            // 
            // labelDescricao
            // 
            this.labelDescricao.AutoSize = true;
            this.labelDescricao.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.labelDescricao.Location = new System.Drawing.Point(104, 81);
            this.labelDescricao.Name = "labelDescricao";
            this.labelDescricao.Size = new System.Drawing.Size(70, 16);
            this.labelDescricao.TabIndex = 47;
            this.labelDescricao.Text = "Descrição";
            // 
            // txDescricao
            // 
            this.txDescricao.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txDescricao.Location = new System.Drawing.Point(174, 79);
            this.txDescricao.Name = "txDescricao";
            this.txDescricao.Size = new System.Drawing.Size(204, 21);
            this.txDescricao.TabIndex = 2;
            this.txDescricao.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txDescricao.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txDescricao_KeyUp);
            // 
            // labelDataPagamento
            // 
            this.labelDataPagamento.AutoSize = true;
            this.labelDataPagamento.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.labelDataPagamento.Location = new System.Drawing.Point(384, 81);
            this.labelDataPagamento.Name = "labelDataPagamento";
            this.labelDataPagamento.Size = new System.Drawing.Size(110, 16);
            this.labelDataPagamento.TabIndex = 47;
            this.labelDataPagamento.Text = "Data Pagamento";
            // 
            // labelObservacoes
            // 
            this.labelObservacoes.AutoSize = true;
            this.labelObservacoes.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.labelObservacoes.Location = new System.Drawing.Point(84, 108);
            this.labelObservacoes.Name = "labelObservacoes";
            this.labelObservacoes.Size = new System.Drawing.Size(90, 16);
            this.labelObservacoes.TabIndex = 47;
            this.labelObservacoes.Text = "Observações";
            // 
            // txObservacoes
            // 
            this.txObservacoes.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txObservacoes.Location = new System.Drawing.Point(174, 106);
            this.txObservacoes.Name = "txObservacoes";
            this.txObservacoes.Size = new System.Drawing.Size(204, 21);
            this.txObservacoes.TabIndex = 3;
            this.txObservacoes.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txObservacoes.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txObservacoes_KeyUp);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(720, 181);
            this.panel1.TabIndex = 13;
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog1";
            // 
            // tbContas
            // 
            this.tbContas.Controls.Add(this.tbTempos);
            this.tbContas.Controls.Add(this.tbPerms);
            this.tbContas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbContas.Location = new System.Drawing.Point(0, 181);
            this.tbContas.Name = "tbContas";
            this.tbContas.SelectedIndex = 0;
            this.tbContas.Size = new System.Drawing.Size(720, 251);
            this.tbContas.TabIndex = 14;
            this.tbContas.SelectedIndexChanged += new System.EventHandler(this.tbContas_SelectedIndexChanged);
            // 
            // tbTempos
            // 
            this.tbTempos.Controls.Add(this.dataGrid1);
            this.tbTempos.Location = new System.Drawing.Point(4, 22);
            this.tbTempos.Name = "tbTempos";
            this.tbTempos.Padding = new System.Windows.Forms.Padding(3);
            this.tbTempos.Size = new System.Drawing.Size(712, 225);
            this.tbTempos.TabIndex = 0;
            this.tbTempos.Text = "Temporários";
            this.tbTempos.UseVisualStyleBackColor = true;
            // 
            // dataGrid1
            // 
            this.dataGrid1.AllowUserToAddRows = false;
            this.dataGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGrid1.Location = new System.Drawing.Point(3, 3);
            this.dataGrid1.Name = "dataGrid1";
            this.dataGrid1.ReadOnly = true;
            this.dataGrid1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGrid1.Size = new System.Drawing.Size(706, 219);
            this.dataGrid1.TabIndex = 2;
            this.dataGrid1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGrid1_CellClick);
            // 
            // tbPerms
            // 
            this.tbPerms.Controls.Add(this.dataGrid2);
            this.tbPerms.Location = new System.Drawing.Point(4, 22);
            this.tbPerms.Name = "tbPerms";
            this.tbPerms.Padding = new System.Windows.Forms.Padding(3);
            this.tbPerms.Size = new System.Drawing.Size(712, 225);
            this.tbPerms.TabIndex = 1;
            this.tbPerms.Text = "Permanentes";
            this.tbPerms.UseVisualStyleBackColor = true;
            // 
            // dataGrid2
            // 
            this.dataGrid2.AllowUserToAddRows = false;
            this.dataGrid2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGrid2.Location = new System.Drawing.Point(3, 3);
            this.dataGrid2.Name = "dataGrid2";
            this.dataGrid2.ReadOnly = true;
            this.dataGrid2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGrid2.Size = new System.Drawing.Size(706, 219);
            this.dataGrid2.TabIndex = 17;
            this.dataGrid2.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGrid2_CellClick);
            // 
            // txNvForn
            // 
            this.txNvForn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txNvForn.Location = new System.Drawing.Point(174, 19);
            this.txNvForn.Name = "txNvForn";
            this.txNvForn.Size = new System.Drawing.Size(204, 21);
            this.txNvForn.TabIndex = 51;
            this.txNvForn.Visible = false;
            // 
            // timer1
            // 
            this.timer1.Interval = 2000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // OperPagar
            // 
            this.ClientSize = new System.Drawing.Size(720, 432);
            this.Controls.Add(this.tbContas);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "OperPagar";
            this.Text = "Contas a Pagar";
            this.Load += new System.EventHandler(this.OperPagar_Load);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.tbContas.ResumeLayout(false);
            this.tbTempos.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).EndInit();
            this.tbPerms.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox groupBox3;
        private Button btFiltrar;
        private DateTimePicker dtpDataPagamento;
        private DateTimePicker dtpDataVencimento;
        private DateTimePicker dtpDataEmissao;
        private Button btnAdicionar;
        private Button btExcluir;
        private Button btLimparFiltro;
        private Label labelFornecedor;
        private ComboBox cmbForn;
        private Label labelDataEmissao;
        private Label labelDataVencimento;
        private Label labelValorTotal;
        private TextBox txValorTotal;
        private Label labelChaveNotaFiscal;
        private TextBox txChaveNotaFiscal;
        private Label labelDescricao;
        private TextBox txDescricao;
        private Label labelDataPagamento;
        private Label labelObservacoes;
        private TextBox txObservacoes;
        private Panel panel1;
        private CheckBox ckPago;
        private CheckBox chPermanente;
        private OpenFileDialog openFileDialog;
        private Button btPDF;
        private TabControl tbContas;
        private TabPage tbTempos;
        private TabPage tbPerms;
        private DataGridView dataGrid1;
        private DataGridView dataGrid2;
        private TextBox txNvForn;
        private Timer timer1;
    }
}