using appRike.Consultas;
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

namespace appRike.Cadastros
{
    public partial class CadastroComputadores : Form
    {
        private ServicoDeComputador computadorApp;
        private Computador objComputador;
        private int iCodigo;

        public CadastroComputadores()
        {
            InitializeComponent();            
            EstadoInicial();
        }

        public void EstadoInicial()
        {
            iCodigo = 0;
            txtDescricao.Text = "";
            lbMensagem.Visible = false;

            HabilitaCampos(false);
        }

        public void HabilitaCampos(bool bHabilita, bool bCarregado = false)
        {
            lbMensagem.Visible = false;
            txtDescricao.Enabled = bHabilita;
            btnNovo.Enabled = (!bHabilita && !bCarregado);
            btnAlterar.Enabled = (!bHabilita && bCarregado);
            btnGravar.Enabled = bHabilita;
            btnExcluir.Enabled = (!bHabilita && bCarregado);
            btnBuscar.Enabled = !bHabilita;
        }

        public void CarregaComputador()
        {
            computadorApp = new ServicoDeComputador();
            var computador = computadorApp.GetById(iCodigo);

            if (computador != null)
            {
                txtDescricao.Text = computador.Descricao;
                HabilitaCampos(false, true);
            }
            else
            {
                AlterarMsg("Computador não encontrado!", true);
            }

            computadorApp.Dispose();
        }

        private void btnNovo_Click(object sender, EventArgs e)
        {
            iCodigo = 0;
            HabilitaCampos(true);
            txtDescricao.Focus();
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            HabilitaCampos(true);
        }

        private void btnGravar_Click(object sender, EventArgs e)
        {
            computadorApp = new ServicoDeComputador();
            objComputador = new Computador();
            lbMensagem.Visible = false;
            bool result = true;

            if (!String.IsNullOrEmpty(txtDescricao.Text))
            {
                if (iCodigo == 0)
                {
                    objComputador.Descricao = txtDescricao.Text;
                    result = computadorApp.Adicionar(objComputador);
                }
                else
                {
                    objComputador = computadorApp.GetById(iCodigo);
                    objComputador.Descricao = txtDescricao.Text;
                    result = computadorApp.Alterar(objComputador);
                }

                if (result)
                {
                    EstadoInicial();
                    AlterarMsg("Gravado com sucesso!", false);
                }
                else
                    AlterarMsg("Erro ao gravar os dados", true);
            }
            else
                AlterarMsg("Nome é obrigatório!", true);

            computadorApp.Dispose();
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            computadorApp = new ServicoDeComputador();
            objComputador = new Computador();
            lbMensagem.Visible = false;

            if (MessageBox.Show("Deseja excluir esse computador?", "Excluir", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (iCodigo > 0)
                {
                    objComputador = computadorApp.GetById(iCodigo);
                    if (computadorApp.Excluir(objComputador))
                    {
                        EstadoInicial();
                        AlterarMsg("Excluido com sucesso!", false);
                    }
                    else
                    {
                        AlterarMsg("Erro ao excluir os dados!", true);
                    }
                }
                else
                    AlterarMsg("Nenhum aluno para excluir!", true);
            }

            computadorApp.Dispose();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            EstadoInicial();
        }
     
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            lbMensagem.Visible = false;
            var telaConsulta = new ConsultaComputadores();
            telaConsulta.ShowDialog();

            if (telaConsulta.iCodigo > 0)
            {
                iCodigo = telaConsulta.iCodigo;
                CarregaComputador();
            }
        }

        public void AlterarMsg(string texto, bool erro)
        {
            lbMensagem.Visible = true;
            lbMensagem.ForeColor = erro == true ? Color.Red : Color.Green;
            lbMensagem.Text = texto;
        }
    }
}
