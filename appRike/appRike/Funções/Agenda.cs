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
    public partial class Agenda : Form
    {
        private ServicoDeAluno alunoApp;
        private ServicoDeComputador computadorApp;
        private ServicoDeHorario horarioApp;
        private ServicoDeAgenda agendaApp;

        private Dominio.Entidades.Agenda objAgenda;

        public Agenda()
        {
            InitializeComponent();
            EstadoInicial();        
        }

        public void EstadoInicial()
        {
            alunoApp = new ServicoDeAluno();
            computadorApp = new ServicoDeComputador();
            horarioApp = new ServicoDeHorario();
            agendaApp = new ServicoDeAgenda();

            CarregaAlunos();
            CarregaComputadores();
            CarregaHorarios();
        }

        public void CarregaAlunos()
        {
            cbbAluno.DataSource = alunoApp.GetAll();
            cbbAluno.ValueMember = "Id";
            cbbAluno.DisplayMember = "Nome";
            cbbAluno.SelectedIndex = 0;
        }
        public void CarregaComputadores()
        {
            cbbComputador.DataSource = computadorApp.GetAll();
            cbbComputador.ValueMember = "Id";
            cbbComputador.DisplayMember = "Descricao";
            cbbComputador.SelectedIndex = 0;
        }
        public void CarregaHorarios()
        {
            int chave;
            string descricao;
            var list = horarioApp.GetAll();
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
        
        private void btnGravar_Click(object sender, EventArgs e)
        {
            objAgenda = new Dominio.Entidades.Agenda();
            lbMensagem.Visible = false;

            objAgenda.AlunoId = (int)cbbAluno.SelectedValue;
            objAgenda.ComputadorId = (int)cbbComputador.SelectedValue;
            objAgenda.HorarioId = (int)cbbHorario.SelectedValue;

            if (agendaApp.VerificarSePodeAgendar(objAgenda))
            {
                agendaApp.Adicionar(objAgenda);
                lbMensagem.Visible = true;
                lbMensagem.ForeColor = Color.Green;
                lbMensagem.Text = "Horario reservado com sucesso!";
            }
            else
            {
                lbMensagem.Visible = true;
                lbMensagem.ForeColor = Color.Red;
                lbMensagem.Text = "Computador já está agendado nesse horário!";
            }
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


    }
}
