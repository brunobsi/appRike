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
    public partial class Lista : Form
    {
        private ServicoDeAgenda agendaApp;
        private List<Dominio.Entidades.Agenda> listAll;
        private DataTable dt;

        public Lista()
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
            agendaApp = new ServicoDeAgenda();
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
            listAll = agendaApp.GetAll("Horario", "Computador", "Aluno").OrderBy(x => x.Aluno.Nome).ToList();
            MontarDataTable();
        }

        public void MontarDataTable()
        {
            string horario;
            dt.Clear();

            foreach (var item in listAll)
            {
                horario = item.Horario.Dia + " das " + item.Horario.HoraInicial + " às " + item.Horario.HoraFinal;
                dt.Rows.Add(item.Id, item.Aluno.Nome, horario, item.Computador.Descricao, "Excluir");
            }

            dgvAgenda.DataSource = dt;
        }

        public void PesquisaAgenda(int tipo, string nome)
        {
            listAll = null;

            switch (tipo)
            {
                case 1: listAll = agendaApp.Get(x => x.Aluno.Nome.ToUpper().Contains(nome.ToUpper()), "Horario", "Computador", "Aluno").OrderBy(x => x.Aluno.Nome).ToList(); break;
                case 2: listAll = agendaApp.Get(x => x.Computador.Descricao.ToUpper().Contains(nome.ToUpper()), "Horario", "Computador", "Aluno").OrderBy(x => x.Aluno.Nome).ToList(); ; break;
                case 3: listAll = agendaApp.Get(x => x.Horario.Dia.ToUpper().Contains(nome.ToUpper()) ||
                                  x.Horario.HoraInicial.ToUpper().Contains(nome.ToUpper()) ||
                                  x.Horario.HoraFinal.ToUpper().Contains(nome.ToUpper()),
                                  "Horario", "Computador", "Aluno").OrderBy(x => x.Aluno.Nome).ToList(); break;
            }

            MontarDataTable();
        }

        private void txtDescricao_Enter(object sender, EventArgs e)
        {
            PesquisaAgenda((int)cbbTipo.SelectedValue, txtDescricao.Text);
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

        private void btnNovo_Click(object sender, EventArgs e)
        {
            var tela = new Agenda();
            tela.ShowDialog();
            EstadoInicial();
        }

    }
}

