using Dominio.Entidades;
using Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace appRike.Funções
{
    public partial class FuncaoListaAgenda : Form
    {
        private readonly IServicoDeAgenda _agendaApp;
        private List<Agenda> _listAll;
        private DataTable _dt;

        public FuncaoListaAgenda(IServicoDeAgenda agendaApp)
        {
            this._agendaApp = agendaApp;
            InitializeComponent();
            EstadoInicial();
        }

        public void CarregaTipo()
        {
            var list = new List<KeyValuePair<int, string>>
            {
                new KeyValuePair<int, string>(1, "Aluno"),
                new KeyValuePair<int, string>(2, "Computador"),
                new KeyValuePair<int, string>(3, "Horario")
            };

            cbbTipo.DataSource = list;
            cbbTipo.ValueMember = "Key";
            cbbTipo.DisplayMember = "Value";
            cbbTipo.SelectedIndex = 0;
        }

        public void EstadoInicial()
        {
            _dt = new DataTable();
            _listAll = null;
            _dt.Columns.Add("Id");
            _dt.Columns.Add("Aluno");
            _dt.Columns.Add("Horario");
            _dt.Columns.Add("Computador");
            _dt.Columns.Add("Acao");

            CarregaAgenda();
            CarregaTipo();
        }

        public void CarregaAgenda()
        {
            _listAll = _agendaApp.GetAll("Horario", "Computador", "Aluno").ToList();
            MontarDataTable();
            _agendaApp.Dispose();
        }

        public void MontarDataTable()
        {
            _dt.Clear();

            _listAll = _listAll.OrderBy(x => x.Horario.Ordem).ThenBy(x => x.Horario.HoraInicial).ThenBy(x => x.Computador.Descricao).ToList();

            foreach (var item in _listAll)
            {
                var horario = item.Horario.Dia + " das " + item.Horario.HoraInicial + " às " + item.Horario.HoraFinal;
                _dt.Rows.Add(item.Id, item.Aluno.Nome, horario, item.Computador.Descricao, "Excluir");
            }

            dgvAgenda.DataSource = _dt;
        }

        public void PesquisaAgenda(int tipo, string nome)
        {
            //agendaApp = new ServicoDeAgenda();
            _listAll = null;

            switch (tipo)
            {
                case 1: _listAll = _agendaApp.Get(x => x.Aluno.Nome.ToUpper().Contains(nome.ToUpper()), "Horario", "Computador", "Aluno").ToList(); break;
                case 2: _listAll = _agendaApp.Get(x => x.Computador.Descricao.ToUpper().Contains(nome.ToUpper()), "Horario", "Computador", "Aluno").ToList(); break;
                case 3: _listAll = _agendaApp.Get(x => x.Horario.Dia.ToUpper().Contains(nome.ToUpper()) ||
                                  x.Horario.HoraInicial.ToUpper().Contains(nome.ToUpper()) ||
                                  x.Horario.HoraFinal.ToUpper().Contains(nome.ToUpper()),
                                  "Horario", "Computador", "Aluno").ToList(); break;
            }

            MontarDataTable();
            _agendaApp.Dispose();
        }

        public void ExcluirAgendamento(int iCodigo)
        {
            var agenda = _agendaApp.GetById(iCodigo);
            _agendaApp.Excluir(agenda);
            _agendaApp.Dispose();
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
                MessageBox.Show(tela.sErro, @"Problemas", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
            {
                tela.ShowDialog();
                EstadoInicial();
            }
        }

        private void dgvAgenda_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvAgenda.CurrentCell == null) return;
            if (dgvAgenda.CurrentCell.ColumnIndex != 4) return;
            if (dgvAgenda.CurrentRow != null)
                ExcluirAgendamento(Convert.ToInt32(dgvAgenda.CurrentRow.Cells["Id"].Value));
            EstadoInicial();
        }

     

  

     

    }
}

