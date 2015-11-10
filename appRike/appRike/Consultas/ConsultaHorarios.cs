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
    public partial class ConsultaHorarios : Form
    {
        public int iCodigo = 0;
        private ServicoDeHorario horarioApp;
        private List<Horario> listAll;
        private DataTable dt;

        public ConsultaHorarios()
        {
            InitializeComponent();
            dt = new DataTable();
            EstadoInicial();
        }

        public void EstadoInicial()
        {
            listAll = null;
            dt.Columns.Add("Id");
            dt.Columns.Add("Horario");
            CarregaHorarios();
        }

        public void CarregaHorarios()
        {
            horarioApp = new ServicoDeHorario();
            listAll = horarioApp.GetAll().OrderBy(x => x.Ordem).ToList();
            MontarDataTable();
            horarioApp.Dispose();
        }

        public void PesquisaHorario(string descricao)
        {
            horarioApp = new ServicoDeHorario();
            listAll = horarioApp.Get(x => x.Dia.ToUpper().Contains(descricao.ToUpper())).OrderBy(x => x.Dia).ToList();
            MontarDataTable();
            horarioApp.Dispose();
        }

        public void MontarDataTable()
        {
            string horario;
            dt.Clear();

            foreach (var item in listAll)
            {                
                horario = item.Dia + " das " + item.HoraInicial + " às " + item.HoraFinal;
                dt.Rows.Add(item.Id, horario);
            }

            dgvHorarios.DataSource = dt;
        }

        private void txtDescricao_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                PesquisaHorario(txtDescricao.Text);
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            PesquisaHorario(txtDescricao.Text);
        }

        private void dgvHorarios_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvHorarios.CurrentRow != null)
            {
                iCodigo = Convert.ToInt32(dgvHorarios.CurrentRow.Cells["Id"].Value);
                Close();
            }
        }
        
    }
}
