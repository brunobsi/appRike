using Dominio.Entidades;
using Infra.Servicos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace appRike.Consultas
{
    public partial class ConsultaComputadores : Form
    {
        public int iCodigo = 0;
        private ServicoDeComputador computadorApp;
        private List<Computador> listAll;

        public ConsultaComputadores()
        {
            InitializeComponent();
            EstadoInicial();
        }

        public void EstadoInicial()
        {
            listAll = null;
            CarregaComputadores();
        }

        public void CarregaComputadores()
        {
            computadorApp = new ServicoDeComputador();
            listAll = computadorApp.GetAll().OrderBy(x => x.Descricao).ToList();
            dgvComputadores.DataSource = listAll;
            computadorApp.Dispose();
        }

        public void PesquisaComputador(string descricao)
        {
            computadorApp = new ServicoDeComputador();
            listAll = computadorApp.Get(x => x.Descricao.ToUpper().Contains(descricao.ToUpper())).OrderBy(x => x.Descricao).ToList();
            dgvComputadores.DataSource = listAll;
            computadorApp.Dispose();
        }

        private void txtDescricao_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                PesquisaComputador(txtDescricao.Text);
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            PesquisaComputador(txtDescricao.Text);
        }

        private void dgvComputadores_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvComputadores.CurrentRow != null)
            {
                iCodigo = Convert.ToInt32(dgvComputadores.CurrentRow.Cells["Id"].Value);
                Close();
            }
        }

    }
}
