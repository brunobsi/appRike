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

namespace appRike.Funções
{
    public partial class FuncaoListaAgenda : Form
    {
        private ServicoDeAgenda agendaApp;
        private List<Agenda> listAll;
        private DataTable dt;

        public FuncaoListaAgenda()
        {
            InitializeComponent();
            EstadoInicial();
        }

        public void CarregaTipo()
        {
            var list = new List<KeyValuePair<int, string>>();
            list.Add(new KeyValuePair<int, string>(1, "Aluno"));
            list.Add(new KeyValuePair<int, string>(2, "Computador"));
            list.Add(new KeyValuePair<int, string>(3, "Horario"));

            cbbTipo.DataSource = list;
            cbbTipo.ValueMember = "Key";
            cbbTipo.DisplayMember = "Value";
            cbbTipo.SelectedIndex = 0;
        }

        public void EstadoInicial()
        {
            dt = new DataTable();
            listAll = null;
            dt.Columns.Add("Id");
            dt.Columns.Add("Aluno");
            dt.Columns.Add("Horario");
            dt.Columns.Add("Computador");
            dt.Columns.Add("Acao");

            CarregaAgenda();
            CarregaTipo();
        }

        public void CarregaAgenda()
        {
            agendaApp = new ServicoDeAgenda();
            listAll = agendaApp.GetAll("Horario", "Computador", "Aluno").ToList();
            MontarDataTable();
            agendaApp.Dispose();
        }

        public void MontarDataTable()
        {
            string horario;
            dt.Clear();

            listAll = listAll.OrderBy(x => x.Horario.Ordem)
                .ThenBy(x => x.Horario.HoraInicial)
                .ThenBy(x => x.Computador.Descricao).ToList();

            foreach (var item in listAll)
            {
                horario = item.Horario.Dia + " das " + item.Horario.HoraInicial + " às " + item.Horario.HoraFinal;
                dt.Rows.Add(item.Id, item.Aluno.Nome, horario, item.Computador.Descricao, "Excluir");
            }

            dgvAgenda.DataSource = dt;
        }

        public void PesquisaAgenda(int tipo, string nome)
        {
            agendaApp = new ServicoDeAgenda();
            listAll = null;

            switch (tipo)
            {
                case 1: listAll = agendaApp.Get(x => x.Aluno.Nome.ToUpper().Contains(nome.ToUpper()), "Horario", "Computador", "Aluno").ToList(); break;
                case 2: listAll = agendaApp.Get(x => x.Computador.Descricao.ToUpper().Contains(nome.ToUpper()), "Horario", "Computador", "Aluno").ToList(); break;
                case 3: listAll = agendaApp.Get(x => x.Horario.Dia.ToUpper().Contains(nome.ToUpper()) ||
                                  x.Horario.HoraInicial.ToUpper().Contains(nome.ToUpper()) ||
                                  x.Horario.HoraFinal.ToUpper().Contains(nome.ToUpper()),
                                  "Horario", "Computador", "Aluno").ToList(); break;
            }

            MontarDataTable();
            agendaApp.Dispose();
        }

        public void ExcluirAgendamento(int iCodigo)
        {
            agendaApp = new ServicoDeAgenda();
            var agenda = agendaApp.GetById(iCodigo);
            agendaApp.Excluir(agenda);
            agendaApp.Dispose();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            PesquisaAgenda((int)cbbTipo.SelectedValue, txtDescricao.Text);
        }

        private void txtDescricao_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                PesquisaAgenda((int)cbbTipo.SelectedValue, txtDescricao.Text);
            }
        }

        private void btnAgendar_Click(object sender, EventArgs e)
        {
            var tela = new FuncaoAgenda();

            if (!String.IsNullOrEmpty(tela.sErro))
                MessageBox.Show(tela.sErro, "Problemas", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
            {
                tela.ShowDialog();
                EstadoInicial();
            }
        }

        private void dgvAgenda_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvAgenda.CurrentCell != null)
            {
              if(dgvAgenda.CurrentCell.ColumnIndex == 4)
              {
                  ExcluirAgendamento(Convert.ToInt32(dgvAgenda.CurrentRow.Cells["Id"].Value));
                  EstadoInicial(); 
              }
            }
        }

     

  

     

    }
}

