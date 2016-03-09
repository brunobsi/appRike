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
    public partial class FuncaoAgenda : Form
    {
        private ServicoDeAluno alunoApp;
        private ServicoDeComputador computadorApp;
        private ServicoDeHorario horarioApp;
        private ServicoDeAgenda agendaApp;
        public string sErro;

        private Agenda objAgenda;

        public FuncaoAgenda()
        {
            sErro = "";
            InitializeComponent();
            EstadoInicial();
        }

        public void EstadoInicial()
        {           
            CarregaAlunos();
            CarregaComputadores();
            CarregaHorarios();           
        }

        public void CarregaAlunos()
        {
            alunoApp = new ServicoDeAluno();
            var list = alunoApp.GetAll();

            if (list != null && list.Count > 0)
            {
                cbbAluno.DataSource = list;
                cbbAluno.ValueMember = "Id";
                cbbAluno.DisplayMember = "Nome";
                cbbAluno.SelectedIndex = 0;
            }
            else
            {
                sErro += "Cadastre pelo menos um aluno para agendar uma aula!\n";
            }

            alunoApp.Dispose();
        }
        public void CarregaComputadores()
        {
            computadorApp = new ServicoDeComputador();
            var list = computadorApp.GetAll();

            if (list != null && list.Count > 0)
            {
                cbbComputador.DataSource = list;
                cbbComputador.ValueMember = "Id";
                cbbComputador.DisplayMember = "Descricao";
                cbbComputador.SelectedIndex = 0;
            }
            else
            {
                sErro += "Cadastre pelo menos um computador para agendar uma aula!\n";
            }

            computadorApp.Dispose();
        }
        public void CarregaHorarios()
        {
            int chave;
            string descricao;
            horarioApp = new ServicoDeHorario();
            var list = horarioApp.GetAll();
            list = list.OrderBy(x => x.Ordem)
                   .ThenBy(x => x.HoraInicial)
                   .ToList();

            if (list != null && list.Count > 0)
            {
                var item = new KeyValuePair<int, string>();
                var listCombo = new List<KeyValuePair<int, string>>();

                for (int i = 0; i < list.Count; i++)
                {
                    chave = list[i].Id;
                    descricao = list[i].Dia + " das " + list[i].HoraInicial + " às " + list[i].HoraFinal;
                    item = new KeyValuePair<int, string>(chave, descricao);
                    listCombo.Add(item);
                }

                cbbHorario.DataSource = listCombo;
                cbbHorario.ValueMember = "Key";
                cbbHorario.DisplayMember = "Value";
                cbbHorario.SelectedIndex = 0;
            }
            else
            {
                sErro += "Cadastre pelo menos um horário para agendar uma aula!\n";
            }

            horarioApp.Dispose();
        }

        private void btnAgendar_Click(object sender, EventArgs e)
        {
            agendaApp = new ServicoDeAgenda();
            objAgenda = new Agenda();
            lbMensagem.Visible = false;

            objAgenda.AlunoId = (int)cbbAluno.SelectedValue;
            objAgenda.ComputadorId = (int)cbbComputador.SelectedValue;
            objAgenda.HorarioId = (int)cbbHorario.SelectedValue;

            if (agendaApp.VerificarSePodeAgendar(objAgenda))
            {
                agendaApp.Adicionar(objAgenda);
                AlterarMsg("Horario reservado com sucesso!", false);
            }
            else
            {
                AlterarMsg("Computador já está agendado nesse horário!", true);
            }

            agendaApp.Dispose();
        }

        private void cbbAluno_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbMensagem.Visible = false;
        }

        private void cbbComputador_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbMensagem.Visible = false;
        }

        private void cbbHorario_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbMensagem.Visible = false;
        }

        public void AlterarMsg(string texto, bool erro)
        {
            lbMensagem.Visible = true;
            lbMensagem.ForeColor = erro == true ? Color.Red : Color.Green;
            lbMensagem.Text = texto;
        }
    }
}
