﻿using appRike.Cadastros;
using appRike.Funções;
using appRike.IoC;
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

namespace appRike
{
    public partial class TelaInicial : Form
    {
        private ModuloNinject ninject = new ModuloNinject();

        public TelaInicial()
        {           
            InitializeComponent();
        }

        private void alunosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var tela = new CadastroAlunos();
            tela.ShowDialog();
        }

        private void computadoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var tela = new CadastroComputadores();
            tela.ShowDialog();
        }

        private void horariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var tela = new CadastroHorarios();
            tela.ShowDialog();
        }

        private void agendaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var tela = new FuncaoListaAgenda(ninject.InstanciaServicoAgenda());
            tela.ShowDialog();
        }
    }
}
