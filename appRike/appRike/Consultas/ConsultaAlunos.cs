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
    public partial class ConsultaAlunos : Form
    {
        public int iCodigo = 0;
        private ServicoDeAluno alunoApp;
        private List<Aluno> listAll;

        public ConsultaAlunos()
        {
            InitializeComponent();
            EstadoInicial();
        }

        public void EstadoInicial()
        {
            listAll = null;
            CarregaAlunos();
        }

        public void CarregaAlunos()
        {
            alunoApp = new ServicoDeAluno();
            listAll = alunoApp.GetAll().OrderBy(x => x.Nome).ToList();
            dgvAlunos.DataSource = listAll;
            alunoApp.Dispose();
        }

        public void PesquisaAluno(string nome)
        {
            alunoApp = new ServicoDeAluno();
            listAll = alunoApp.Get(x => x.Nome.ToUpper().Contains(nome.ToUpper())).OrderBy(x => x.Nome).ToList();
            dgvAlunos.DataSource = listAll;
            alunoApp.Dispose();
        }


        private void txtDescricao_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                PesquisaAluno(txtDescricao.Text);
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            PesquisaAluno(txtDescricao.Text);
        }

        private void dgvAlunos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvAlunos.CurrentRow != null)
            {
                iCodigo = Convert.ToInt32(dgvAlunos.CurrentRow.Cells["Id"].Value);
                Close();
            }
        }

    }
}
