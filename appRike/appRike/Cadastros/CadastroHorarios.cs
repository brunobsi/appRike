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
    public partial class CadastroHorarios : Form
    {
        private ServicoDeHorario horarioApp;
        private Horario objHorario;
        private int iCodigo;

        public CadastroHorarios()
        {
            InitializeComponent();            
            EstadoInicial();
        }

        public void EstadoInicial()
        {
            iCodigo = 0;
            cbbDia.SelectedIndex = 0;
            dtpHoraInicial.Value = new DateTime(1900, 1, 1, 8, 0, 0);
            dtpHoraFinal.Value = new DateTime(1900, 1, 1, 9, 0, 0);
            lbMensagem.Visible = false;
            HabilitaCampos(false);
        }

        public void HabilitaCampos(bool bHabilita, bool bCarregado = false)
        {
            lbMensagem.Visible = false;
            cbbDia.Enabled = bHabilita;
            dtpHoraInicial.Enabled = bHabilita;
            dtpHoraFinal.Enabled = bHabilita;
            btnNovo.Enabled = (!bHabilita && !bCarregado);
            btnAlterar.Enabled = (!bHabilita && bCarregado);
            btnGravar.Enabled = bHabilita;
            btnExcluir.Enabled = (!bHabilita && bCarregado);
            btnBuscar.Enabled = !bHabilita;
        }

        public void CarregaHorario()
        {
            horarioApp = new ServicoDeHorario();
            var horario = horarioApp.GetById(iCodigo);
            int hora, minuto;
            DateTime dtt;

            if (horario != null)
            {
                cbbDia.SelectedItem = horario.Dia;

                hora = Convert.ToInt32(horario.HoraInicial.Substring(0, 2));
                minuto = Convert.ToInt32(horario.HoraInicial.Substring(3, 2));
                dtt = new DateTime(1900, 1, 1, hora, minuto, 0);
                dtpHoraInicial.Value = dtt;

                hora = Convert.ToInt32(horario.HoraFinal.Substring(0, 2));
                minuto = Convert.ToInt32(horario.HoraFinal.Substring(3, 2));
                dtt = new DateTime(1900, 1, 1, hora, minuto, 0);
                dtpHoraFinal.Value = dtt;

                HabilitaCampos(false, true);
            }
            else
            {
                AlterarMsg("Horario não encontrado!", true);
            }

            horarioApp.Dispose();
        }

        private void btnNovo_Click(object sender, EventArgs e)
        {
            iCodigo = 0;
            HabilitaCampos(true);
            cbbDia.Focus();
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            HabilitaCampos(true);
        }

        private void btnGravar_Click(object sender, EventArgs e)
        {
            horarioApp = new ServicoDeHorario();
            objHorario = new Horario();
            lbMensagem.Visible = false;
            bool result = true;

            objHorario.HoraInicial = dtpHoraInicial.Value.Hour.ToString().PadLeft(2, '0') + ":" + dtpHoraInicial.Value.Minute.ToString().PadLeft(2, '0');
            objHorario.HoraFinal = dtpHoraFinal.Value.Hour.ToString().PadLeft(2, '0') + ":" + dtpHoraFinal.Value.Minute.ToString().PadLeft(2, '0');
            objHorario.Dia = cbbDia.SelectedItem.ToString();

            if (iCodigo == 0)
            {
                result = horarioApp.Adicionar(objHorario);
            }
            else
            {
                if (horarioApp.VerificaExistente(objHorario))
                {
                    result = false;
                }
                else
                {
                    var horarioBanco = horarioApp.GetById(iCodigo);
                    horarioBanco.HoraInicial = objHorario.HoraInicial;
                    horarioBanco.HoraFinal = objHorario.HoraFinal;
                    horarioBanco.Dia = objHorario.Dia;
                    result = horarioApp.Alterar(horarioBanco);
                }
            }

            if (result)
            {
                EstadoInicial();
                AlterarMsg("Gravado com sucesso!", false);
            }
            else
                AlterarMsg("Erro ao gravar. Verifique se o horário já não está cadastrado!", true);

            horarioApp.Dispose();
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            horarioApp = new ServicoDeHorario();
            objHorario = new Horario();
            lbMensagem.Visible = false;

            if (MessageBox.Show("Deseja excluir esse horario?", "Excluir", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (iCodigo > 0)
                {
                    objHorario = horarioApp.GetById(iCodigo);
                    if (horarioApp.Excluir(objHorario.Id))
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
                    AlterarMsg("Nenhum horario para excluir!", true);
            }

            horarioApp.Dispose();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            EstadoInicial();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            lbMensagem.Visible = false;
            var telaConsulta = new ConsultaHorarios();
            telaConsulta.ShowDialog();

            if (telaConsulta.iCodigo > 0)
            {
                iCodigo = telaConsulta.iCodigo;
                CarregaHorario();
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
