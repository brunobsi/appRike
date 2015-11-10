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
    public partial class CadastroAlunos : Form
    {
        private ServicoDeAluno alunoApp;
        private Aluno objAluno;
        private int iCodigo;

        public CadastroAlunos()
        {
            InitializeComponent();           
            EstadoInicial();
        }

        public void EstadoInicial()
        {
            iCodigo = 0;
            txtNome.Text = "";
            lbMensagem.Visible = false;

            HabilitaCampos(false);
        }

        public void HabilitaCampos(bool bHabilita, bool bCarregado = false)
        {
            lbMensagem.Visible = false;
            txtNome.Enabled = bHabilita;
            btnNovo.Enabled = (!bHabilita && !bCarregado);
            btnAlterar.Enabled = (!bHabilita && bCarregado);
            btnGravar.Enabled = bHabilita;
            btnExcluir.Enabled = (!bHabilita && bCarregado);
            btnBuscar.Enabled = !bHabilita;
        }

        public void CarregaAluno()
        {
            alunoApp = new ServicoDeAluno();
            var aluno = alunoApp.GetById(iCodigo);

            if (aluno != null)
            {
                txtNome.Text = aluno.Nome;
                HabilitaCampos(false, true);
            }
            else
            {
                AlterarMsg("Aluno não encontrado!", true);
            }

            alunoApp.Dispose();
        }

        private void btnNovo_Click(object sender, EventArgs e)
        {
            iCodigo = 0;
            HabilitaCampos(true);
            txtNome.Focus();
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            HabilitaCampos(true);
        }

        private void btnGravar_Click(object sender, EventArgs e)
        {
            alunoApp = new ServicoDeAluno();
            objAluno = new Aluno();
            lbMensagem.Visible = false;
            bool result = true;

            if (!String.IsNullOrEmpty(txtNome.Text))
            {
                if (iCodigo == 0)
                {
                    objAluno.Nome = txtNome.Text;
                    result = alunoApp.Adicionar(objAluno);
                }
                else
                {
                    objAluno = alunoApp.GetById(iCodigo);
                    objAluno.Nome = txtNome.Text;
                    result = alunoApp.Alterar(objAluno);
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

            alunoApp.Dispose();
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            alunoApp = new ServicoDeAluno();
            objAluno = new Aluno();
            lbMensagem.Visible = false;

            if (MessageBox.Show("Deseja excluir esse aluno?", "Excluir", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (iCodigo > 0)
                {
                    objAluno = alunoApp.GetById(iCodigo);
                    if (alunoApp.Excluir(objAluno))
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

            alunoApp.Dispose();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            EstadoInicial();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            lbMensagem.Visible = false;
            var telaConsulta = new ConsultaAlunos();
            telaConsulta.ShowDialog();

            if (telaConsulta.iCodigo > 0)
            {
                iCodigo = telaConsulta.iCodigo;
                CarregaAluno();
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
